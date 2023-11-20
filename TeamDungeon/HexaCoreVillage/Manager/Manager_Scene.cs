
namespace HexaCoreVillage.Manager;

/// <summary>
/// # Scene을 관리하는 매니저 클래스
/// ## Scene by Scene : 씬을 이동 시키는 매개 클래스
/// </summary>
public class Manager_Scene
{
    #region Member Variables

    private Scene? _currentScene = null;

    #endregion



    #region Properties (Getter)
    public Scene? GetCurrentScene() { return _currentScene; }
    #endregion

    

    #region Main Methods
    public void LoadScene(SCENE_NAME sceneKey)
    {
        // 추후 리소스 관리쪽으로 넘어갔을 때를 위한 처리 (아직은 기능 없음)
        if (_currentScene != null)
            _currentScene = null;

        // switch Lambda, _currentScene에 SCENE_NAME에 해당하는 SCENE으로 동적 할당
        _currentScene = sceneKey switch
        {
            SCENE_NAME.TITLE => new TitleScene(),
            SCENE_NAME.LOGIN => new LoginScene(),
            SCENE_NAME.LOBBY => new LobbyScene(),
            SCENE_NAME.STATUS => new StatusScene(),
            SCENE_NAME.STORE => new StoreScene(),
            SCENE_NAME.BATTLE => new BattleScene(),
            SCENE_NAME.REWARD => new RewardScene(),
            _ => throw new ArgumentException("Invalid [SceneKey]")
        };

        // Change the 'scene' and initalize(Start)
        _currentScene.Start();
    }
    #endregion
}