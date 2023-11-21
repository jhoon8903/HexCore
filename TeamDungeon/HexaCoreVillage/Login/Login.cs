using HexaCoreVillage.Utility;
using Newtonsoft.Json;
using NAudio.Wave;
using TeamDungeon.Utility;
using HexaCoreVillage.Manager;
using System.Text;

namespace HexaCoreVillage.Login;

public class Login : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.LOGIN;

    // by 정훈
    // 다른 PC에서는 절대경로 설정시 File을 찾을 수 없어 상대경로 표시를 위한 변수를 별도로 작성했습니다.
    // 하지만 맥에서는 작동하지 않습니다.
    //  'winmm.dll' 의존성이 문제 입니다. 
    static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    static string relativePathToAudio = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", "..", "..", "TeamDungeon", "HexaCoreVillage"));
    static string startBGMPath = Path.Combine(relativePathToAudio, "startBGM.wav");
    static string newWorldBGMPath = Path.Combine(relativePathToAudio, "newWorldBGM.wav");

    static AudioFileReader startBGM = new AudioFileReader(startBGMPath);
    static LoopStream startLoop = new LoopStream(startBGM);
    static AudioFileReader newWorldBGM = new AudioFileReader(newWorldBGMPath);
    static LoopStream newWorldLoop = new LoopStream(newWorldBGM);
    static WaveOutEvent audioMgr = new WaveOutEvent();

    public static List<Item> ItemBox = new List<Item>();
    public static Player? player = null;
    string CurrentDirectory = Directory.GetCurrentDirectory();
    public override void Start()
    {
        CursorVisible = false;
        audioMgr.Init(startLoop);
        audioMgr.Play();
    }

    public override void Update()
    {
        LoginScene();
    }

    public void LoginScene()
    {
        
        int selectedOption = 0;
        string[] options = { "New Game", "Load Game", "Exit" };
        string[] plusOpt = { "새로운 아이디로 게임을 처음부터 시작합니다.(프롤로그 포함)", "전에 사용하던 아이디로 게임을 시작합니다.(프롤로그 스킵)", "게임 종료하기" };
        while (true)
        {
            Clear();
            SetItemBox();
            SetCursorPosition(0, 28);

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedOption)
                {
                    ForegroundColor = ConsoleColor.Blue;
                    WriteLine("-> " + options[i]);
                    WriteLine("   - " + plusOpt[i]);
                    ResetColor();
                }
                else
                {
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
        WriteLine();
        WriteLine("사용할 아이디를 입력해 주세요.");
        SetCursorPosition(0, 28);
        userID = ReadLine();

        Clear();
        WriteLine();
        WriteLine("당신은 이 헥사코어 안에서 새로운 코드명으로 불리게 될 것입니다.");
        WriteLine("당신을 드러낼 코드네임을 정해주세요.");
        SetCursorPosition(0, 28);
        userName = ReadLine();

        while (true)
        {
            Clear();
            WriteLine();
            WriteLine("당신은 어떤 일을 하고 있나요?");
            SetCursorPosition(0, 5);

            for (int i = 0; i < jobOptions.Count; i++)
            {
                if (i == selectedOption)
                {
                    ForegroundColor = ConsoleColor.Blue; // 선택중인 옵션
                    WriteLine("-> " + jobOptions[i]);
                    ResetColor();
                }
                else
                {
                    WriteLine("   " + jobOptions[i]);
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
        player = new Player(userID, userName, userJob);

        while (true)                //배경 설정
        {
            Clear();
            WriteLine();
            WriteLine("지금까지 어떻게 살아왔는지 알려주세요.");
            SetCursorPosition(0, 5);

            for (int i = 0; i < backgroundOptions.Length; i++)
            {
                if (i == selectedOption)
                {
                    ForegroundColor = ConsoleColor.Blue; // 선택중인 옵션
                    WriteLine("-> " + backgroundOptions[i]);
                    WriteLine("   - " + statusUpObt[i] + " Up!");
                    ResetColor();
                }
                else
                {
                    WriteLine("   " + backgroundOptions[i]);
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
                player.HP += 50;
                player.CurrentHp += 50;
                break;

            case BackgroundSelectOption.DMGup:
                player.TypingSpeed += 5;
                break;

            case BackgroundSelectOption.DEFup:
                player.C += 5;
                break;

            case BackgroundSelectOption.Mentalup:
                player.Mental += 50;
                player.CurrentMental += 50;
                break;

            case BackgroundSelectOption.Goldup:
                player.Gold += 500;
                break;
        }


        while (true)                //배경 설정
        {
            Clear();
            WriteLine();
            WriteLine("항상 좋은 일만 있는 건 아니었잖아요. 그렇죠?");
            SetCursorPosition(0, 5);

            for (int i = 0; i < weakOptions.Length; i++)
            {
                if (i == selectedOption)
                {
                    ForegroundColor = ConsoleColor.Blue; // 선택중인 옵션
                    WriteLine("-> " + weakOptions[i]);
                    WriteLine("   - " + statusUpObt[i] + " Down");
                    ResetColor();
                }
                else
                {
                    WriteLine("   " + weakOptions[i]);
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
                player.HP -= 30;
                player.CurrentHp -= 30;
                break;

            case WeakSelectOption.DMGdown:
                player.TypingSpeed -= 3;
                break;

            case WeakSelectOption.DEFdown:
                player.C -= 3;
                break;

            case WeakSelectOption.Mentaldown:
                player.Mental -= 30;
                player.CurrentMental -= 30;
                break;

            case WeakSelectOption.Golddown:
                player.Gold -= 300;
                break;
        }
        SetInvenItem();
        SaveData();
        Managers.Scene.LoadScene(SCENE_NAME.LOBBY);
    }

    private void LoadGame()
    {
        string loadData = File.ReadAllText(CurrentDirectory + "\\savePlayer.json");         //캐릭터 정보 불러오기
        player = JsonConvert.DeserializeObject<Player>(loadData);

        while (true)
        {
            Clear();
            WriteLine("사용하던 아이디를 입력해 주세요.");
            SetCursorPosition(0, 28);
            if (player.ID == ReadLine())
            {
                ForegroundColor = ConsoleColor.Blue;
                WriteLine("사용자를 확인하였습니다.");
                ResetColor();
                Thread.Sleep(1500);
                break;
            }
            ForegroundColor = ConsoleColor.Blue;
            WriteLine("데이터가 일치하지 않습니다.");
            ResetColor();
            Thread.Sleep(1500);
        }
        audioMgr.Stop();
        audioMgr.Init(newWorldLoop);
        audioMgr.Play();
        Managers.Scene.LoadScene(SCENE_NAME.LOBBY);

    }

    private void Prologue()
    {
        Clear();
        SetCursorPosition(0, 28);
        WriteLine("오늘따라 옛날 생각이 난다.");
        Thread.Sleep(1500);
        WriteLine("언제였더라? 돌아보면 시간이 이렇게도 빠르다.");
        Thread.Sleep(3000);

        Clear();
        SetCursorPosition(0, 28);
        WriteLine("동기들과 내배캠을 수료할 때만 해도 이런저런 걱정이 많았었는데");
        Thread.Sleep(1500);
        WriteLine("금방 이렇게 번듯한 회사에 취업하게 될 줄이야..");
        Thread.Sleep(3000);

        Clear();
        SetCursorPosition(0, 28);
        WriteLine("어라..? 근데 왜 이렇게 어지럽....");
        Thread.Sleep(3000);
        audioMgr.Stop();
        audioMgr.Init(newWorldLoop);
        audioMgr.Play();
        //사운드 변환
        ForegroundColor = ConsoleColor.Red;
        Clear();
        SetCursorPosition(0, 28);
        WriteLine("잘 살아가던 당신은 알 수 없는 이유로 이상한 세계로 이동해버렸다!");
        Thread.Sleep(3000);

        Clear();
        SetCursorPosition(0, 28);
        WriteLine("이곳은 놀랍게도 몬스터가 존재하는 세상.");
        Thread.Sleep(1500);
        WriteLine("몬스터를 쓰러트리기 위해서는 코딩으로만 공격할 수 있다고 한다?!");
        Thread.Sleep(3000);

        Clear();
        SetCursorPosition(0, 28);
        WriteLine("승리를 위한 코딩!");
        Thread.Sleep(1500);
        WriteLine("생존을 위한 디버깅!");
        Thread.Sleep(3000);

        Clear();
        SetCursorPosition(0, 28);
        WriteLine("이 험난한 세상에서, 당신은 살아갈 수 있을 것인가...");
        Thread.Sleep(3000);
        ResetColor();
    }

    private void SaveData()
    {
        string playerData = JsonConvert.SerializeObject(player, Formatting.Indented);        //캐릭터 정보 저장
        File.WriteAllText(CurrentDirectory + ".\\savePlayer.json", playerData);

        //string dungeonData =                  //던전 정보도 저장할 예정.
    }

    private void SetItemBox()
    {
        using StreamReader json = new(CurrentDirectory + "\\ItemData.json", Encoding.UTF8);     //버그 저장한거 보고 따라함;
        string file = json.ReadToEnd();
        ItemBox = JsonConvert.DeserializeObject<List<Item>>(file);
    }

    private void SetInvenItem()
    {
        player.Inventory.Add(new InventoryItem(ItemBox[0].ItemName, false, false, 1));
        player.Inventory.Add(new InventoryItem(ItemBox[1].ItemName, false, false, 1));
        player.Inventory.Add(new InventoryItem(ItemBox[2].ItemName, false, false, 1));
        player.Inventory.Add(new InventoryItem(ItemBox[3].ItemName, false, false, 1));
        player.Inventory.Add(new InventoryItem(ItemBox[4].ItemName, false, false, 1));
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