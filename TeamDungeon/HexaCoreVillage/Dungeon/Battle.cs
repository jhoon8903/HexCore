using HexaCoreVillage.Utility;
using Newtonsoft.Json;
using System.Text;

namespace HexaCoreVillage.Dungeon
{
    public class Battle  : Scene
    {
        public override SCENE_NAME SceneName => SCENE_NAME.BATTLE;
        private static Player? Player { get; set; }
        private static readonly Random Percentage = new Random();
        public static int BugPercentage = 15;
        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string BugFilePath = Path.GetFullPath(Path.Combine(BaseDirectory, "..", "..", "..", "..", "..", "TeamDungeon", "HexaCoreVillage", "Utility", "BugList.json"));
        private static List<Bug>? _bugList = new List<Bug>();
        private static List<Bug>? _selectedBugs;
        private const int BugsCount = 4;
        private static readonly Random Random = new Random();

        public override void Start()
        { 
            ListOfBug();
        }

        public override void Update()
        {
            Compiling();
            Bug selectBug = DebuggingList();
            if (selectBug != null)
            {
                Debugging(selectBug);               
            }
            else
            {
                Escape();
            }
        }

        # region BugList
        /// <summary>
        ///  버그 목록을 역직렬화 하여 언제든지 사용가능하도록
        ///  리스트에 담는 메서드 
        /// </summary>
        private static void ListOfBug()
        {
            using StreamReader json = new(BugFilePath, Encoding.UTF8);
            string file = json.ReadToEnd();
            _bugList = JsonConvert.DeserializeObject<List<Bug>>(file);
        } 
        # endregion

        # region Start Compiling Action
        /// <summary>
        ///  던전 (컴파일링) 입장
        ///  일정 확룰로 Bug 발견 시나리오
        ///  발견확률은 임의로 조정
        ///  추후 케릭터의 Luk 스텟이나 별도의 조정이 있다면 수정 가능
        ///  기본값은 50% 확률
        /// </summary>
        private static void Compiling()
        {
            while (true)
            {
                // "....."이 10초 단위로 Loop 하면서 초기화 진행
                for (int i = 0; i < 10; i++)
                {
                    Clear();
                    WriteLine("[Running To Project]\n");
                    // 초당 1개씩 Dot을 추가
                    Write("Compiling" + new string('.', i));
                    
                    if (Percentage.Next(0,100) <=  BugPercentage)
                    {
                        FoundBugs();
                        return;
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        ///  Json에서 받아온 버그 리스트 중 랜덤하게 1 ~ 3 개의 버그 리스트 제공
        /// </summary>
        private static void FoundBugs()
        {
            _selectedBugs = new List<Bug>();
            int count = Random.Next(1, BugsCount);
            for (int i = 0; i < count; i++)
            {
                if (_bugList == null)
                {
                    continue;
                }
                int index = Random.Next(_bugList.Count);
                _selectedBugs.Add(_bugList[index]);
            }
        }

        /// <summary>
        ///  Console에 FoundBugs 중에서 고른 Bug 리스트를 출력
        /// </summary>
        /// <returns>선택한 Bug를 반환</returns>
        private static Bug DebuggingList()
        {
            CursorVisible = false;
            int selectedIndex = 0;
            int totalOption = _selectedBugs.Count + 1;
            while (true)
            {
                Clear();
                WriteLine("[디버깅]\n");
                WriteLine("[확인 된 버그 목록]\n"); 
                WriteLine("[디버깅을 시작할 버그를 선택해주세요]\n");

                if (_selectedBugs != null)
                {
                    for (int i = 0; i < _selectedBugs.Count; i++)
                    {
                        if (i == selectedIndex)
                        {
                            ForegroundColor = ConsoleColor.Green;
                        }

                        WriteLine(
                            $"\t버그명 : {_selectedBugs[i].BugName}\n\t타입 : {_selectedBugs[i].BugType} | 디버깅 난이드 : {_selectedBugs[i].BugDifficulty} | 디버깅 복잡도 : {_selectedBugs[i].BugComplexity} | 디버깅 진행률 : {_selectedBugs[i].BugProgress}\n");
                        ResetColor();
                    }
                }

                if (selectedIndex == _selectedBugs.Count)
                {
                    ForegroundColor = ConsoleColor.Red;
                }
                WriteLine("\n\t컴파일 종료"); 
                ResetColor();

                ConsoleKeyInfo  key = ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex - 1 + totalOption) % totalOption;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % totalOption;
                        break;
                    case ConsoleKey.Enter:
                        return selectedIndex == _selectedBugs.Count ? null : _selectedBugs[selectedIndex];
                }
            }
        }
        # endregion

        #region EscapeAction
        /// <summary>
        ///  전투 도중 "컴파일 종료 선택 시 탈출 안내창 출력"
        /// </summary>
        private static void Escape()
        {
            CursorVisible = false;
            int menuIndex = 0;
            int totalOption = 2;

            while (true)
            {           
                Clear();
                BackgroundColor = ConsoleColor.Yellow;
                ForegroundColor = ConsoleColor.Black;
                WriteLine("[Warning]");
                ResetColor();
                WriteLine("\n[이대로 나가시면 스테미나가 감소합니다.]\n");
                WriteLine("[정말 컴파일을 종료하시겠습니까?]");

                string[] options = { "디버깅 돌아가기", "메인화면으로 돌아가기" };
                for (int i = 0; i < totalOption; i++)
                {
                    if (i == menuIndex)
                    {
                        ForegroundColor = ConsoleColor.Green;
                    } 
                    WriteLine($"\n{options[i]}");
                    ResetColor();
                }

                ConsoleKeyInfo key = ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        menuIndex = (menuIndex - 1 + totalOption) % totalOption;
                        break;
                    case ConsoleKey.DownArrow:
                        menuIndex = (menuIndex + 1) % totalOption;
                        break;
                    case ConsoleKey.Enter:
                        if (menuIndex == 0)
                        {
                            return;
                        }
                        if (menuIndex == 1)
                        {
                            Managers.Scene.LoadScene(SCENE_NAME.LOBBY);
                        }
                        return;
                }
            }
        }
        #endregion

        #region Battle
        private static void Debugging(Bug selectBug)
        {

        }
        

        #endregion
    }
}