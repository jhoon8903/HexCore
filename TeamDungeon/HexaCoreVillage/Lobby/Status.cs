
namespace HexaCoreVillage.Lobby;

public class Status : Scene
{
    public override SCENE_NAME SceneName => throw new NotImplementedException();

    public override void Start()
    {
        Console.Clear();
        Console.WriteLine("안녕 나는 캐릭터씬의 스타트 메서드야 한번만 실행돼");
        Console.ReadKey();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
}