
using HexaCoreVillage.Utility;

namespace HexaCoreVillage.Manager;

/// <summary>
/// # 매니저들을 관리하는 관리자
/// ## Manager_Scene
/// # (필요시) 게임의 흐름/분기 제어
/// </summary>
public class GameManager : Singleton<GameManager>
{
    #region Member Variables
    private Manager_Scene _sceneManager = new();
    private Manager_Resource _resourceManager = new();
    private Manager_UserInterface _userInterfaceManager = new();
    #endregion

    #region Properties
    public Manager_Scene Scene => _sceneManager;
    public Manager_Resource Resource => _resourceManager;
    public Manager_UserInterface UserInterface => _userInterfaceManager;
    #endregion
}

/// <summary>
/// # 게임매니저가 관리하는 매니저들을 보다 쉽게 부르기 위한 별칭용 정적 클래스
/// ## 사용예제 => using Managers = HexaCoreVillage.Manager.ManagersAliasClass;
/// ### 위 내용은 프로젝트 자체에 전역으로 네임스페이스를 설정함.
/// </summary>
public static class ManagersAliasClass
{
    public static GameManager GM => GameManager.Instance;
    public static Manager_Scene Scene => GameManager.Instance.Scene;
    public static Manager_Resource Resource => GameManager.Instance.Resource;
    public static Manager_UserInterface UI => GameManager.Instance.UserInterface;
}