
using System.Drawing;

namespace HexaCoreVillage.Manager;

public class Manager_UserInterface
{
    #region Draw Methods
    public void DrawProgress(string message, int width, int length)
    {

    }

    public void DrawAsciiMessage()
    {
        string textOrigin; 
    }
    #endregion



    #region Print Text Methods
    /// <summary>
    /// 중앙 정렬하여 출력하는 메서드
    /// </summary>
    public void PrintMsgAlignCenter(string message, int posY, 
        ConsoleColor fontColor = ConsoleColor.Gray, ConsoleColor bgColor = ConsoleColor.Black)
    {
        int padding = (Renderer.FixedXColumn - GetMessageUTFLength(message)) / 2;

        SetPos(padding, posY);
        PrintMsgToColor(message, fontColor, bgColor);
    }

    /// <summary>
    /// X : 중앙, Y도 중앙
    /// 즉 화면 정중앙에 출력하는 메서드
    /// </summary>
    public void PrintMsgAlignCenterByCenter(string message,
        ConsoleColor fontColor = ConsoleColor.Gray, ConsoleColor bgColor = ConsoleColor.Black)
    {
        int wPadding = (Renderer.FixedXColumn - GetMessageUTFLength(message)) / 2;
        int hPadding = (Renderer.FixedYRows / 2);

        SetPos(wPadding, hPadding);
        PrintMsgToColor(message, fontColor, bgColor);
    }

    /// <summary>
    /// 원하는 위치에 출력하는 메서드
    /// </summary>
    public void PrintMsgCoordbyColor(string message, int posX, int posY,
        ConsoleColor fontColor = ConsoleColor.Gray, ConsoleColor bgColor = ConsoleColor.Black)
    {
        SetPos(posX, posY);
        PrintMsgToColor(message, fontColor, bgColor);
    }
    #endregion



    #region Helper Methods
    /// <summary>
    /// # 매개변수로 받은 문자열의 길이를 한글(멀티바이트)를 포함해 계산해서 반환
    /// ## "안녕하세요 저는 누구" : 이런것이면 길이를 정확히 반환해줌 (10 + 1 + 4 + 1 + 4)
    /// </summary>
    public int GetMessageUTFLength(string message)
    {
        int msgLength = 0;

        foreach(char ch in message)
            msgLength += (ch >= '\uAC00' && ch <= '\uD7A3') ? 2 : 1;

        return msgLength;
    }


    /// <summary>
    /// 문자열에 색상을 더해주는 함수
    /// </summary>
    public void PrintMsgToColor(string message, ConsoleColor fontColor, ConsoleColor bgColor = ConsoleColor.Black)
    {
        ForegroundColor = fontColor;
        BackgroundColor = bgColor;
        WriteLine(message);
        ResetColor();
    }
    #endregion



    /* 사용이 필요하지 않다면 굳이 이걸 사용할 필요 없습니다. */
    #region Rename Basic Methods
    // Foreground Color
    public static void SetColor(ConsoleColor color)
    {
        ForegroundColor = color;
    }
    // Background Color
    public static void SetBGColor(ConsoleColor color)
    {
        BackgroundColor = color;
    }
    // Set Cursor Position
    public static void SetPos(int posX, int posY)
    {
        SetCursorPosition(posX, posY);
    }
    public static void PrintMsg(string message)
    {
        Write(message);
    }
    #endregion
}
