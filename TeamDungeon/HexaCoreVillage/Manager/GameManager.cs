
using HexaCoreVillage.Framework;
namespace HexaCoreVillage.Manager;

/// <summary>
/// # 매니저들을 관리하는 관리자
/// ## Manager_Scene
/// # (필요시) 게임의 흐름/분기 제어
/// </summary>
public class GameManager : Singleton<GameManager>
{
    #region Member Variables
    private Manager_Scene SceneManager = new();
    #endregion

    #region Properties
    public static Manager_Scene Scene => Instance.SceneManager;
    #endregion
}