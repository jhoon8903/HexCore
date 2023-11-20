using HexaCoreVillage.Utility;
using static HexaCoreVillage.Utility.DividerLineUtility;
using static HexaCoreVillage.Utility.ConsoleSizeUtility;

namespace HexaCoreVillage.Dungeon
{ 
    public static class BattleLeftWindow
    {
        public static void LeftWindow()
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
                Write($"[ {Battle.Player!.NickName} 상태 ]");

                SetCursorPosition(4, CursorTop + 2);
                Write($"레벨 : {Battle.Player.Level}");

                SetCursorPosition(4, CursorTop + 1);
                Write($"체력 : {Battle.CurrentHp} / {Battle.Player.HP}");

                SetCursorPosition(4, CursorTop + 1);
                Write($"타이핑 속력 (DMG) : {Battle.Player.TypingSpeed}");

                SetCursorPosition(4, CursorTop + 1);
                Write($"C# 언어 능력 (DEF)  : {Battle.Player.C}");

                DividerLine(1,CursorTop + 2, ConsoleColor.Yellow);
        }
        private static void SelectToDebugging()
        {
            CursorVisible = false;
            int debuggingOption = 0;
            string[] options = Enum.GetNames(typeof(Debugging.SolutionTypes));

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
                Debugging.SolutionTypes solution = (Debugging.SolutionTypes)Enum.Parse(typeof(Debugging.SolutionTypes), options[debuggingOption]);
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
                        Battle.Solution = solution;
                        BattleRightWindow.InputSolution();
                        if (Data.BattleSuccess)
                        {
                            return;
                        }
                        break;
                }
            }
        }
        private static void DescriptionSolution(Debugging.SolutionTypes solution)
        {
            SetCursorPosition(4, 22);
            WriteLine(new string(' ', 40));
            SetCursorPosition(4, 22);
            Write($"[ {solution.ToString().ToUpper()} 솔루션 설명 ]\n\n");

            switch (solution)
            {
                case Debugging.SolutionTypes.ModifyVariables:
                    WriteDesc("프로그램 실행 중 변수값", 24);
                    WriteDesc("변경 사항 반영", 25);
                    WriteDesc("사용자 입력 기반 업데이트", 26);
                    WriteDesc("변수값 수정으로 오류 해결", 27);
                    break;
                case Debugging.SolutionTypes.FixTypos:
                    WriteDesc("코드 내 오타 수정 필요", 24);
                    WriteDesc("변수명, 함수명 오류 검사", 25);
                    WriteDesc("정확한 코드 작성 중요", 26);
                    WriteDesc("예기치 못한 버그 예방", 27);
                    break;
                case Debugging.SolutionTypes.ModifyScope:
                    WriteDesc("변수 및 함수 범위 조정", 24);
                    WriteDesc("코드 가독성 향상", 25);
                    WriteDesc("적절한 스코프 설정", 26);
                    WriteDesc("프로그램 안정성 향상", 27);
                    break;
                case Debugging.SolutionTypes.EditPath:
                    WriteDesc("파일/디렉토리 경로 수정", 24);
                    WriteDesc("정확한 파일 접근 중요", 25);
                    WriteDesc("경로 오류 해결 필요", 26);
                    WriteDesc("프로그램 실행 보장", 27);
                    break;
                case Debugging.SolutionTypes.CheckMemory:
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
    }
}