
using System.Runtime.InteropServices;

namespace HexaCoreVillage.Framework;

public class WindowsAPI
{
    #region Member Variables

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INT_RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_SCREEN_BUFFER_INFO
    {
        public COORD dwSize;
        public COORD dwCursorPosition;
        public short wAttributes;
        public RECT srWindow;
        public COORD dwMaximumWindowSize;
    }

    /* Literals */
    private const string CONSOLE_TITLE = "HexaCore Dungeon";

    private const string KERNEL = "kernel32.dll";
    private const string USER = "user32.dll";

    // CX, CY
    private const int SM_CXSCREEN = 0;
    private const int SM_CYSCREEN = 1;

    // Console Menu Box Flags
    private const int GWL_STYLE = -16;
    private const int WS_THICKFRAME = 0x00040000;
    private const int WS_MAXIMIZEBOX = 0x00010000;
    private const int WS_MINIMIZEBOX = 0x00020000;

    // SetWindowPos Flags
    private const uint SWP_NOSIZE = 0x0001;

    // private const int STD_INPUT_HANDLE = -10;
    private const int STD_OUTPUT_HANLDE = -11;

    /* Variables */
    //public static IntPtr hCursor = GetForegroundWindow();
    public static IntPtr hConsole = GetConsoleWindow();
    public static IntPtr hOuput = GetStdHandle(STD_OUTPUT_HANLDE);

    private static readonly IntPtr HWND_TOP = IntPtr.Zero;

    #endregion




    #region Extern Function

    /* Get Handlers */
    [DllImport(USER, SetLastError = true)] public static extern IntPtr GetForegroundWindow();
    [DllImport(KERNEL, SetLastError = true)] public static extern IntPtr GetConsoleWindow();
    [DllImport(KERNEL, SetLastError = true)] public static extern IntPtr GetStdHandle(int nStdHandle);


    // Setting Console Title Text
    [DllImport(USER, CharSet = CharSet.Auto, SetLastError = true)] public static extern bool SetWindowText(IntPtr hWnd, String lpText);

    // Get Console Menus
    [DllImport(USER, SetLastError = true)] private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    // Setting Console Menus
    [DllImport(USER, SetLastError = true)] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);




    // Setting Console Buffers
    [DllImport(KERNEL, SetLastError = true)] private static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, COORD dwSize);

    // Setting Window ConsoleSize by Buffers
    [DllImport(KERNEL, SetLastError = true)]
    private static extern bool SetConsoleWindowInfo(IntPtr hConsoleOutput, bool bAbsolute, ref RECT lpConsoleWindow);




    // Windows 콘솔의 사이즈 또는 위치 지정하는 함수
    [DllImport(USER, SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    // 현재 화면의 해상도를 구하는 함수
    [DllImport(USER)] private static extern int GetSystemMetrics(int nIndex);

    // 현재 윈도우 콘솔창의 크기를 구하는 함수
    [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out INT_RECT lpRect);
    
    // 현재 윈도우 콘솔 버퍼의 정보를 구하는 함수

    [DllImport("kernel32.dll")]
    public static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

    #endregion




    #region User Function

    public void ProgressConsoleSetting()
    {
        SetWindowsConsoleTitle();
        SetWindowsConsoleSize();
        SetWindowsConsolePosition();
    }

    /// <summary>
    /// # Windows Console Setting 1
    /// ## 1. 타이틀(프로젝트명) 설정
    /// ## 2. 콘솔 쓸모 없는 메뉴들(최소화, 최대화, 창크기조절 등) 제거
    /// </summary>
    private void SetWindowsConsoleTitle()
    {
        SetWindowText(hConsole, CONSOLE_TITLE);

        // Current Console
        var windowStyle = GetWindowLong(hConsole, GWL_STYLE);
        // Setting Console
        SetWindowLong(hConsole, GWL_STYLE, windowStyle & ~WS_THICKFRAME & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
    }

    /// <summary>
    /// # Windows Console Setting 2
    /// ## 콘솔의 내부 버퍼 크기 조절
    /// ## 버퍼크기에 맞춰 해상도 조절
    /// </summary>
    private void SetWindowsConsoleSize()
    {
        // 콘솔의 버퍼 설정
        COORD consoleBuffers;
        consoleBuffers.X = Renderer.FixedXColumnBuffer;
        consoleBuffers.Y = Renderer.FixedYRowsBuffer;

        // 콘솔 버퍼에 맞는 렉탱글(콘솔화면) 설정
        RECT consoleRect;
        consoleRect.Left = 0;
        consoleRect.Top = 0;

        // 화면이 초기 버퍼를 초과할 때 예외 처리
        if (IsBufferOverflow(consoleBuffers))
        {
            consoleRect.Right = 5;
            consoleRect.Bottom = 5;
            SetConsoleWindowInfo(hOuput, true, ref consoleRect);
        }
        else
        {
            consoleRect.Right = Renderer.FixedXColumnBuffer - 1;
            consoleRect.Bottom = Renderer.FixedYRowsBuffer - 1;
        }

        // 콘솔 버퍼 셋팅
        if (SetConsoleScreenBufferSize(hOuput, consoleBuffers))
            WriteLine($"화면이 {consoleBuffers.X} , {consoleBuffers.Y}로 고정되었습니다.");
        else
            throw new Exception("화면 버퍼 설정 실패.");


        // 콘솔 윈도우 화면 셋팅
        if (SetConsoleWindowInfo(hOuput, true, ref consoleRect))
            WriteLine($"콘솔 화면 크기가 {consoleBuffers.X - 1} , {consoleBuffers.Y - 1}로 맞춰졌습니다.");
        else
            throw new Exception("화면 버퍼에 따른 크기 조정 실패.");

        Thread.Sleep(Renderer.INIT_RESIZE_TIME_MS);
    }

    /// <summary>
    /// # Windows Console Setting 3
    /// ## 리사이즈된 콘솔의 포지션을 화면 해상도의 중심으로 설정
    /// </summary>
    private void SetWindowsConsolePosition()
    {
        GetWindowsWH(out COORD coord);

        int screenWidth = GetSystemMetrics(SM_CXSCREEN);
        int screenHeight = GetSystemMetrics(SM_CYSCREEN);

        int x = (screenWidth - coord.X) / 2;
        int y = (screenHeight - coord.Y) / 2;

        SetWindowPos(hConsole, HWND_TOP, x, y, 0, 0, SWP_NOSIZE);
    }

    /// <summary>
    /// # 윈도우 콘솔창 너비,높이를 반환하는 함수
    /// </summary>
    private void GetWindowsWH(out COORD widthHeight)
    {
        if (GetWindowRect(hConsole, out INT_RECT rect))
        {
            widthHeight.X = (short)(rect.Right - rect.Left);
            widthHeight.Y = (short)(rect.Bottom - rect.Top);
        }
        else
            throw new Exception("콘솔 화면의 크기를 가져올 수 없습니다.");
    }

    private bool IsBufferOverflow(COORD coord)
    {
        COORD bufferSize;
        GetConsoleScreenBufferInfo(hOuput, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo);

        bufferSize.X = (short)(bufferInfo.srWindow.Right - bufferInfo.srWindow.Left + 1);
        bufferSize.Y = (short)(bufferInfo.srWindow.Bottom - bufferInfo.srWindow.Top + 1);

        return (bufferSize.X > coord.X) || (bufferSize.Y > coord.Y);
    }
    #endregion
}