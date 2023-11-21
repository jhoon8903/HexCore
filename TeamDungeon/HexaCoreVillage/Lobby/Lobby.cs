
using HexaCoreVillage.Utility;

namespace HexaCoreVillage.Lobby;

public class Lobby : Scene
{
    #region Member Variables
    public override SCENE_NAME SceneName => SCENE_NAME.LOBBY;

    private int _menuSelector = 0;
    #endregion



    #region Flow Methods
    public override void Start()
    {
        // Cursor Setting
        CursorVisible = false;

        Renderer.Instance.DrawConsoleBorder();
    }

    public override void Update()
    {
        Managers.UI.ClearRows(1, 10);
        Managers.UI.DrawAsciiMessage(ResourceKeys.TextLobbyAscii);
        Thread.Sleep(100);
    }
    #endregion



    #region Logic Methods
    
    #endregion
}