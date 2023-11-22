using HexaCoreVillage.Utility;
using Newtonsoft.Json;

namespace HexaCoreVillage.Dungeon;

public class Reward : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.REWARD;
    private static Player player = new Player();
    private static string battleResult = "";
    public static Random random = new Random();
    string CurrentDirectory = Directory.GetCurrentDirectory();
    private static int limitExp = 100;


    public override void Start()
    {
        
        Clear();
        // 초기화 해주는 부분,
        // 아래 부분 따로 메서드 선언 후 분리,
        // 눈에 띄지 않는 부분
        // 파일들 json 으로 저장하기 ?? 어떻게, login.cs의 SaveData()를 사용?
        if (Data.BattleSuccess == true)
        {
            SuccessReward();            
        }
        else
        {
            if (player.Gold > 0)
            {
                FailReward();                
            }
        }
        if (player.Exp >= limitExp) LevelUp();
        DisplayReward();
        SaveData();
    }

    private void SaveData()
    {
        var playerDataPath = Path.Combine(Managers.Resource.GetResourceFolderPath(), Literals.PlayerDataPath);

        string playerData = JsonConvert.SerializeObject(player, Formatting.Indented);        //캐릭터 정보 저장
        File.WriteAllText(playerDataPath, playerData);

        //string dungeonData =                  //던전 정보도 저장할 예정.
    }

    private static void DisplayReward()
    {
        SetCursorPosition(4, 10);
        WriteLine("[전투 결과입니다]");
        SetCursorPosition(4, 12);
        Write($"디버그 결과는 : {battleResult} 입니다!");

        Renderer.Instance.DrawConsoleBorder();

        WriteLine();
        SetCursorPosition(4, 13);
        WriteLine($"플레이어의 골드는 {player.Gold}G입니다.");
        SetCursorPosition(4, 14);
        WriteLine($"플레이어의 현재 상태는");
        SetCursorPosition(4, 15);
        WriteLine($"플레이어 HP : {Battle.CurrentHp}/{player.HP}");
        SetCursorPosition(4, 16);
        WriteLine($"플레이어 Metal : {Battle.CurrentMental}/{player.Mental}");
        SetCursorPosition(4, 17);
        WriteLine($"플레이어 Exp : {player.Exp}");
        SetCursorPosition(4, 18);
        WriteLine($"플레이어 레벨 : {player.Level}");
        Renderer.Instance.DrawConsoleBorder();
    }

    private static void SuccessReward()
    {
        // 계획은 레벨에 따른 배틀 성공 리워드 수치 증가
        player.Gold += random.Next(100, 300) * player.Level; // 현재 주는 골드 수(랜덤 100 ~ 500) * 현제 레벨 * 처치 버그 수 
        player.Exp += random.Next(50, 100) * Data.BugCount;   // 현재 주는 경험치 량(적게)*레벨 * 처지 버그 수
        battleResult = "Debug Complete";
    }

    private static void FailReward()
    {
        // 여기도 마찬가지로 
        player.Gold -= random.Next(50, 100);
        battleResult = "Debug Delayed";
    }

    private static void LevelUp()
    {
        SetCursorPosition(4, 5);
        WriteLine("!!! 레벨업 !!!");
        SetCursorPosition(4, 6);
        Write($"현재 레벨 {player.Level}에서 ");

        player.Exp -= limitExp;
        limitExp = limitExp + (player.Level * 20);
        player.Level += 1;

        WriteLine($"레벨 {player.Level}로 올랐습니다.");
        SetCursorPosition(4, 7);
        WriteLine($"현제 다음 레벨업을 하기 위한 필요 경험치 수는 {limitExp - player.Exp} 가 필요합니다.");
        SetCursorPosition(4, 8);
        WriteLine($"LimitExp : {limitExp}, currentExp: {player.Exp}");
        // 일정 경험치 한계 이상이 되면 플레이어의 레벨을 업해야 한다
        // 그러기 위해선 플레이어 정보가 담긴 Json 파일에 접근해서 수정해야 한다
        // 문제는 레벨을 포함한 다른 player 수치를 어떻게 담아야 하는가?
        // 이 문제는 캐릭터 설정 관련으로 준호씨나 연호씨한테 물어봐야 한다 
    }

    public override void Stop()
    {
        
    }

    public override void Update()
    {
        ChooseScene();
        // 기능적으로 계속 업데이트 해야 하는 부분
        // 이 부분은 다른 분들 처럼 커서를 위 아래로 이동시키는 목록을 작성한다던가?,
        // 아니면 그냥 Readline을 Start에 때려 박아서 원하는 술자를 입력한다던가?
    }

    /// <summary>
    /// 이 메서드는 리워드 적용 후 씬 고르기다.
    /// </summary>
    private static void ChooseScene()
    {
        CursorVisible = false;
        int index = 0;
        int totalMenuOption = 2;
        string[] options = { "계속 디버깅하기", "로비로 돌아가기" };

        while (true)
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
    // 경험치 맥스 값 확인, 연호 & 준호(경험),
    // 키인포값 찾아보고 적용
}