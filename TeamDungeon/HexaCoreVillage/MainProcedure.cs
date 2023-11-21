
namespace HexaCoreVillage;

public static class MainProcedure
{
    /* Main */
    public static void Main()
    {
        // Console Size [OS condition] Resetting
        Renderer.Instance.InitalizeRenderer();

        // Title Scene Load.
        Managers.Scene.LoadScene(SCENE_NAME.BATTLE);

        Run();
    }

    #region Running To Game (LOOP)
    private static void Run()
    {
        /* 조건식이 게임이 종료되지 않았을 때로 바꿔야한다. */
        while (Managers.Scene.GetCurrentScene() != null)
            Managers.Scene.GetCurrentScene()?.Update();
    }
    #endregion
}