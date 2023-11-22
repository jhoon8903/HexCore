using HexaCoreVillage.Utility;
using Newtonsoft.Json;
using System.Text;
using static HexaCoreVillage.Dungeon.Debugging;

namespace HexaCoreVillage.Dungeon
{
    
    #region Debugging Solution
    /// <summary>
    ///  디버깅 솔루션 클래스
    ///  ModifyVariables, FixTypos, ModifyScope, EditPath, CheckMemory 총 5가지 enum
    /// </summary>
    public static class Debugging
    {
        public enum SolutionTypes{ ModifyVariables, FixTypos, ModifyScope, EditPath, CheckMemory }
    }
    #endregion

    public class Battle  : Scene
    {
        #region SceneManager Variable
        // 씬 네임 할당 BATTLE
        public override SCENE_NAME SceneName => SCENE_NAME.BATTLE;
        #endregion

        #region Player Variable
        public static Player? Player { get; private set; }  // 더미 플레이어 데이터 
        public static int CurrentHp;    // 현재 플레이어 체력
        public static int CurrentMental;    // 현재 플레이어 멘탈
        #endregion

        #region BUG Variable
        private static List<Bug>? _bugList = new List<Bug>();   // Json 에서 나온 버그 리스트를 담는 변수
        private static List<Bug>? _selectedBugs;    // 선택되어 화면에 출력되기 위한 버그 리스트
        public static int CurrentBugIndex { get; set; } // 현재 디버깅을 진행해야하는 버그 인덱스
        public static List<Bug> DebuggingBugs { get; set; } = new List<Bug>();  // 현재 진행중인 버그의 리스트
        public static Bug CurrentBug { get; set; } = new Bug(); // 현재 디버깅 진행중인 버그 인스턴스
        #endregion

        #region SOLUTION Variable       
        public static SolutionTypes Solution;   // Solution 입력에 필요한 솔루션 변수
        public static List<LoggingText>? LoggingText = new List<LoggingText>(); // 솔루션 로깅을 위한 JSON 인스턴스
        #endregion

        public static readonly Random Random = new Random();    // 난수 생성을 위한 Random
        public static int BattleCursorTop = 12; // SetCursorTop

        # region Deserialize JSON
        /// <summary>
        ///  Player 데이터 파싱
        /// </summary>
        public override void Start()
        {
            InitializePlayer();
            LoadBugs();
            LoadLoggingText();
            AudioPlayer.AudioController(Managers.Resource.GetSoundResource(ResourceKeys.BattleBGM), AudioPlayer.PlayOption.LoopStart);
            AudioPlayer.AudioVolume(3);
        }
        
        /// <summary>
        ///  플레이어 데이터 초기화
        /// </summary>
        private static void InitializePlayer()
        {
            Player = new Player { BugPercentage = 15 };
            CurrentHp = Player.CurrentHp;
            CurrentMental = Player.CurrentMental;
        }

        /// <summary>
        ///  버그 목록을 역직렬화 하여 언제든지 사용가능하도록
        ///  리스트에 담는 메서드 
        /// </summary>
        private static void LoadBugs()
        {
            string file = Managers.Resource.GetTextResource(ResourceKeys.BugList);
            _bugList = JsonConvert.DeserializeObject<List<Bug>>(file);
        } 

        /// <summary>
        ///  로깅 리스트 역직렬화
        /// </summary>
        private static void LoadLoggingText()
        {
            string file = Managers.Resource.GetTextResource(ResourceKeys.DebugText);
            LoggingText = JsonConvert.DeserializeObject<List<LoggingText>>(file);
        }
        #endregion

        #region Loop Logic
        /// <summary>
        ///  상속 받아 반복 실행 되는 Update 
        /// </summary>
        public override void Update()
        {
            Compiling();
           DebuggingList();
            if (DebuggingBugs != null)
            {
                Debugging();
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
        
                    if (Random.Next(0,100) <=  Player!.BugPercentage)
                    {
                        FoundBugs();
                        return;
                    }
                    Renderer.Instance.DrawConsoleBorder();
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
            int bugsCount = Player!.Level switch
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
                Data.BugCount = _selectedBugs.Count;
            }
        }

        /// <summary>
        ///  Console에 FoundBugs 중에서 고른 Bug 리스트의 첫 번재 버그를 출력
        /// </summary>
        private static void DebuggingList()
        {
            CursorVisible = false;
            Bug selectedBug = _selectedBugs![Random.Next(_selectedBugs.Count)];
            List<Bug> selectedBugs = new List<Bug> { selectedBug };
            selectedBugs.AddRange(_selectedBugs.Where(b => b != selectedBug));
            int choiceOption = 0;

            while (true)
            {
                Clear();
                WriteLine("\n\n\t[ 디버깅 - 버그 확인 ]\n");
                WriteLine($"\t[ {Player!.NickName}의 상태 ]"); 
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
                Renderer.Instance.DrawConsoleBorder();
                ConsoleKeyInfo  key = ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        choiceOption = 1 - choiceOption;
                        break;
                    case ConsoleKey.Enter:
                        DebuggingBugs = (choiceOption == 0 ? selectedBugs : null)!;
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
                Renderer.Instance.DrawConsoleBorder();
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
        /// <summary>
        ///  디버깅 실행 시 화면 분할 및 디버그 로직 실행
        ///  Left, Right 두 부분으로 레이아웃을 나눠서 실행
        /// </summary>
        private static void Debugging()
        {
            Clear();
            const int splitPosition = Renderer.FixedXColumn / 4; // 분할 위치 결정
            for (int i = 0; i < Renderer.FixedYRows; i++) // 분할선 그리기
            {
                SetCursorPosition(splitPosition, i);
                BackgroundColor = ConsoleColor.Yellow;
                Write(" "); // 분할선 그리기
                ResetColor();
            }

            // 왼쪽 영역에 텍스트 출력
            BattleRightWindow.RightWindow();
            BattleLeftWindow.LeftWindow();
            Renderer.Instance.DrawConsoleBorder();
            ReadLine(); // 콘솔 창이 즉시 닫히는 것을 방지
        }
        #endregion

        #region Line Utility
        /// <summary>
        ///  로깅 영역 한줄을 지우는 메서드
        /// </summary>
        /// <param name="cursorTop"> 커서 탑을 지정해서 해당 부분을 120만큼을 ' '으로 덮어씀</param>
        public static void ClearLine(int cursorTop)
        {
            SetCursorPosition(50, cursorTop);
            Write(new string(' ', 120 ));
        }

        /// <summary>
        ///  로깅 화면의 초기화를 위한 로깅 전체를 지우는 메서드
        /// </summary>
        /// <param name="start"> int 시작 줄</param>
        /// <param name="end">int 마지막 줄</param>
        public static void ClearLogging(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                SetCursorPosition(50, i);
                Write(new string(' ', 120 ));
            }
        }

        public override void Stop()
        {
            
        }
        #endregion
    }
}
