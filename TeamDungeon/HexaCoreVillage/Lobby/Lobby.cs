
using HexaCoreVillage.Utility;

namespace HexaCoreVillage.Lobby;

public class Lobby : Scene
{
    #region Member Variables
    public override SCENE_NAME SceneName => SCENE_NAME.LOBBY;
    private Player _player = new Player(); // 예시입니다.
    private bool isCreditFlag = false;
    #endregion



    #region Flow Methods
    public override void Start()
    {
        Console.WriteLine("STart");
    }

    public override void Update()
    {
        // while(true) { }안에 작성하신다고 생각하면 됩니다.
        // Update라고 생각 X : while(true) {  // 내용 }
        PrintLobbyMessage();
        InputLobby();
    }
    #endregion

    #region Logic Methods
    private void InputLobby()
    {
        var value = int.Parse(Console.ReadKey().KeyChar.ToString());
        if (value < 0 || value > 9)
        {
            Console.WriteLine("다시 입력바랍니다.");
            Thread.Sleep(1000);
        }
        else
        {
            switch(value)
            {
                case 0:
                    // 종료
                    break;
                case 1:
                    // 스테이터스 씬으로 넘김
                    Managers.Scene.LoadScene(SCENE_NAME.STATUS);
                    break;
                case 2:
                    // 만약 여기서 또 입력처리를 해야한다면? 또 무한반복 하면된다.
                    while (!isCreditFlag) // 플래그를 사용하면 로직을 모듈화 하기 편하다.
                    {
                        PrintCreditMessage();
                        InputCreditMessage();
                    }
                    break;
                case 3:
                    // 함수 안에서 무한 루프를 돌려도된다.
                    PrintLoopingTest();
                    break;
            }
        }
    }

    private void InputCreditMessage()
    {
        var myString = Console.ReadLine();

        if (myString == "희성")
            isCreditFlag = true;
        else
        {
            Console.Write("다시 입력하라고 (아무키 눌러)");
            Console.ReadKey();
        }
    }
    #endregion

    #region Render Methods
    private void PrintLoopingTest()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("A. 던전 입장");
            Console.WriteLine("B");
            Console.WriteLine("C");
            Console.WriteLine("D");
            Console.WriteLine("E");
            Console.WriteLine("F");
            Console.WriteLine("G");
            Console.WriteLine("H. 나가기");
            Console.WriteLine("");
            Console.WriteLine("입력>>");

            var value = Console.ReadLine();
            if (value == "H")
                break;
            else if (value == "A")
                PrintDungeon();
        }
    }

    private void PrintDungeon()
    {
        int value = -1;

        while (value != 0)
        {
            Console.Clear();
            Console.WriteLine("던전성공");
            Console.WriteLine("1 키를 누르면 던전입구로 돌아갑니다.");
            Console.WriteLine("0 키를 누르면 던전을 나갑니다.");
            value = int.Parse(Console.ReadKey().KeyChar.ToString());
            if (value == 1)
                PrintLoopingTest();
        }
    }

    private void PrintCreditMessage()
    {
        Console.Clear();
        Console.WriteLine("THIS IS A CREDIT");
        Console.WriteLine("THIS IS A CREDIT");
        Console.WriteLine("1. 정훈");
        Console.WriteLine("2. 연호");
        Console.WriteLine("3. 승철");
        Console.WriteLine("4. 준호");
        Console.WriteLine("5. 형석");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("입력>>");
    }

    private void PrintLobbyMessage()
    {
        Console.Clear();
        Console.WriteLine("HEXA VILLAGE");
        Console.WriteLine("헥사 빌리지에 오신걸 환영합니다.");
        Console.WriteLine("(메뉴를 선택하세요)");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("1. 스테이터스");
        Console.WriteLine("2. 상점");
        Console.WriteLine("3. 인벤토리");
        Console.WriteLine("4. 던전");
        Console.WriteLine("5. 휴식");
        Console.WriteLine("6. 타이틀로");
        Console.WriteLine("0. 종료");
        Console.WriteLine("입력 > ");
    }
    #endregion
}