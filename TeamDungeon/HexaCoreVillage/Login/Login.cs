using HexaCoreVillage.Utility;
using Newtonsoft.Json;

namespace HexaCoreVillage.Login;

static class Login
{
    public static Player? player = null;
    static string userID = "";
    static string CurrentDirectory = Directory.GetCurrentDirectory();
    public static void LoginScene()
    {
        
        int selectedOption = 0;
        string[] options = { "새로 시작하기", "이전 저장 불러오기", "게임 종료하기" };

        while (true)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 25);
            // 선택지 출력
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Blue; // 선택된 옵션은 파란색으로
                    Console.WriteLine("-> " + options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("   " + options[i]);
                }
            }

            // 키보드 입력 처리
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
                break; // 선택 완료 시 루프 종료
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
                Environment.Exit(0);    //종료
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
        Console.WriteLine("사용할 아이디를 입력해 주세요.");
        Console.SetCursorPosition(0, 25);
        userID = Console.ReadLine();

        Console.Clear();
        Console.WriteLine("사용할 이름을 입력해 주세요.");
        Console.SetCursorPosition(0, 25);
        userName = Console.ReadLine();
        //한번 더 물어보기?

        while (true)
        {
            Console.Clear();
            Console.WriteLine("당신의 직업은 무엇인가요?");

            // 선택지 출력
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Blue; // 선택된 옵션은 파란색으로
                    Console.WriteLine("-> " + options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("   " + options[i]);
                }
            }

            // 키보드 입력 처리
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
                break; // 선택 완료 시 루프 종료
            }
        }
        userJob = (Job)selectedOption;

        player = new Player(userID,userName,userJob);
    }

    private static void LoadGame()
    {
        string loadData = File.ReadAllText(CurrentDirectory + "\\savePlayer.json");         //불러오기 기능
        player = JsonConvert.DeserializeObject<Player>(loadData);
    }

    public static void SaveData()          //데이터 저장
    {
        string playerData = JsonConvert.SerializeObject(player, Formatting.Indented);        //플레이어 정보
        File.WriteAllText(CurrentDirectory + ".\\savePlayer.json", playerData);

        //string dungeonData =                  //던전 정보도 저장 예정
        
    }

}



enum LoginSelectOption
{
    NewGame,
    LoadGame,
    Exit
}

