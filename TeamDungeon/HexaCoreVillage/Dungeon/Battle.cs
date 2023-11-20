using HexaCoreVillage.Utility;
using Newtonsoft.Json;
using System.Text;
using static HexaCoreVillage.Dungeon.Debugging;
using static HexaCoreVillage.Utility.ConsoleSizeUtility;
using static HexaCoreVillage.Utility.DividerLineUtility;

namespace HexaCoreVillage.Dungeon
{
    #region Debugging Solution
    // 디버깅 솔루션 클래스
    public static class Debugging
    {
        public enum SolutionTypes{ ModifyVariables, FixTypos, ModifyScope, EditPath, CheckMemory }
    }
    #endregion

    #region Logging Text Data
    // 디버깅 솔루션에 따른 Logging Data Class
    public class LoggingText
    {
        public List<string> ModifyVariables { get; set; }
        public List<string> FixTypos { get; set; }
        public List<string> ModifyScope { get; set; }
        public List<string> EditPath { get; set; }
        public List<string> CheckMemory { get; set; }
    }
    #endregion

    public class Battle  : Scene
    {
        public override SCENE_NAME SceneName => SCENE_NAME.BATTLE;
        private static Player? Player { get; set; }
        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string BugFilePath = Path.GetFullPath(Path.Combine(BaseDirectory, "..", "..", "..", "..", "..", "TeamDungeon", "HexaCoreVillage", "Utility", "BugList.json"));
        private static readonly string LoggingFilePath = Path.GetFullPath(Path.Combine(BaseDirectory, "..", "..", "..",
            "..", "..", "TeamDungeon", "HexaCoreVillage", "Utility", "DebugText.json"));
        private static List<Bug>? _bugList = new List<Bug>();
        private static List<Bug>? _selectedBugs;
        private static readonly Random Random = new Random();
        private static SolutionTypes _solution;
        private static int _currentBugIndex;
        private static List<Bug> _debuggingBugs = new List<Bug>();
        private static Bug _currentBug = new Bug();
        private static List<LoggingText> _loggingText = new List<LoggingText>();
        private static int _cursorTop = 12;
        private static int _currentHP;
        private static int _currentMental;

        # region Deserialize JSON
        /// <summary>
        ///  Player 데이터 파싱
        /// </summary>
        public override void Start()
        {
            Player = new Player();
            Player.BugPercentage += 15;
            _currentHP = Player.CurrentHp;
            _currentMental = Player.CurrentMental;
            ListOfBug();
            LoggingTextLoad();
        }

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

        /// <summary>
        ///  로깅 리스트 역직렬화
        /// </summary>
        private static void LoggingTextLoad()
        {
            using StreamReader json = new(LoggingFilePath, Encoding.UTF8);
            string file = json.ReadToEnd();
            _loggingText = JsonConvert.DeserializeObject<List<LoggingText>>(file);
        }
        #endregion

        #region Loop Logic
        public override void Update()
        {
            Compiling();
           DebuggingList();
            if (_debuggingBugs != null)
            {
                Data.BattleSuccess = Debugging();
                // Reward 호출
            }
            else
            {
                Escape();
            }
        }
        #endregion

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
                    WriteLine("\n\n\t[ Running To Project ]\n");

                    // 초당 1개씩 Dot을 추가
                    Write("\tCompiling" + new string('.', i));
        
