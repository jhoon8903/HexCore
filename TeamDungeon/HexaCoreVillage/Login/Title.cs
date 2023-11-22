using HexaCoreVillage.Utility;

namespace HexaCoreVillage.Login;

public class Title : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.TITLE;

    public override void Start()
    {
        Managers.Scene.LoadScene(SCENE_NAME.LOGIN);
        // 초기화
        // Renderer.Instance.DrawConsoleBorder();

    }

    public override void Stop()
    {
        
    }

    public override void Update()
    {
        
    }
}