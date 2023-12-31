
using HexaCoreVillage.Utility;

namespace HexaCoreVillage;

public static class MainProcedure
{
    /* Main */
    public static void Main()
    {
        AudioPlayer.SetupApplicationExitHandling();
        // Console Size [OS condition] Resetting
        Renderer.Instance.InitalizeRenderer();
        // Load All Reosurces
        Managers.Resource.LoadAllResources();
        // Title Scene Load.
        Managers.Scene.LoadScene(SCENE_NAME.LOGIN);
        Run();
    }

    #region Running To Game (LOOP)
    private static void Run()
    {
        /* 조건식이 게임이 종료되지 않았을 때로 바꿔야한다. */
        while (Managers.Scene.GetCurrentScene() != null)
        {
            Managers.Scene.GetCurrentScene()?.Update();
        }
    }
    #endregion
}