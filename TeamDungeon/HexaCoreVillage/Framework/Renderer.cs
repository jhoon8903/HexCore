
using System.Runtime.InteropServices;

namespace HexaCoreVillage.Framework;

public class Renderer : Singleton<Renderer>
{
    #region Member Variables

    /* Literals */
    // 버퍼 및 해상도 조절을 위한 리터럴
    public const int FixedXColumnBuffer = 180;
    public const int FixedYRowsBuffer = 40;

    // 실제 문자를 쓸 때 계산되는 X, Y
    public const int FixedXColumn = FixedXColumnBuffer - 1;
    public const int FixedYRows = FixedYRowsBuffer - 1;

    // 창 크기 제어 대기시간 (ms)
    public const int INIT_RESIZE_TIME_MS = 1000;

    /* Variables */
    private WindowsAPI? _winAPI = null;
    private MacAPI? _macAPI = null;

    #endregion



    #region Init Console(Terminal)

    /// <summary>
    /// 
    /// 현재 OS에 맞게 해상도를 조절해주는 메서드
    /// 
    /// # 윈도우
    /// ## 창 크기 설정
    /// ### 타이틀, 메뉴바, 스크롤, 버퍼, 포지셔닝
    /// 
    /// # 맥
    /// ## 창 크기 설정
    /// ### 내용 추가되면 추가바람
    /// </summary>
    public void InitalizeRenderer()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            _winAPI = new WindowsAPI();
            _winAPI.ProgressConsoleSetting();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            _macAPI = new MacAPI();
            _macAPI.ProgressConsoleSetting();
        }

        WriteLine("창을 재설정 중입니다. 조금만 기다려주세요");
        Thread.Sleep(INIT_RESIZE_TIME_MS);
        WriteLine("설정이 완료됐습니다. 아무키나 누르면 시작됩니다.");
        ReadKey(true); Clear();
    }

    #endregion



    #region Main Methods
    public void DrawConsoleBorder()
    {
        CursorVisible = false;
        ForegroundColor = ConsoleColor.Green;
        // FixConsole();

        // 콘솔 상단을 그리는 로직
        SetCursorPosition(0, 0);
        Write('┌'); Write(new string('─', FixedXColumn - 2)); Write('┐');

        for(int yAxis = 1; yAxis < FixedYRows; ++yAxis)
        {
            SetCursorPosition(0, yAxis); Write('│');
            SetCursorPosition(FixedXColumn - 1, yAxis); Write('│');
        }

        // 콘솔 하단을 그리는 로직
        SetCursorPosition(0, FixedYRows);
        Write('└'); Write(new string('─', FixedXColumn - 2)); Write('┘');
        ResetColor();
    }
    #endregion
}