                    if (Random.Next(0,100) <=  Player.BugPercentage)
                    {
                        FoundBugs();
                        return;
                    }
                    RedrawBorder();
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
            _selectedBugs = new List<Bug>();
            int bugsCount = Player.Level switch
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
        private static void DebuggingList()
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
                WriteLine("\n\n\t[ 디버깅 - 버그 확인 ]\n");
                WriteLine($"\t[ {Player.NickName}의 상태 ]"); 
                WriteLine($"\t레벨 : {Player.Level}");
                WriteLine($"\t체력 : {Player.CurrentHp} / {Player.HP}"); 
                WriteLine($"\t디버깅 능력 : {Player.TypingSpeed}");
                WriteLine($"\t버그대응 능력 : {Player.C}\n"); 
                Write("\t");
                BackgroundColor = ConsoleColor.Red;
                ForegroundColor = ConsoleColor.Black;
                Write("[ Compile Error!! ]\n");
                ResetColor();
                Write("\t");
                BackgroundColor = ConsoleColor.Red;
                ForegroundColor = ConsoleColor.Black;
                Write($"[ {selectedBugs.Count}개의 버그가 확인 되었습니다! - 확인 된 버그는 다음과 같습니다. ]\n\n");
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
                RedrawBorder();
                ConsoleKeyInfo  key = ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        choiceOption = 1 - choiceOption;
                        break;
                    case ConsoleKey.Enter:
                        _debuggingBugs = choiceOption == 0 ? selectedBugs : null;
                        return;
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
                WriteLine("[ Warning ]");
                ResetColor();
                WriteLine("\n[ 이대로 나가시면 스테미나가 감소합니다. ]\n");
                WriteLine("[ 정말 컴파일을 종료하시겠습니까? ]");

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
                RedrawBorder();
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
        private static bool Debugging()
        {
            Clear();
            const int splitPosition = FixedColumns / 4; // 분할 위치 결정
            for (int i = 0; i < FixedRows; i++) // 분할선 그리기
            {
                SetCursorPosition(splitPosition, i);
                BackgroundColor = ConsoleColor.Yellow;
                Write(" "); // 분할선 그리기
                ResetColor();
            }

            // 왼쪽 영역에 텍스트 출력
            RightWindow();
            LeftWindow();
            RedrawBorder();
            ReadLine(); // 콘솔 창이 즉시 닫히는 것을 방지
            return true;
        }
        #endregion

