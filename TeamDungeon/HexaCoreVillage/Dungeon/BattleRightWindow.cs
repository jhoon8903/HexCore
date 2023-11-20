using HexaCoreVillage.Utility;
using static HexaCoreVillage.Dungeon.Battle;
using static HexaCoreVillage.Dungeon.Debugging.SolutionTypes;

namespace HexaCoreVillage.Dungeon
{
    public static class BattleRightWindow
    {
        public static void RightWindow()
        {
            BugStatus();
            RightBottomDisplay();
        }

        private static void BugStatus()
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

        public static void InputSolution()
        {
            // 데미지 적용 부분 수정 필요
            SetCursorPosition(50, 10);
            int progressIncrease = CurrentBug.SolutionType == Solution ? Battle.Random.Next(1, 20) : Battle.Random.Next(1, 10);

            CurrentBug.BugProgress += progressIncrease;

            if (CurrentBug.BugProgress >= 100)
            {
                CurrentBug.BugProgress = 100; 
                BugStatus();
                CurrentBugIndex++; // 다음 버그로 이동
                if (CurrentBugIndex >= DebuggingBugs.Count && CurrentBug.BugProgress >= 100)
                {
                    DebuggingSuccess(); 
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
                BugStatus(); // 버그 진행률 업데이트
                PlayerLoggingDisplay();
            }
        }

        private static void PlayerLoggingDisplay()
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

        private static void BugLoggingDisplay()
        { 
            ClearLine(BattleCursorTop);
            SetCursorPosition(50, BattleCursorTop);
            ForegroundColor = ConsoleColor.Red;
            int diff = Battle.Random.Next(1,CurrentBug.BugDifficulty+1);
            int comp = Battle.Random.Next(1,CurrentBug.BugComplexity+1);
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
                RightBottomDisplay();
            }
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
          Write($"[ 체력 : {CurrentHp:D3} / {Battle.Player!.HP:D3} ]  ");
          int staminaBlocks = CurrentHp;
          for (int i = 0; i < Battle.Player.HP; i++)
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
          Write($"[ 멘탈 : {CurrentHp:D3} / {Battle.Player.Mental:D3} ]  ");
          int mentalBlock = CurrentMental;
          for (int i = 0; i < Battle.Player.Mental; i++)
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
          Write($"[ 디버깅 추가 성능 : {Battle.Player.BonusDmg} ]  [ 언어 추가 이해력 : {Battle.Player.BonusDef} ]");
          
          // 멘트
          SetCursorPosition(50, 36);
          Write("[ 멘탈이 일정 수준 감소할 때 마다 디버깅 확률 및 디버깅 능력이 감소합니다. ]");
        }

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
                        Managers.Scene.LoadScene(SCENE_NAME.REWARD);
                        return;
                    default:
                        continue;      
                }
            }
        }

        private static void DebuggingSuccess()
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
                        Managers.Scene.LoadScene(SCENE_NAME.REWARD);
                        return;
                    default:
                        continue;      
                }
            }
        }
    }
}