using HexaCoreVillage.Utility;
using Newtonsoft.Json;
using System.Text;
using static HexaCoreVillage.Utility.AudioPlayer;
using Player = HexaCoreVillage.Utility.Player;

namespace HexaCoreVillage.Login;

public class Login : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.LOGIN;

    static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    static string relativePathToAudio = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", "..", "..", "TeamDungeon", "HexaCoreVillage"));
    private static string UtilityPath = Path.Combine(relativePathToAudio, "Utility");

    public static List<Item> ItemBox = new List<Item>();
    private static Player _player = null;

    /* Audio Resources */
    private string _newWorldBGM = Managers.Resource.GetSoundResource(ResourceKeys.newWorldBGM);
    private string _startBGM = Managers.Resource.GetSoundResource(ResourceKeys.startBGM);


    public override void Start()
    {
        SetItemBox();
        CursorVisible = false;
        AudioController(Managers.Resource.GetSoundResource(ResourceKeys.startBGM),PlayOption.Play);
    }

    public override void Update()
    {
        LoginScene();
    }

    private void LoginScene()
    {
        int selectedOption = 0;
        string[] options = { "New Game", "Load Game", "Exit" };
        string[] plusOpt = { "새로운 아이디로 게임을 처음부터 시작합니다.(프롤로그 포함)", "전에 사용하던 아이디로 게임을 시작합니다.(프롤로그 스킵)", "게임 종료하기" };
        while (true)
        {
            Clear();
            Renderer.Instance.DrawConsoleBorder();
            SetCursorPosition(0, 35);

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedOption)
                {
                    SetCursorPosition(CursorLeft + 1, CursorTop);
                    ForegroundColor = ConsoleColor.Blue;
                    WriteLine("-> " + options[i]);
                    SetCursorPosition(CursorLeft + 1, CursorTop);
                    WriteLine("   - " + plusOpt[i]);
                    ResetColor();
                }
                else
                {
                    SetCursorPosition(CursorLeft + 1, CursorTop);
                    WriteLine("   " + options[i]);
                }
            }


            ConsoleKeyInfo keyInfo = ReadKey();
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
                break;
            }
        }

        switch ((JobSelectOption)selectedOption)
        {
            case JobSelectOption.NewGame:
                NewGame();
                break;

            case JobSelectOption.LoadGame:
                LoadGame();
                break;

            case JobSelectOption.Exit:
                Environment.Exit(0);
                break;
        }
    }

    private void NewGame()
    {
        string userID = "";
        int selectedOption = 0;
        List<string> jobOptions = new List<string> { "Unity 개발자", "Unreal 개발자", "AI 개발자", "PM(Product manager)", "QA(Quality Assurance)" };
        List<ConsoleKey> NeoKey = new List<ConsoleKey>() { ConsoleKey.RightArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.LeftArrow, ConsoleKey.UpArrow, ConsoleKey.DownArrow };


        string[] backgroundOptions = { "언제는 하루 종일 코딩만 한 적이 있어요. 그래서인지 하루 12시간 정도는 전혀 지치치 않죠."     //HP
        ,"저는 초등학교 때 이미 선생님의 타자 속도를 넘었어요. 이제 영타까지 섭렵한 저에게 긴 문장이란 없습니다."                   //DMG
        ,"전 컴퓨터공학과를 나왔으며, C언어에 대한 탄탄한 지식을 소유하고 있습니다."                                                 //DEF
        ,"버그 하나에 일주일동안 시달려 본 적 있어서, 이제 웬만한 버그에는 눈도 깜박하지 않습니다."                                        //Mental
        ,"비싼 의자, 비싼 마우스, 비싼 모니터와 키보드. 제가 보기엔 코딩도 아이템이 무척 중요하다고 생각합니다."};                            //Gold

        string[] weakOptions = { "최근 계속 앉아만 있다보니 걸어다니면 무릎이 좀 아프고, 체력이 약해지는 게 느껴져요."          //HP
        ,"요즘 너무 컴퓨터를 오래 사용했는지 무리하면 손목이 좀 아프더라고요."         //DMG
        ,"다른 일을 하다가 개발을 시작해서 사실 기초는 잘 모르고 느낌대로 하고 있어요."         //DEF
        ,"저는 다 좋은데 화가 좀 많다고 하더라고요. 그런가? 어떤 새끼들이 그렇게 말하고 다니는 걸까."         //Mental
        ,"제가 컴퓨터가 좀 느린데 괜찮겠죠?? 빌드에 5분정도는 다 걸리는거 맞죠..?"};       //Gold

        string[] statusUpObt = { "HP", "DMG", "DEF", "Mental", "Gold" };

        List<ConsoleKey> userInput = new List<ConsoleKey>();
        Job userJob;
        string? userName;

        Prologue();

        Clear();
        Renderer.Instance.DrawConsoleBorder();
        WriteLine();
        SetCursorPosition(CursorLeft+1, CursorTop);
        WriteLine("사용할 아이디를 입력해 주세요.");
        SetCursorPosition(1, 35);
        userID = ReadLine();

        Clear();
        Renderer.Instance.DrawConsoleBorder();
        WriteLine();
        WriteInForm("당신은 이 헥사코어 안에서 새로운 코드명으로 불리게 될 것입니다.", ConsoleColor.White);
        WriteInForm("당신을 드러낼 코드네임을 정해주세요.", ConsoleColor.White );
        SetCursorPosition(1, 35);
        userName = ReadLine();

        while (true)
        {
            Clear();
            Renderer.Instance.DrawConsoleBorder();
            WriteLine();
            WriteInForm("당신은 어떤 일을 하고 있나요?", ConsoleColor.White);
            SetCursorPosition(0, 5);

            for (int i = 0; i < jobOptions.Count; i++)
            {
                if (i == selectedOption)
                {
                    WriteInForm("-> " + jobOptions[i],ConsoleColor.Blue);
                }
                else
                {
                    WriteInForm("   " + jobOptions[i], ConsoleColor.White);
                }
                WriteLine();
            }
            ConsoleKeyInfo keyInfo = ReadKey();
            userInput.Add(keyInfo.Key);
            if (userInput.Count > 6 && jobOptions.Count == 5)
            {
                for (int i = 0; i < userInput.Count - 5; i++)
                {
                    if (NeoKey.SequenceEqual(userInput.GetRange(i, 6)))
                    {
                        jobOptions.Add("Neo");
                    }
                }
            }
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedOption = (selectedOption == 0) ? jobOptions.Count - 1 : selectedOption - 1;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedOption = (selectedOption == jobOptions.Count - 1) ? 0 : selectedOption + 1;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break; // 엔터 누르면 루프 탈출
            }
        }

        userJob = (Job)selectedOption;
        _player = new Player(userID, userName, userJob);

        while (true)                //배경 설정
        {
            Clear();
            Renderer.Instance.DrawConsoleBorder();
            WriteLine();
            WriteInForm("지금까지 어떻게 살아왔는지 알려주세요.",ConsoleColor.White);
            SetCursorPosition(0, 5);

            for (int i = 0; i < backgroundOptions.Length; i++)
            {
                if (i == selectedOption)
                {
                    WriteInForm("-> " + backgroundOptions[i],ConsoleColor.Blue);
                    WriteInForm("   - " + statusUpObt[i] + " Up!", ConsoleColor.Blue);
                }
                else
                {
                    WriteInForm("   " + backgroundOptions[i], ConsoleColor.White);
                }
                WriteLine();
            }

            ConsoleKeyInfo keyInfo = ReadKey();
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
        switch ((BackgroundSelectOption)selectedOption)
        {
            case BackgroundSelectOption.HPup:
                _player.HP += 50;
                _player.CurrentHp += 50;
                break;

            case BackgroundSelectOption.DMGup:
                _player.TypingSpeed += 5;
                break;

            case BackgroundSelectOption.DEFup:
                _player.C += 5;
                break;

            case BackgroundSelectOption.Mentalup:
                _player.Mental += 50;
                _player.CurrentMental += 50;
                break;

            case BackgroundSelectOption.Goldup:
                _player.Gold += 500;
                break;
        }


        while (true)                //배경 설정
        {
            Clear();
            Renderer.Instance.DrawConsoleBorder();
            WriteLine();
            WriteInForm("항상 좋은 일만 있는 건 아니었잖아요. 그렇죠?", ConsoleColor.White);
            SetCursorPosition(0, 5);

            for (int i = 0; i < weakOptions.Length; i++)
            {
                if (i == selectedOption)
                {
                    WriteInForm("-> " + weakOptions[i], ConsoleColor.Blue);
                    WriteInForm("   - " + statusUpObt[i] + " Down", ConsoleColor.Blue);
                }
                else
                {
                    WriteInForm("   " + weakOptions[i], ConsoleColor.White);
                }
                WriteLine();
            }

            ConsoleKeyInfo keyInfo = ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedOption = (selectedOption == 0) ? weakOptions.Length - 1 : selectedOption - 1;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedOption = (selectedOption == weakOptions.Length - 1) ? 0 : selectedOption + 1;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break; // 엔터 누르면 루프 탈출
            }
        }
        switch ((WeakSelectOption)selectedOption)
        {
            case WeakSelectOption.HPdown:
                _player.HP -= 30;
                _player.CurrentHp -= 30;
                break;

            case WeakSelectOption.DMGdown:
                _player.TypingSpeed -= 3;
                break;

            case WeakSelectOption.DEFdown:
                _player.C -= 3;
                break;

            case WeakSelectOption.Mentaldown:
                _player.Mental -= 30;
                _player.CurrentMental -= 30;
                break;

            case WeakSelectOption.Golddown:
                _player.Gold -= 300;
                break;
        }
        SetInvenItem();
        SaveData();
        Managers.Scene.LoadScene(SCENE_NAME.LOBBY);
    }

    private void LoadGame()
    {

        // string loadData = File.ReadAllText(UtilityPath + "/savePlayer.json");         //캐릭터 정보 불러오기

        /* By. 희성
         * 기존 내용을 리소스매니저가 대체하게 했습니다. */
        string loadData = Managers.Resource.GetTextResource(ResourceKeys.SavePlayer);
        // 데이터가 존재하지 않을경우
        if (loadData == string.Empty)
        {
            NewGame();
        }
        _player = JsonConvert.DeserializeObject<Player>(loadData);

        while (true)
        {
            Clear();
            Renderer.Instance.DrawConsoleBorder();
            SetCursorPosition(0, 1);
            WriteInForm("사용하던 아이디를 입력해 주세요.",ConsoleColor.White);
            WriteInForm("나가기 - ESC", ConsoleColor.White);
            SetCursorPosition(1, 38);
            if(ConsoleKey.Escape == ReadKey().Key)
            {
                LoginScene();
            }
            if (_player.ID == ReadLine())
            {
                SetCursorPosition(CursorLeft, CursorTop+1);
                WriteInForm("사용자를 확인하였습니다.",ConsoleColor.Blue);
                Thread.Sleep(1500);
                break;
            }
            SetCursorPosition(CursorLeft, CursorTop + 1);
            WriteInForm("데이터가 일치하지 않습니다.", ConsoleColor.Blue);
            Thread.Sleep(1500);
        }
        AudioController(_newWorldBGM, PlayOption.Change);
        Managers.Scene.LoadScene(SCENE_NAME.BATTLE);

    }

    private void Prologue()
    {
        Clear();
        Renderer.Instance.DrawConsoleBorder();
        SetCursorPosition(0, 35);
        WriteInForm("오늘따라 옛날 생각이 난다.", ConsoleColor.White);
        Thread.Sleep(1500);
        WriteInForm("언제였더라? 돌아보면 시간이 이렇게도 빠르다.", ConsoleColor.White);
        Thread.Sleep(3000);

        Clear();
        Renderer.Instance.DrawConsoleBorder();
        SetCursorPosition(0, 35);
        WriteInForm("동기들과 내배캠을 수료할 때만 해도 이런저런 걱정이 많았었는데", ConsoleColor.White);
        Thread.Sleep(1500);
        WriteInForm("금방 이렇게 번듯한 회사에 취업하게 될 줄이야..",ConsoleColor.White);
        Thread.Sleep(3000);

        Clear();
        Renderer.Instance.DrawConsoleBorder();
        SetCursorPosition(0, 35);
        WriteInForm("어라..? 근데 왜 이렇게 어지럽....", ConsoleColor.White);
        Thread.Sleep(3000);

        AudioController(_newWorldBGM, PlayOption.Change);
        //사운드 변환
        Clear();
        Renderer.Instance.DrawConsoleBorder();
        SetCursorPosition(0, 35);
        WriteInForm("잘 살아가던 당신은 알 수 없는 이유로 이상한 세계로 이동해버렸다!", ConsoleColor.Red);
        Thread.Sleep(3000);

        Clear();
        Renderer.Instance.DrawConsoleBorder();
        SetCursorPosition(0, 35);
        WriteInForm("이곳은 놀랍게도 몬스터가 존재하는 세상.", ConsoleColor.Red);
        Thread.Sleep(1500);
        WriteInForm("몬스터를 쓰러트리기 위해서는 코딩으로만 공격할 수 있다고 한다?!", ConsoleColor.Red);
        Thread.Sleep(3000);

        Clear();
        Renderer.Instance.DrawConsoleBorder();
        SetCursorPosition(0, 35);
        WriteInForm("승리를 위한 코딩!", ConsoleColor.Red);
        Thread.Sleep(1500);
        WriteInForm("생존을 위한 디버깅!", ConsoleColor.Red);
        Thread.Sleep(3000);

        Clear();
        Renderer.Instance.DrawConsoleBorder();
        SetCursorPosition(0, 35);
        WriteInForm("이 험난한 세상에서, 당신은 살아갈 수 있을 것인가...",ConsoleColor.Red);
        Thread.Sleep(3000);
    }
    private void WriteInForm(string s,ConsoleColor c)
    {
        SetCursorPosition(CursorLeft+1, CursorTop);
        ForegroundColor = c;
        WriteLine(s);
        ResetColor();
    }

    private void SaveData()
    {
        // By. 희성
        // Literals.cs에 저장되야할 json파일의 이름이 지정되었습니다.
        // 경로 지정 (리소스 폴더와 json파일의 이름을 합침)
        var playerDataPath = Path.Combine(Managers.Resource.GetResourceFolderPath(), Literals.PlayerDataPath);

        string playerData = JsonConvert.SerializeObject(_player, Formatting.Indented);        //캐릭터 정보 저장
        File.WriteAllText(playerDataPath, playerData);

        //File.WriteAllText(UtilityPath+"/savePlayer.json", playerData);

        //string dungeonData =                  //던전 정보도 저장할 예정.
    }

    private void SetItemBox()
    {
        string file = Managers.Resource.GetTextResource(ResourceKeys.ItemList);
        ItemBox = JsonConvert.DeserializeObject<List<Item>>(file);
    }

    private void SetInvenItem()
    {
        _player.Inventory.Add(new InventoryItem(ItemBox[0].ItemName, false, false, 1));
        _player.Inventory.Add(new InventoryItem(ItemBox[1].ItemName, false, false, 1));
        _player.Inventory.Add(new InventoryItem(ItemBox[2].ItemName, false, false, 1));
        _player.Inventory.Add(new InventoryItem(ItemBox[3].ItemName, false, false, 1));
        _player.Inventory.Add(new InventoryItem(ItemBox[4].ItemName, false, false, 1));
    }

    public override void Stop()
    {
        Clear();
        AudioController(Managers.Resource.GetSoundResource(ResourceKeys.newWorldBGM), PlayOption.LoopStop);
    }
}



enum JobSelectOption
{
    NewGame,
    LoadGame,
    Exit
}

enum BackgroundSelectOption
{
    HPup,
    DMGup,
    DEFup,
    Mentalup,
    Goldup
}

enum WeakSelectOption
{
    HPdown,
    DMGdown,
    DEFdown,
    Mentaldown,
    Golddown
}