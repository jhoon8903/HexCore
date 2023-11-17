namespace HexaCoreVillage.Login;

public class Title : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.TITLE;

    public override void Start()
    {
        Managers.Scene.LoadScene(SCENE_NAME.LOGIN);
        // 초기화
    }

    public override void Update()
    {
        
    }
}