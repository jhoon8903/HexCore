using HexaCoreVillage.Utility;
using HexaCoreVillage.Framework;
using Newtonsoft.Json;

namespace HexaCoreVillage.Login;

static class Login : Scene
{
    public override SCENE_NAME SceneName => throw new NotImplementedException();

    public static Player? player = null;
    static string userID = "";
    static string CurrentDirectory = Directory.GetCurrentDirectory();
    public static void LoginScene()
    {
        
        int selectedOption = 0;
        string[] options = { "���� �����ϱ�", "���� ���� �ҷ�����", "���� �����ϱ�" };

        while (true)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 25);
            // ������ ���
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Blue; // ���õ� �ɼ��� �Ķ�������
                    Console.WriteLine("-> " + options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("   " + options[i]);
                }
            }

            // Ű���� �Է� ó��
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break; // ���� �Ϸ� �� ���� ����
            }
        }

        switch ((LoginSelectOption)selectedOption)
        {
            case LoginSelectOption.NewGame:
                NewGame();
                break;

            case LoginSelectOption.LoadGame:
                LoadGame();
                break;

            case LoginSelectOption.Exit:
                Environment.Exit(0);    //����
                break;
        }
    }

    private static void NewGame()
    {
        int selectedOption = 0;
        string[] options = { "Unity", "Unreal", "AI", "PM", "QA"};
        Job userJob;
        string userName;

        Console.Clear();
        Console.WriteLine("����� ���̵� �Է��� �ּ���.");
        Console.SetCursorPosition(0, 25);
        userID = Console.ReadLine();

        Console.Clear();
        Console.WriteLine("����� �̸��� �Է��� �ּ���.");
        Console.SetCursorPosition(0, 25);
        userName = Console.ReadLine();
        //�ѹ� �� �����?

        while (true)
        {
            Console.Clear();
            Console.WriteLine("����� ������ �����ΰ���?");

            // ������ ���
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Blue; // ���õ� �ɼ��� �Ķ�������
                    Console.WriteLine("-> " + options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("   " + options[i]);
                }
            }

            // Ű���� �Է� ó��
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break; // ���� �Ϸ� �� ���� ����
            }
        }
        userJob = (Job)selectedOption;

        player = new Player(userID,userName,userJob);
    }

    private static void LoadGame()
    {
        string loadData = File.ReadAllText(CurrentDirectory + "\\savePlayer.json");         //�ҷ����� ���
        player = JsonConvert.DeserializeObject<Player>(loadData);
    }

    public static void SaveData()          //������ ����
    {
        string playerData = JsonConvert.SerializeObject(player, Formatting.Indented);        //�÷��̾� ����
        File.WriteAllText(CurrentDirectory + ".\\savePlayer.json", playerData);

        //string dungeonData =                  //���� ������ ���� ����
        
    }
    
    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

}



enum LoginSelectOption
{
    NewGame,
    LoadGame,
    Exit
}
