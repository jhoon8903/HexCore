using HexaCoreVillage.Utility;
using Newtonsoft.Json;
using System.Text;

namespace HexaCoreVillage.Dungeon
{
    public static class Debugging
    {
        public enum SolutionTypes{ ModifyVariables, FixTypos, ModifyScope, EditPath, CheckMemory }
    }
    public class Battle  : Scene
    {
        public override SCENE_NAME SceneName => SCENE_NAME.BATTLE;
        private static Player? Player { get; set; }
        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string BugFilePath = Path.GetFullPath(Path.Combine(BaseDirectory, "..", "..", "..", "..", "..", "TeamDungeon", "HexaCoreVillage", "Utility", "BugList.json"));
        private static List<Bug>? _bugList = new List<Bug>();
        private static List<Bug>? _selectedBugs;
        private static readonly Random Random = new Random();

        # region Start
        /// <summary>
        ///  Player 데이터 파싱
        /// </summary>
        public override void Start()
        {
            Player = new Player();
            Player.BugPercentage += 15;
            ListOfBug();
        }
        #endregion

        #region Loop Logic
        public override void Update()
        {
            Compiling();
            List<Bug> selectBugs = DebuggingList();
            if (selectBugs != null)
            {
                Data.BattleSuccess = Debugging(selectBugs);
            }
            else
            {
                Escape();
            }
        }
        #endregion

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
            CursorVisible = false;
            while (true)
            {
                // "....."이 10초 단위로 Loop 하면서 초기화 진행
                for (int i = 0; i < 10; i++)
                {
                    Clear();
                    WriteLine("\n\n\t[Running To Project]\n");
                    // 초당 1개씩 Dot을 추가
                    Write("\tCompiling" + new string('.', i));
                    
                    if (Random.Next(0,100) <=  Player.BugPercentage)
                    {
                        FoundBugs();
                        return;
                    }    
                    ConsoleSizeUtility.RedrawBorder();
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        ///  Json에서 받아온 버그 리스트 중 랜덤하게  버그 리스트 제공
        ///  버그의 난이도에 따라서 5이상 5 이하로 구분하여 진행
        /// </summary>
        private static void FoundBugs()
        {
            int bugsCount = 0;
            _selectedBugs = new List<Bug>();
            bugsCount = Player.Level switch
            {
                1 => Random.Next(1, 4),
                2 =>  Random.Next(1, 5),
                3 =>  Random.Next(2, 5),
                4 =>  Random.Next(3, 6),
                5 =>  Random.Next(4, 7),
                _ => 8
            };

            int count = Random.Next(1, bugsCount + 1);

            for (int i = 0; i < count; i++)
            {
                if (_bugList == null || _bugList.Count == 0)
                {
                    continue;
                }

                Bug selectedBug;
                do
                {
                    int index = Random.Next(_bugList.Count);
                    selectedBug = _bugList[index];
                }
                while (Player.Level <= 5 && selectedBug.BugDifficulty > 5);

                _selectedBugs.Add(selectedBug);
            }
        }

        /// <summary>
        ///  Console에 FoundBugs 중에서 고른 Bug 리스트의 첫 번재 버그를 출력
        /// </summary>
        /// <returns>선택한 Bug 리스트를 반환</returns>
        private static List<Bug> DebuggingList()
        {
            CursorVisible = false;
            Bug selectedBug = _selectedBugs[Random.Next(_selectedBugs.Count)];
            List<Bug> selectedBugs = new List<Bug>(); 
            selectedBugs.Add(selectedBug);
            selectedBugs.AddRange(_selectedBugs.Where(b => b != selectedBug));
            int choiceOption = 0;

            while (true)
            {
                Clear();
                WriteLine("\n\n\t[디버깅 - 버그 확인]\n");
                WriteLine($"\t[{Player.NickName}의 상태]"); 
                WriteLine($"\t레벨 : {Player.Level}");
                WriteLine($"\t체력 : {Player.CurrentHp} / {Player.HP}"); 
                WriteLine($"\t디버깅 능력 : {Player.TypingSpeed}");
                WriteLine($"\t버그대응 능력 : {Player.C}\n"); 
                Write("\t");
                BackgroundColor = ConsoleColor.Red;
                ForegroundColor = ConsoleColor.Black;
                Write($"[Compile Error!!]\n");
                ResetColor();
                Write("\t");
                BackgroundColor = ConsoleColor.Red;
                ForegroundColor = ConsoleColor.Black;
                Write($"[{selectedBugs.Count}개의 버그가 확인 되었습니다! - 확인 된 버그는 다음과 같습니다.]\n\n");
                ResetColor();

                if (choiceOption == 0)
                {
                    ForegroundColor = ConsoleColor.Green;
                }
                WriteLine(
                    $"\t버그명 : {selectedBugs[0].BugName}" +
                    $"\n\t버그 설명 : {selectedBugs[0].BugDesc}" +
                    $"\n\t디버깅 난이도 : {selectedBugs[0].BugDifficulty} | 디버깅 복잡도 : {selectedBugs[0].BugComplexity} | 디버깅 진행률 : {selectedBugs[0].BugProgress} %\n");
                ResetColor();

                if (choiceOption == 1)
                {
                    ForegroundColor = ConsoleColor.Red;
                }
                WriteLine("\t컴파일 종료"); 
                ResetColor();
                ConsoleSizeUtility.RedrawBorder();
                ConsoleKeyInfo  key = ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        choiceOption = 1 - choiceOption;
                        break;
                    case ConsoleKey.Enter:
                        return choiceOption == 0 ? selectedBugs : null;
                }
            }
        }
        # endregion

         #region Escape Action
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

        #region Battle Action

        private static bool Debugging(List<Bug> selectBugs)
        {
            int consoleWidth = WindowWidth;
            int splitPosition = consoleWidth / 2; // Determine the split position

            for (int i = 0; i < WindowHeight; i++)
            {
                SetCursorPosition(splitPosition, i);
                Write("|"); // Drawing a dividing line
            }

            // Example of writing to the left side
            SetCursorPosition(0, 0); // Set cursor to top left
            WriteLine("Left side of the split");

            // Example of writing to the right side
            SetCursorPosition(splitPosition + 1, 0); // Set cursor to top right
            WriteLine("Right side of the split");

            ConsoleSizeUtility.RedrawBorder();
            // Prevent console from closing immediately
            ReadLine();
            return true;
        }

        #endregion
    }
}
