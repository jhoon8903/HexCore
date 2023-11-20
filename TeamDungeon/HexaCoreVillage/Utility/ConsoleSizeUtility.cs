using System.Runtime.InteropServices;
namespace HexaCoreVillage.Utility
{
    public static class ConsoleSizeUtility
    {
        // For Unix systems
        [DllImport("libc")]
        private static extern int system(string exec);

        // For Windows systems
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const int FixedRows = 40;
        public const int FixedColumns = 180;

        private static void FixConsole()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FixConsoleWindows();
            }
            else
            {
                FixConsoleMac();
            }
        }

        private static void FixConsoleWindows()
        {
            // Adjust the window size for Windows
            IntPtr consoleWindow = GetConsoleWindow();
            // 3 is the SW_MAXIMIZE value to maximize the console window
            ShowWindow(consoleWindow, 3);
        }

        private static void FixConsoleMac()
        {
            // Adjust the window size for Unix-like systems
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