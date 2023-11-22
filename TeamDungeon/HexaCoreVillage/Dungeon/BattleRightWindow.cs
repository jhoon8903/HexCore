using HexaCoreVillage.Utility;
using static HexaCoreVillage.Dungeon.Battle;
using static HexaCoreVillage.Dungeon.Debugging.SolutionTypes;

namespace HexaCoreVillage.Dungeon
{
    public static class BattleRightWindow
    {
        #region RightWindow Starter
        public static void RightWindow()
        {
            BugStatus();
            PlayerStatusDisplay();
        }
        #endregion

        #region Right TOP

        /// <summary>
        ///  버그의 Status 를 출력하는 메서드
        ///  LeftWindow 에서 플레이어가 솔루션을 입력하면
        ///  갱신되는 호출 메서드
        /// </summary>
        public static void BugStatus()
        {
            CurrentBug = DebuggingBugs[CurrentBugIndex];
            SetCursorPosition(50, 10);
            Write(new string(' ', 120 ));
            SetCursorPosition(50,2);
            Write($"[BugCount : {CurrentBugIndex+1} / {DebuggingBugs.Count}]");
            ClearLine(4);
            SetCursorPosition(50,4);
            Write($"[ Bug : {CurrentBug.BugName} ]");
            ClearLine(5);
            SetCursorPosition(50,5);
            Write($"[ Desc : {CurrentBug.BugDesc} ]");
            ClearLine(6);
            SetCursorPosition(50,6);
            Write($"[ Debug Difficulty : {CurrentBug.BugDifficulty} ]");
            ClearLine(7);
            SetCursorPosition(50,7);
            Write($"[ Debug Complexity : {CurrentBug.BugComplexity} ]");
            ClearLine(8);
            SetCursorPosition(50,8);
            Write($"[ Debug Process : {CurrentBug.BugProgress:D3} % ]  ");
            const int totalBlocks = 100;
            int greenBlocks = CurrentBug.BugProgress;

            for (int i = 0; i < totalBlocks; i++)
            {
                BackgroundColor = i < greenBlocks ? ConsoleColor.Green : ConsoleColor.DarkRed;
                Write(' '); // 공백으로 진행률 표시
            }
            ResetColor();
        }
        #endregion

        #region Right Middle

        /// <summary>
        ///  플레이어가 전달한 솔루션을 로깅 위치에 출력하는 로깅 메서드
        ///  메서드 출력 이후에 적 턴으로 로깅 메서드를 출력함
        /// </summary>
        public static void PlayerLoggingDisplay()
        {
            LoggingText loggingText = Battle.LoggingText!.First();
            List<string>? logs = Solution switch
            {
                ModifyVariables => loggingText.ModifyVariables,
                FixTypos => loggingText.FixTypos,
                ModifyScope => loggingText.ModifyScope,
                EditPath => loggingText.EditPath,
                CheckMemory => loggingText.CheckMemory,
                _ => new List<string>()
            };

            if (!logs!.Any()) return;

            int index = Battle.Random.Next(logs!.Count);
            string logMessage = logs[index];

            ClearLine(BattleCursorTop);
            SetCursorPosition(50, BattleCursorTop);
            WriteLine(logMessage);

            if (BattleCursorTop >= 26)
            {
                BattleCursorTop = 12;
                ClearLogging(BattleCursorTop, 28);
            }
            else
            {
                BattleCursorTop++;
            }
            BugLoggingDisplay();
        }

        /// <summary>
        ///  버그의 공격 로깅이 출력 되는 메서드
        /// </summary>
        private static void BugLoggingDisplay()
        { 
            ClearLine(BattleCursorTop);
            SetCursorPosition(50, BattleCursorTop);
            ForegroundColor = ConsoleColor.Red;
            int playerDef = LoginPlayer.C + LoginPlayer.BonusDef;
            int diff = CurrentBug.BugDifficulty - (playerDef / 5);
            int playerMental = LoginPlayer.Mental;
            int comp = CurrentBug.BugComplexity - (playerMental / 5);
            if (diff <= 0) {
                diff = 1;
            }
            if (comp <= 0) {
                comp = 1;
            }
            Write($"디버깅 어려움과 복잡도로 인해서  플레이어의 체력이 {diff}, 멘탈이 {comp} 감소 했습니다.");
            ResetColor();
            BattleCursorTop++;
            CurrentHp -= diff;
            CurrentMental -= comp;
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;
                ClearLoggingAndDefeat();
            }
            else
            {
                PlayerStatusDisplay();
            }
        }
        #endregion

        #region Right Bottom
        /// <summary>
        ///  플레이어의 현재 상태를 출력하는 Display 메서드
        /// 체력, 멘탈 등의 Stauts 가 갱신됨
        /// </summary>
        private static void PlayerStatusDisplay()
        {
            // 라인 그리기
            SetCursorPosition(46,29);
            for (int i = 0; i < 133; i++)
            {
                BackgroundColor = ConsoleColor.Yellow;
                Write(" ");
                ResetColor();
            }
          
            // 체력 바 그리기
            SetCursorPosition(50, 31);
            Write($"[ 체력 : {CurrentHp:D3} / {Battle.LoginPlayer!.HP:D3} ]  ");
            int staminaBlocks = CurrentHp;
            for (int i = 0; i < Battle.LoginPlayer.HP; i++)
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
            Write($"[ 멘탈 : {CurrentHp:D3} / {Battle.LoginPlayer.Mental:D3} ]  ");
            int mentalBlock = CurrentMental;
            for (int i = 0; i < Battle.LoginPlayer.Mental; i++)
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
            Write($"[ 디버깅 추가 성능 : {Battle.LoginPlayer.BonusDmg} ]  [ 언어 추가 이해력 : {Battle.LoginPlayer.BonusDef} ]");
          
            // 멘트
            SetCursorPosition(50, 36);
            Write("[ 멘탈이 일정 수준 감소할 때 마다 디버깅 확률 및 디버깅 능력이 감소합니다. ]");
        }
        #endregion

        #region Game Result
        /// <summary>
        ///  게임 패배시 로그를 비우고 Data.BattleSuccess를 false 로 지정하는 메서드
        /// </summary>
        private static void ClearLoggingAndDefeat()
        {
            while (true)
            {
                ClearLogging(12, 28);
                SetCursorPosition(50,20);
                Write("체력이 다해 디버깅이 실패했습니다."); 
                SetCursorPosition(50,22);
                Write("Enter Key를 눌러 보상화면으로 이동하세요");
                ConsoleKeyInfo key = ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        Data.BattleSuccess = false;
                        _isLoadScene = true;
                        Managers.Scene.LoadScene(SCENE_NAME.REWARD);
                        return;
                    default:
                        continue;      
                }
            }
        }

        /// <summary>
        ///  디버깅 실행결과 Data.BattleSuccess를 true 로 설정하는 메서드
        /// </summary>
        public static void DebuggingSuccess()
        {
            // 모든 버그를 디버깅 완료했을 경우
            while (true)
            {
                ClearLogging(12, 28);
                ClearLine(10);
                SetCursorPosition(50, 10);
                Write("디버그를 완료 하였습니다.");
                SetCursorPosition(50, 12);
                Write("Enter Key를 눌러 보상화면으로 이동하세요");
                ConsoleKeyInfo key = ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        Data.BattleSuccess = true;
                        _isLoadScene = true;
                        Managers.Scene.LoadScene(SCENE_NAME.REWARD);
                        return;
                    default:
                        continue;      
                }
            }
        }
        #endregion
    }
}