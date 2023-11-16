
using HexaCoreVillage.Framework;
using HexaCoreVillage.Login;
using Login = HexaCoreVillage.Login.Login;

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

    #region Main Methods
    public void LoadScene(SCENE_NAME sceneKey)
    {
        _currentScene = sceneKey switch
        {
            SCENE_NAME.TITLE => new Title(),
            //SCENE_NAME.LOGIN => new Login(),    // 뭔지 잘 모르겠음

        };
    }
    #endregion
}
