
namespace HexaCoreVillage.Utility
{
    // 리소스 명(ex. startBGM)과 일치시켜서 등록해주세요.
    public enum ResourceKeys
    {
        // TEXT
        TextLobbyAscii,
        // JSON
        BugList,
        DebugText,
        ItemList,
        SavePlayer,
        // BGMS
        newWorldBGM,
        startBGM,
        BattleBGM
    }

    public struct COORD
    {
        public int X;
        public int Y;

        public COORD(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    /// <summary>
    /// # 리터럴 상수들을 모아놓은 클래스
    /// </summary>
    public static class Literals
    {
        // ex. public const float PI = 3.141592f;
        public const string PlayerDataPath = "SavePlayer.json";
    }
}