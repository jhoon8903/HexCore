
using Newtonsoft.Json;

namespace HexaCoreVillage.Utility;

public static class Data
{
    #region BATTLE DATA
    public static bool BattleSuccess { get; set; }
    public static int BugCount { get; set; }
    #endregion

    public static void SavePlayerData(Player player)
    {
        // 경로 지정 (리소스 폴더와 json파일의 이름을 합침)
        var playerDataPath = Path.Combine(Managers.Resource.GetResourceFolderPath(), Literals.PlayerDataPath);
        string playerData = JsonConvert.SerializeObject(player, Formatting.Indented);        //캐릭터 정보 저장
        File.WriteAllText(playerDataPath, playerData);
    }
}