        #region LEFT WINDOWS
        private static void LeftWindow()
        {
            // Left Top
            PlayerInformation();
            // Left Middle
            SelectToDebugging();
        }
        private static void PlayerInformation()
        {
                SetCursorPosition(4, 1);
                BackgroundColor = ConsoleColor.Green;
                ForegroundColor = ConsoleColor.Black;
                Write("[ Running To Debug ]");

                SetCursorPosition(4, CursorTop + 2);
                ResetColor();
                Write($"[ {Player.NickName} 상태 ]");

                SetCursorPosition(4, CursorTop + 2);
                Write($"레벨 : {Player.Level}");

                SetCursorPosition(4, CursorTop + 1);
                Write($"체력 : {_currentHP} / {Player.HP}");

                SetCursorPosition(4, CursorTop + 1);
                Write($"타이핑 속력 (DMG) : {Player.TypingSpeed}");

                SetCursorPosition(4, CursorTop + 1);
                Write($"C# 언어 능력 (DEF)  : {Player.C}");

                DividerLine(1,CursorTop + 2, ConsoleColor.Yellow);
        }
        private static void SelectToDebugging()
        {
            CursorVisible = false;
            int debuggingOption = 0;
            string[] options = Enum.GetNames(typeof(SolutionTypes));

            while (true)
            {
                SetCursorPosition(4,  12);
                BackgroundColor = ConsoleColor.Green;
                ForegroundColor = ConsoleColor.Black;
                Write("[ 디버깅 솔루션을 선택하세요 ]");
                ResetColor();
                for (int i = 0; i < options.Length; i++)
                {
                    SetCursorPosition(4, 14 + i);
                    if (i == debuggingOption)
                    {
                        BackgroundColor = ConsoleColor.Green;
                        ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        ResetColor();
                    }
                    Write(options[i]);
                    ResetColor();
                }
                RedrawBorder();
                DividerLine(1,20, ConsoleColor.Yellow);
                SolutionTypes solution = (SolutionTypes)Enum.Parse(typeof(SolutionTypes), options[debuggingOption]);
                DescriptionSolution(solution);
                ConsoleKeyInfo key = ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        debuggingOption = (debuggingOption - 1 + options.Length) % options.Length; 
                        DescriptionSolution(solution);
                        break;
                    case ConsoleKey.DownArrow:
                        debuggingOption = (debuggingOption + 1) % options.Length;
                        DescriptionSolution(solution);
                        break;
                    case ConsoleKey.Enter:
                        _solution = solution;
                        InputSolution();
                        if (Data.BattleSuccess)
                        {
                            return;
                        }
                        break;
                }
            }
        }
        private static void DescriptionSolution(SolutionTypes solution)
        {
            SetCursorPosition(4, 22);
            WriteLine(new string(' ', 40));
            SetCursorPosition(4, 22);
            Write($"[ {solution.ToString().ToUpper()} 솔루션 설명 ]\n\n");

            switch (solution)
            {
                case SolutionTypes.ModifyVariables:
                    WriteDesc("프로그램 실행 중 변수값", 24);
                    WriteDesc("변경 사항 반영", 25);
                    WriteDesc("사용자 입력 기반 업데이트", 26);
                    WriteDesc("변수값 수정으로 오류 해결", 27);
                    break;
                case SolutionTypes.FixTypos:
                    WriteDesc("코드 내 오타 수정 필요", 24);
                    WriteDesc("변수명, 함수명 오류 검사", 25);
                    WriteDesc("정확한 코드 작성 중요", 26);
                    WriteDesc("예기치 못한 버그 예방", 27);
                    break;
                case SolutionTypes.ModifyScope:
                    WriteDesc("변수 및 함수 범위 조정", 24);
                    WriteDesc("코드 가독성 향상", 25);
                    WriteDesc("적절한 스코프 설정", 26);
                    WriteDesc("프로그램 안정성 향상", 27);
                    break;
                case SolutionTypes.EditPath:
                    WriteDesc("파일/디렉토리 경로 수정", 24);
                    WriteDesc("정확한 파일 접근 중요", 25);
                    WriteDesc("경로 오류 해결 필요", 26);
                    WriteDesc("프로그램 실행 보장", 27);
                    break;
                case SolutionTypes.CheckMemory:
                    WriteDesc("메모리 사용 상태 점검", 24);
                    WriteDesc("메모리 누수 탐지 및 해결", 25);
                    WriteDesc("성능 및 안정성 최적화", 26);
                    WriteDesc("효율적 메모리 관리", 27);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(solution), solution, null);
            }
        }
        private static void WriteDesc(string desc, int cursorTop)
        {
            SetCursorPosition(4, cursorTop);
            Write(new string(' ', 40));
            SetCursorPosition(4, cursorTop);
            Write(desc); 
            DividerLine(1,29,ConsoleColor.Yellow); 
            Logo();
        }
        // private static void Logo()
        // {
        //     string[] asciiArt = {
        //         "■■   ■■ ■■■■■■■ ■■   ■■  ■■■■■",  
        //         "■■   ■■ ■■       ■■ ■■  ■■   ■■", 
        //         "■■■■■■■ ■■■■■     ■■■   ■■■■■■■", 
        //         "■■   ■■ ■■       ■■ ■■  ■■   ■■", 
        //         "■■   ■■ ■■■■■■■ ■■   ■■ ■■   ■■"
        //     };
        //
        //
        //     ConsoleColor[] colors = {
        //         ConsoleColor.Red,
        //         ConsoleColor.Yellow,
        //         ConsoleColor.Green,
        //         ConsoleColor.Cyan,
        //         ConsoleColor.Blue,
        //         ConsoleColor.Magenta
        //     };
        //
        //     for (int i = 0; i < asciiArt.Length; i++)
        //     {
        //         SetCursorPosition(6, 32 + i);
        //         ForegroundColor = colors[i % colors.Length];
        //         WriteLine(asciiArt[i]);
        //     }
        //
        //     ResetColor();
        // }
        private static void Logo()
        {
            string[] asciiArt = {
                "   ■■   ■      ■   ■■■   ■■■■   ",
                "   ■■   ■   ■■■■■   ■   ■■■   ■   ",
                "        ■      ■■■■   ■■■■         ",
                "   ■■   ■   ■■■■■   ■   ■■   ■■■   ",
                "   ■■   ■      ■   ■■■   ■   ■■■   "
            };

            // {
            //     string[] asciiArt = {
            //         "■■   ■■ ■■■■■■■ ■■   ■■  ■■■■■",  
            //         "■■   ■■ ■■       ■■ ■■  ■■   ■■", 
            //         "■■■■■■■ ■■■■■     ■■■   ■■■■■■■", 
            //         "■■   ■■ ■■       ■■ ■■  ■■   ■■", 
            //         "■■   ■■ ■■■■■■■ ■■   ■■ ■■   ■■"
            //     };

            ConsoleColor[] colors = {
                ConsoleColor.Red,
                ConsoleColor.Yellow,
                ConsoleColor.Green,
                ConsoleColor.Cyan,
                ConsoleColor.Blue, 
                ConsoleColor.Magenta
            };

            ConsoleColor bgColor = BackgroundColor; // Assuming background color is black

            for (int i = 0; i < asciiArt.Length; i++)
            {
                SetCursorPosition(5, 32 + i);

                foreach (char c in asciiArt[i])
                {
                    if (c == '■')
                    {
                        // For special character, set background color same as current background
                        BackgroundColor = bgColor;
                        ForegroundColor = bgColor; // Set foreground color to match background
                        Write(' '); // Print space with background color
                    }
                    else
                    {
                        // For spaces, set background color to one of the specified colors
                        BackgroundColor = colors[i % colors.Length];
                        ForegroundColor = colors[i % colors.Length]; // Set foreground to match background
                        Write(' '); // Print space with colored background
                    }
                }

                WriteLine(); // Move to the next line after printing each row
                BackgroundColor = bgColor; // Reset background color for new line
            }

            ResetColor(); // Reset to default colors
        }
        #endregion

        #region RIGHT WINDOWS
        private static void RightWindow()
        {
            BugStatus();
            RightBottomDisplay();
        }

        private static void BugStatus()
        {
            if (_currentBugIndex >= _debuggingBugs.Count)
            {
                WriteLine("디버그를 완료 하였습니다."); 
                return;
            }
            _currentBug = _debuggingBugs[_currentBugIndex];
            SetCursorPosition(50,2);
            Write($"[BugCount : {_currentBugIndex+1} / {_debuggingBugs.Count}]");
            ClearLine(4);
            SetCursorPosition(50,4);
            Write($"[ Bug : {_currentBug.BugName} ]");
            ClearLine(5);
            SetCursorPosition(50,5);
            Write($"[ Desc : {_currentBug.BugDesc} ]");
            ClearLine(6);
            SetCursorPosition(50,6);
            Write($"[ Debug Difficulty : {_currentBug.BugDifficulty} ]");
            ClearLine(7);
            SetCursorPosition(50,7);
            Write($"[ Debug Complexity : {_currentBug.BugComplexity} ]");
            ClearLine(8);
            SetCursorPosition(50,8);
            Write($"[ Debug Process : {_currentBug.BugProgress:D3} % ]  ");
            const int totalBlocks = 100;
            int greenBlocks = _currentBug.BugProgress;

            for (int i = 0; i < totalBlocks; i++)
            {
                BackgroundColor = i < greenBlocks ? ConsoleColor.Green : ConsoleColor.DarkRed;
                Write(' '); // 공백으로 진행률 표시
            }
            ResetColor();
        }

        private static void InputSolution()
        {
            SetCursorPosition(50, 10);
            
            // 데미지 적용 부분 수정 필요
            int progressIncrease = _currentBug.SolutionType == _solution ? Random.Next(1, 20) : Random.Next(1, 10);

            _currentBug.BugProgress += progressIncrease;

            if (_currentBug.BugProgress >= 100)
            {
                _currentBug.BugProgress = 100;
                BugStatus(); // 현재 버그 상태 업데이트

                if (_currentBugIndex + 1 >= _debuggingBugs.Count)
                {
                    ClearLine(10);
                    SetCursorPosition(50, 10);
                    // 모든 버그를 디버깅 완료했을 경우
                    WriteLine("디버그를 완료 하였습니다.");
                }
                else
                {
                    ClearLine(10);
                    SetCursorPosition(50, 10);
                    // 다음 버그로 넘어가기 전에 메시지 출력
                    WriteLine("현재 버그 디버깅 완료. 다음 버그로 이동합니다.");
                    _currentBugIndex++; // 다음 버그로 이동
                }
            }
            else
            {
                BugStatus(); // 버그 진행률 업데이트
            }
            PlayerLoggingDisplay();
        }

        private static void PlayerLoggingDisplay()
        {
            LoggingText loggingText = _loggingText.First();
            List<string> logs = _solution switch
            {
                SolutionTypes.ModifyVariables => loggingText.ModifyVariables,
                SolutionTypes.FixTypos => loggingText.FixTypos,
                SolutionTypes.ModifyScope => loggingText.ModifyScope,
                SolutionTypes.EditPath => loggingText.EditPath,
                SolutionTypes.CheckMemory => loggingText.CheckMemory,
                _ => new List<string>()
            };

            if (!logs.Any()) return;

            int index = Random.Next(logs.Count);
            string logMessage = logs[index];

            ClearLine(_cursorTop);
            SetCursorPosition(50, _cursorTop);
            WriteLine(logMessage);

            if (_cursorTop >= 26)
            {
                _cursorTop = 12;
            }
            else
            {
                _cursorTop++;
            }
            BugLoggingDisplay();
        }

        private static void BugLoggingDisplay()
        { 
            ClearLine(_cursorTop);
            SetCursorPosition(50, _cursorTop);
            ForegroundColor = ConsoleColor.Red; 
            Write($"디버깅 어려움과 복잡도로 인해서  플레이어의 체력이 {_currentBug.BugDifficulty}, 멘탈이 {_currentBug.BugComplexity} 감소 했습니다.");
            ResetColor();
            _cursorTop++;
            _currentHP -= _currentBug.BugDifficulty;
            _currentMental -= _currentBug.BugComplexity;
            RightBottomDisplay();
        }

        private static void RightBottomDisplay()
        {
            // 라인 그리기
          SetCursorPosition(46,29);
          for (int i = 0; i < 135; i++)
          {
              BackgroundColor = ConsoleColor.Yellow;
              Write(" ");
              ResetColor();
          }
          
            // 체력 바 그리기
          SetCursorPosition(50, 31);
          Write($"[ 체력 : {_currentHP:D3} / {Player.HP:D3} ]  ");
          int staminaBlocks = _currentHP;
          for (int i = 0; i < Player.HP; i++)
          {
              BackgroundColor = i < staminaBlocks ? 
                  staminaBlocks > 60 ? ConsoleColor.DarkGreen :
                  staminaBlocks > 30 ? ConsoleColor.DarkYellow : 
                  ConsoleColor.DarkRed : 
                  ConsoleColor.Gray; // 미사용 부분은 회색으로 표시
              Write(' '); // 공백으로 진행률 표시
          }
          ResetColor();

          // 멘탈 바 그리기
          SetCursorPosition(50, 32);
          Write($"[ 멘탈 : {_currentHP:D3} / {Player.Mental:D3} ]  ");
          int mentalBlock = _currentMental;
          for (int i = 0; i < Player.Mental; i++)
          {
              BackgroundColor = i < mentalBlock ? 
                  mentalBlock > 60 ? ConsoleColor.Cyan :
                  mentalBlock > 30 ? ConsoleColor.Blue : 
                  ConsoleColor.DarkRed : 
                  ConsoleColor.Gray; // 미사용 부분은 회색으로 표시
              Write(' '); // 공백으로 진행률 표시
          }
          ResetColor();

          // 디버깅 추가 확률
          SetCursorPosition(50, 34);
          Write($"[ 디버깅 추가 성능 : {Player.BonusDmg} ]  [ 언어 추가 이해력 : {Player.BonusDef} ]");
          
          // 멘트
          SetCursorPosition(50, 36);
          Write("[ 멘탈이 일정 수준 감소할 때 마다 디버깅 확률 및 디버깅 능력이 감소합니다. ]");
        }

        private static void ClearLine(int cursorTop)
        {
            SetCursorPosition(50, cursorTop);
            Write(new string(' ', 120 ));
        }
        #endregion
    }


}
