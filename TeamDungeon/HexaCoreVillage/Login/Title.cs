
using HexaCoreVillage.Framework;
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
        // 로직이 될 내용 작성
        this.WriteConsole();
    }

    private void WriteConsole()
    {
        //Console.Write("안녕");
    }
}