using HexaCoreVillage.Utility;
using static HexaCoreVillage.Utility.DividerLineUtility;
using static HexaCoreVillage.Dungeon.Battle;

namespace HexaCoreVillage.Dungeon
{ 
    public static class BattleLeftWindow
    {
        #region Left Procedure
        /// <summary>
        ///  Left 레이아웃 실행 메서드
        /// </summary>
        public static void LeftWindow()
        {
            // Left Top
            PlayerInformation();
            // Left Middle
            SelectToDebugging();
        }
        #endregion

        #region Player Layout
        /// <summary>
        ///  플레이어의 정보를 받아오는 메서드
        ///  Battle Class의 Player 인스턴스를 참조
        /// </summary>
        private static void PlayerInformation()
        {
            SetCursorPosition(4, 1);
            BackgroundColor = ConsoleColor.Green;
            ForegroundColor = ConsoleColor.Black;
            Write("[ Running To Debug ]");

            SetCursorPosition(4, CursorTop + 2);
            ResetColor();
            Write($"[ {LoginPlayer!.NickName} 상태 ]");

            SetCursorPosition(4, CursorTop + 2);
            Write($"레벨 : {LoginPlayer.Level}");

            SetCursorPosition(4, CursorTop + 1);
            Write($"체력 : {CurrentHp} / {LoginPlayer.HP}");

            SetCursorPosition(4, CursorTop + 1);
            Write($"타이핑 속력 (DMG) : {LoginPlayer.TypingSpeed}");

            SetCursorPosition(4, CursorTop + 1);
            Write($"C# 언어 능력 (DEF)  : {LoginPlayer.C}");

            DividerLine(1,CursorTop + 2, ConsoleColor.Yellow);
        }
        /// <summary>
        ///  디버깅 솔루션을 선택하는 메서드
        ///  Battle.Solution 변수를 할당
        /// </summary>
        /// <returns>솔루션 Class 를 할당하여 반환</returns>>
        private static void SelectToDebugging()
        {
            CursorVisible = false;
            int debuggingOption = 0;
            string[] options = Enum.GetNames(typeof(Debugging.SolutionTypes));

            while (true)
            {
                if (_isLoadScene) return;
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
                Renderer.Instance.DrawConsoleBorder();
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
                        Solution = solution;
                        InputSolution();
                        if (Data.BattleSuccess)
                        {
                            return;
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///  솔루션 선택시 RightWindow로 선책한 솔루션 데이터를 전달
        ///  버그의 진행률을 높히는 공격 메서드,
        /// </summary>
        private static void InputSolution()
        {
            // 데미지 적용 부분 수정 필요
            SetCursorPosition(50, 10);
            if (LoginPlayer != null)
            {
                int damage = LoginPlayer.TypingSpeed + LoginPlayer.BonusDmg;
                int minDamage = damage / 3;
                int randomDamage = Battle.Random.Next(minDamage, damage);
                int progressIncrease = CurrentBug.SolutionType == Solution ? randomDamage * 2 : randomDamage;

                CurrentBug.BugProgress += progressIncrease;
            }

            if (CurrentBug.BugProgress >= 100)
            {
                CurrentBug.BugProgress = 100; 
                BattleRightWindow.BugStatus();
                CurrentBugIndex++; // 다음 버그로 이동
                if (CurrentBugIndex >= DebuggingBugs.Count && CurrentBug.BugProgress >= 100)
                {
                    BattleRightWindow.DebuggingSuccess(); 
                    return;
                }
                ClearLogging(11,28);
                SetCursorPosition(50, 10);
                WriteLine("현재 버그 디버깅 완료. 다음 버그로 이동합니다."); 
                SetCursorPosition(50, 13);
                WriteLine("아무키나 입력 하세요");
                CurrentBug = DebuggingBugs[CurrentBugIndex];
                BattleCursorTop = 12;
                ReadKey();
            }
            else
            {
                BattleRightWindow.BugStatus(); // 버그 진행률 업데이트
                BattleRightWindow.PlayerLoggingDisplay();
            }
        }
        #endregion

        #region Solution Desc Method
        /// <summary>
        ///  솔루션 선택시 솔루션 타입을 받아
        ///  하단에 설명을 다음과 같이 출력
        /// </summary>
        /// <param name="solution"> 선택에서 솔루션을 입력</param>
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
            }
        }
        #endregion

        #region LOGO

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
        #endregion
    }
}