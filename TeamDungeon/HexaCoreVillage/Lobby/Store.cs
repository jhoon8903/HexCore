
namespace HexaCoreVillage.Lobby;

public class Store : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.STORE;

    public override void Start()
    {
        //throw new NotImplementedException();
    }

    public override void Update()
    {
        Console.Write("상점입니다.");
    }
}