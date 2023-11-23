
using HexaCoreVillage.Utility;

namespace HexaCoreVillage.Manager;

public class Manager_UserInterface
{
    #region Member Variables
    private const int posX = 2;
    private const int posY = 1;

    public readonly int StartPosX = posX;
    public readonly int StartPosY = posY;
    public readonly int EndPosX = Renderer.FixedXColumn - posX;
    public readonly int EndPosY = Renderer.FixedYRows - posY;
    public readonly int areaX = Renderer.FixedXColumn - (posX * 2);
    public readonly int areaY = Renderer.FixedYRows - (posY * 2);

    private readonly object _lock = new object();

    #endregion



    #region Draw Methods
    public int DrawAsciiMessage(ResourceKeys key, int startPosY = 2)
    {
        string asciiArtResource = Managers.Resource.GetTextResource(key);
        string[] asciiArtSplitMessage = asciiArtResource.Split(new[] { "\n" }, StringSplitOptions.None);

        int idx;
        for(idx = 0; idx < asciiArtSplitMessage.Length; ++idx)
        {
            ConsoleColor color = ConsoleColor.Gray;
            if (idx < 3) color = ConsoleColor.Yellow;
            else if (idx >= 3 && idx < 6) color = ConsoleColor.Cyan;
            else color = ConsoleColor.Blue;

            PrintMsgAlignCenter(asciiArtSplitMessage[idx], idx + startPosY, color);
        }

        return idx;
    }

    public void DrawBox(int startPosX, int startPosY, int width, int height, ConsoleColor color = ConsoleColor.Green)
    {
        string topView = "┌" + new string('─', width - 2) + "┐";
        string bottomView = "└" + new string('─', width - 2) + "┘";

        PrintMsgCoordbyColor(topView, startPosX, startPosY, color);
        for(int col = startPosY + 1; col < startPosY + height; ++col)
        {
            string symbol = "│";
            PrintMsgCoordbyColor(symbol, startPosX, col, color);
            PrintMsgCoordbyColor(symbol, startPosX + width - 1, col, color);
        }
        PrintMsgCoordbyColor(bottomView, startPosX, startPosY + height, color);
    }

    public COORD[] DrawColumnBox(int startPosX, int startPosY, int width, int height, int columnsNumber = 2, ConsoleColor color = ConsoleColor.Green)
    {
        int columnWidth = (width - 1) / columnsNumber;
        COORD[] columnPositions = new COORD[columnsNumber];

        // 상단 테두리
        string topView = "┌" + string.Join("┬", Enumerable.Repeat(new string('─', columnWidth), columnsNumber)) + "┐";
        PrintMsgCoordbyColor(topView, startPosX, startPosY, color);

        // 측면 테두리 및 컬럼 분리선
        for (int col = startPosY + 1; col < startPosY + height; ++col)
        {
            string sideView = "│" + string.Join("│", Enumerable.Repeat(new string(' ', columnWidth), columnsNumber)) + "│";
            PrintMsgCoordbyColor(sideView, startPosX, col, color);
        }

        // 하단 테두리
        string bottomView = "└" + string.Join("┴", Enumerable.Repeat(new string('─', columnWidth), columnsNumber)) + "┘";
        PrintMsgCoordbyColor(bottomView, startPosX, startPosY + height, color);

        // 컬럼 시작 좌표 계산
        for (int i = 0; i < columnsNumber; ++i)
            columnPositions[i] = new COORD(
                startPosX + 2 + i * (columnWidth + 1),
                startPosY + 1);

        return columnPositions;
    }
    #endregion



    #region Print Text Methods
    /// <summary>
    /// 중앙 정렬하여 출력하는 메서드
    /// </summary>
    public void PrintMsgAlignCenter(string message, int posY, ConsoleColor fontColor = ConsoleColor.Gray)
    {
        PrintMsgAlignCenter(message, posY, fontColor, BackgroundColor);
    }
    public void PrintMsgAlignCenter(string message, int posY, 
        ConsoleColor fontColor, ConsoleColor bgColor)
    {
        lock (_lock)
        {
            int padding = (Renderer.FixedXColumn - GetMessageUTFLength(message)) / 2;

            SetPos(padding, posY);
            PrintMsgToColor(message, fontColor, bgColor);
        }
    }

    /// <summary>
    /// X : 중앙, Y도 중앙
    /// 즉 화면 정중앙에 출력하는 메서드
    /// </summary>
    public void PrintMsgAlignCenterByCenter(string message, ConsoleColor fontColor = ConsoleColor.Gray)
    {
        PrintMsgAlignCenterByCenter(message, fontColor, BackgroundColor);
    }
    public void PrintMsgAlignCenterByCenter(string message,
        ConsoleColor fontColor, ConsoleColor bgColor)
    {
        lock (_lock)
        {
            int wPadding = (Renderer.FixedXColumn - GetMessageUTFLength(message)) / 2;
            int hPadding = (Renderer.FixedYRows / 2);

            SetPos(wPadding, hPadding);
            PrintMsgToColor(message, fontColor, bgColor);
        }
    }

    /// <summary>
    /// 원하는 위치에 출력하는 메서드
    /// </summary>
    public void PrintMsgCoordbyColor(string message, int posX, int posY, ConsoleColor fontColor = ConsoleColor.Gray)
    {
        PrintMsgCoordbyColor(message, posX, posY, fontColor, BackgroundColor);
    }
    public void PrintMsgCoordbyColor(string message, int posX, int posY,
        ConsoleColor fontColor, ConsoleColor bgColor)
    {
        lock (_lock)
        {
            SetPos(posX, posY);
            PrintMsgToColor(message, fontColor, bgColor);
        }
    }
    #endregion



    #region Clear Methods
    public void ClearRow(int row)
    {
        lock (_lock)
        {
            SetPos(StartPosX, row);
            PrintMsg(new string(' ', areaX));
        }
    }

    public void ClearRows(int from, int to)
    {
        lock (_lock)
        {
            for (int row = from; row <= to; ++row)
            {
                SetPos(StartPosX, row);
                PrintMsg(new string(' ', areaX));
            }
        }
    }

    public void ClearRowColSelect(int row, int colStart, int colEnd)
    {
        lock (_lock)
        {
            SetPos(colStart, row);
            PrintMsg(new string(' ', colEnd - colStart));
        }
    }

    public void ClearRowsColSelect(int from, int to, int colStart, int colEnd)
    {
        lock(_lock)
        {
            for(int row = from; row <= to; ++row)
            {
                SetPos(colStart, row);
                PrintMsg(new string(' ', colEnd - colStart));
            }
        }
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
    public void PrintMsgToColor(string message, ConsoleColor fontColor, ConsoleColor bgColor)
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
    public void SetPos(int posX, int posY)
    {
        lock(_lock)
            SetCursorPosition(posX, posY);
    }
    public static void PrintMsg(string message)
    {
        Write(message);
    }
    #endregion
}
