
namespace HexaCoreVillage.Login;

public class Title : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.TITLE;

    public override void Start()
    { 
        Login.LoginScene();
        // 초기화
    }

    public override void Update()
    {
        
    }
}