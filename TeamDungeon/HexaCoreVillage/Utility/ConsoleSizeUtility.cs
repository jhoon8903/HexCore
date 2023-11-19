using System.Runtime.InteropServices;
namespace HexaCoreVillage.Utility
{
    public static class ConsoleSizeUtility
    {
        [DllImport ("libc")] private static extern int system (string exec);
        public const int FixedRows = 40;
        public const int FixedColumns = 180;


        public static void FixConsole()
        {
            system($@"printf '\e[8;{FixedRows};{FixedColumns}t'");
        }
        public static void RedrawBorder()
        {
            ForegroundColor = ConsoleColor.Green;
            FixConsole();
            // 콘솔의 상단을 그리는 로직
            SetCursorPosition(0, 0);
            Write(new string('=', FixedColumns));

            // 콘솔의 양 옆의 창을 그리는 로직
            for (int i = 1; i < FixedRows - 1; i++)
            {
                SetCursorPosition(0, i);
                Write('|');
                SetCursorPosition(FixedColumns - 1, i);
                Write('|');
            }

            // 콘솔의 바닥을 그리는 로직
            SetCursorPosition(0, FixedRows - 1);
            Write(new string('=', FixedColumns));

            ResetColor();
        }
    }
}