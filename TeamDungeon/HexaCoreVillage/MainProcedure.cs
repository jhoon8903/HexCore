
using HexaCoreVillage.Framework;
using HexaCoreVillage.Login;

namespace HexaCoreVillage;

public static class MainProcedure
{
    static void Main()
    {
        Title titleScene = new Title();
        titleScene.Start();

        Scene currentScene = titleScene;


        // 임시용 무한반복
        while(true)
        {
            currentScene.Update();
        }
    }
}
