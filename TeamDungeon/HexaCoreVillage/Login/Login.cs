using HexaCoreVillage.Utility;
using Newtonsoft.Json;

namespace HexaCoreVillage.Login;

public class Login : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.LOGIN;

    public static Player? player = null;
    static string userID = "";
    static string CurrentDirectory = Directory.GetCurrentDirectory();


    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public static void LoginScene()
    {
        
        int selectedOption = 0;
        string[] options = { "새 게임 시작", "이전 게임 불러오기", "게임 종료하기" };

        while (true)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 25);
            
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
                Environment.Exit(0);    
                break;
        }
    }

    private static void NewGame()
    {
        int selectedOption = 0;
        string[] jobOptions = { "Unity", "Unreal", "AI", "PM", "QA"};
        string[] backgroundOptions = { "Unity", "Unreal", "AI", "PM", "QA" };
        Job userJob;
        string userName;

        Console.Clear();
        Console.WriteLine("사용할 아이디를 입력해 주세요.");
        Console.SetCursorPosition(0, 25);
        userID = Console.ReadLine();

        Console.Clear();
        Console.WriteLine("불리고 싶은 이름을 정해주세요.");
        Console.SetCursorPosition(0, 25);
        userName = Console.ReadLine();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("직업을 선택해주세요.");

            
            for (int i = 0; i < jobOptions.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Blue; // 선택중인 옵션
                    Console.WriteLine("-> " + jobOptions[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("   " + jobOptions[i]);
                }
            }

            
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedOption = (selectedOption == 0) ? jobOptions.Length - 1 : selectedOption - 1;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedOption = (selectedOption == jobOptions.Length - 1) ? 0 : selectedOption + 1;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break; // 엔터 누르면 루프 탈출
            }
        }

        userJob = (Job)selectedOption;
        player = new Player(userID,userName,userJob);

        while (true)                //배경 설정
        {
            Console.Clear();
            Console.WriteLine("어떻게 살아왔는지 알려주세요.");

            for (int i = 0; i < backgroundOptions.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Blue; // 선택중인 옵션
                    Console.WriteLine("-> " + backgroundOptions[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("   " + backgroundOptions[i]);
                }
            }


            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedOption = (selectedOption == 0) ? backgroundOptions.Length - 1 : selectedOption - 1;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedOption = (selectedOption == backgroundOptions.Length - 1) ? 0 : selectedOption + 1;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break; // 엔터 누르면 루프 탈출
            }
        }

    }

    private static void LoadGame()
    {
        string loadData = File.ReadAllText(CurrentDirectory + "\\savePlayer.json");         //캐릭터 정보 불러오기
        player = JsonConvert.DeserializeObject<Player>(loadData);
    }

    public static void SaveData()          
    {
        string playerData = JsonConvert.SerializeObject(player, Formatting.Indented);        //캐릭터 정보 저장
        File.WriteAllText(CurrentDirectory + ".\\savePlayer.json", playerData);

        //string dungeonData =                  //던전 정보도 저장할 예정.
        
    }
}



enum LoginSelectOption
{
    NewGame,
    LoadGame,
    Exit
}
