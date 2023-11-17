namespace HexaCoreVillage.Utility
{
    public static class ConsoleSizeUtility
    {
        private const int FixedRows = 30;
        private const int FixedColumns = 180;

        public static void RedrawBorder()
        {
            ForegroundColor = ConsoleColor.Green;

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