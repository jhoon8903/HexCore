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
        private static readonly List<Bug> SelectedBugs = new List<Bug>();
        private const int BugsCount = 3;
        private static readonly Random Random = new Random();


        public override void Start()
        { 
            ListOfBug();
        }

        public override void Update()
        {
            Compiling();
            Bug selectBug = Debugging();

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

        private static void FoundBugs()
        {
            int count = Random.Next(1, BugsCount);
            for (int i = 0; i < count; i++)
            {
                if (_bugList == null)
                {
                    continue;
                }
                int index = Random.Next(_bugList.Count);
                SelectedBugs.Add(_bugList[index]);
            }
        }

        private static Bug Debugging()
        {
            CursorVisible = false;
            int selectedIndex = 0;
            while (true)
            {
                Clear();
                WriteLine("[디버깅]\n");
                WriteLine("[확인 된 버그 목록]\n"); 
                WriteLine("[디버깅을 시작할 버그를 선택해주세요]\n");

                for (int i = 0; i < SelectedBugs.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        ForegroundColor = ConsoleColor.Green;
                    }
                    WriteLine($"버그명 : {SelectedBugs[i].BugName}\n타입 : {SelectedBugs[i].BugType} | 디버깅 난이드 : {SelectedBugs[i].BugDifficulty} | 디버깅 복잡도 : {SelectedBugs[i].BugComplexity} | 디버깅 진행률 : {SelectedBugs[i].BugProgress}");
                    ResetColor();
                }

                ConsoleKeyInfo  key = ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex - 1 + SelectedBugs.Count) % SelectedBugs.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex + 1) % SelectedBugs.Count;
                        break;
                    case ConsoleKey.Enter:
                        return SelectedBugs[selectedIndex];
                }
            }
        }
        # endregion
    }
}