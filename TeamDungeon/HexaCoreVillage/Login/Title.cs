
using HexaCoreVillage.Framework;
namespace HexaCoreVillage.Login;

public class Title : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.TITLE;

    public override void Start()
    {
        // �ʱ�ȭ
    }

    public override void Update()
    {
        // ������ �� ���� �ۼ�
        this.WriteConsole();
    }

    private void WriteConsole()
    {
        Console.Write("�ȳ�");
    }
}