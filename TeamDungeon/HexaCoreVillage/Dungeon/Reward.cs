using HexaCoreVillage.Utility;

namespace HexaCoreVillage.Dungeon;

public class Reward : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.REWARD;
    private static readonly Player Player = Managers.GM.Player;
    private static string _battleResult = "";
    private static readonly Random Random = new Random();
    private static int _limitExp = 100;


    public override void Start()
    {
        StartCommon();
        Clear();

        // 초기화 해주는 부분,
        // 아래 부분 따로 메서드 선언 후 분리,
        // 눈에 띄지 않는 부분
        // 파일들 json 으로 저장하기 ?? 어떻게, login.cs의 SaveData()를 사용?
        if (Data.BattleSuccess)
        {
            SuccessReward();            
        }
        else
        {
            if (Player.Gold > 0)
            {
                FailReward();                
            }
        }
    }

    private static void ShowReward()
    {
        while (true)
        {
            ConsoleKeyInfo key = ReadKey();

            SetCursorPosition(70, 20);
            Write("결과를 확인하시려면 Enter를 입력 하세요");
            if (key.Key == ConsoleKey.Enter)
            {
                return;
            }
        }
    }

    //private void SaveData()
    //{
    //    var playerDataPath = Path.Combine(Managers.Resource.GetResourceFolderPath(), Literals.PlayerDataPath);

    //    string playerData = JsonConvert.SerializeObject(player, Formatting.Indented);        //캐릭터 정보 저장
    //    File.WriteAllText(playerDataPath, playerData);

    //    //string dungeonData =                  //던전 정보도 저장할 예정.
    //}

    private static void DisplayReward()
    {
        while (true)
        {
            Clear();
            SetCursorPosition(4, 10);
            WriteLine("[전투 결과입니다]");
            SetCursorPosition(4, 12);
            Write($"디버그 결과는 : {_battleResult} 입니다!");


            WriteLine();
            SetCursorPosition(4, 13);
            WriteLine($"플레이어의 골드는 {Player.Gold}G입니다.");
            SetCursorPosition(4, 14);
            WriteLine("플레이어의 현재 상태는");
            SetCursorPosition(4, 15);
            WriteLine($"플레이어 HP : {Battle.CurrentHp}/{Player.HP}");
            SetCursorPosition(4, 16);
            WriteLine($"플레이어 Metal : {Battle.CurrentMental}/{Player.Mental}");
            SetCursorPosition(4, 17);
            WriteLine($"플레이어 Exp : {Player.Exp}");
            SetCursorPosition(4, 18);
            WriteLine($"플레이어 레벨 : {Player.Level}");
            Renderer.Instance.DrawConsoleBorder();
            ConsoleKeyInfo key = ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                return;
            }
        }
    }


    /// <summary>
    /// 배틀 승리 시 골그 및 경험치 지급
    /// </summary>
    private static void SuccessReward()
    {
        // 계획은 레벨에 따른 배틀 성공 리워드 수치 증가
        Player.Gold += Random.Next(100, 300) * Player.Level; // 현재 주는 골드 수(랜덤 100 ~ 500) * 현제 레벨 * 처치 버그 수 
        Player.Exp += Random.Next(50, 100) * Data.BugCount;   // 현재 주는 경험치 량(적게)*레벨 * 처지 버그 수
        _battleResult = "Debug Complete";
    }


    /// <summary>
    /// 배틀 패배 후 골드는 무시됩니다.
    /// </summary>
    private static void FailReward()
    {
        // 여기도 마찬가지로
        if(Player.Gold < 50)
        {
            Player.Gold -= Player.Gold;
        }
        else Player.Gold -= Random.Next(50, 100);
        _battleResult = "Debug Failed";
    }

    private static void LevelUp()
    {
        SetCursorPosition(4, 5);
        WriteLine("!!! 레벨업 !!!");
        SetCursorPosition(4, 6);
        Write($"현재 레벨 {Player.Level}에서 ");

        Player.Exp -= _limitExp;
        _limitExp = _limitExp + (Player.Level * 20);
        Player.Level += 1;

        WriteLine($"레벨 {Player.Level}로 올랐습니다.");
        SetCursorPosition(4, 7);
        WriteLine($"현제 다음 레벨업을 하기 위한 필요 경험치 수는 {_limitExp - Player.Exp} 가 필요합니다.");
        SetCursorPosition(4, 8);
        WriteLine($"LimitExp : {_limitExp}, currentExp: {Player.Exp}");
        // 일정 경험치 한계 이상이 되면 플레이어의 레벨을 업해야 한다
        // 그러기 위해선 플레이어 정보가 담긴 Json 파일에 접근해서 수정해야 한다
        // 문제는 레벨을 포함한 다른 player 수치를 어떻게 담아야 하는가?
        // 이 문제는 캐릭터 설정 관련으로 준호씨나 연호씨한테 물어봐야 한다 
    }

    public override void Stop()
    {
        StopCommon();
    }

    public override void Update()
    {
        UpdateCommon();
        if (Player.Exp >= _limitExp) LevelUp();
        ShowReward();
        DisplayReward();
        Data.SavePlayerData(Player);
        ChooseScene();
        //SaveData();
    }

    /// <summary>
    /// 이 메서드는 리워드 적용 후 씬 고르기다.
    /// </summary>
    private void ChooseScene()
    {
        CursorVisible = false;
        int index = 0;
        int totalMenuOption = 2;
        string[] options = { "계속 디버깅하기", "로비로 돌아가기" };

        while (_isRun)
        {
            Clear();
            SetCursorPosition(4, 6);
            Write("[현재 해결해야 하는 버그들은 사라졌습니다]");
            SetCursorPosition(4, 7);
            Write("[앞으로 무얼 하시겠습니까?]\n");
            for (int i = 0; i < totalMenuOption; i++)
            {
                if (i == index)
                {
                    ForegroundColor = ConsoleColor.Green;
                }
                SetCursorPosition(4, 9+i);
                Write($"{i+1}. {options[i]}");
                ResetColor();
            }
            Renderer.Instance.DrawConsoleBorder();
            ConsoleKeyInfo key = ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    index = (index - 1 + totalMenuOption) % totalMenuOption;
                    break;
                case ConsoleKey.DownArrow:
                    index = (index + 1) % totalMenuOption;
                    break;
                case ConsoleKey.Enter:
                    if (index == 0)
                    {
                        Managers.Scene.LoadScene(SCENE_NAME.BATTLE);
                    }
                    if (index == 1)
                    {
                        Managers.Scene.LoadScene(SCENE_NAME.LOBBY);
                    }
                    return;
            }
        }
    }

    // Battle 성공시 Data Class의 "bool BattleSuccess" 로  bool 값 넣어두도록 하겠습니다,
    // 해당 bool 값으로 성공 실패 정하시면 될 것 같아요 
    // Battle에서 끝나면 Start() 호출해서 실행하도록 하겠습니다.
}