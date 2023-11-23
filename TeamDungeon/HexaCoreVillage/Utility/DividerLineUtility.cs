
namespace HexaCoreVillage.Utility;

public static class DividerLineUtility
{
    /// <summary>
    ///  구열을 나누는 줄을 생성합니다.
    /// </summary>
    /// <param name="left">줄 시작점 int Type</param>
    /// <param name="top">줄 상단 시작점 int Type</param>
    /// <param name="color">줄 색상 ConsoleColor Type</param>
    public static void DividerLine(int left, int top, ConsoleColor color)
    {
        const int splitPosition = Renderer.FixedXColumn / 4; // 분할 위치 결정

        SetCursorPosition(left,top);

        for (int i = 0; i < splitPosition - 1; i++)
        {
            BackgroundColor = color;
            Write(" ");
            ResetColor();
        }
    }
}