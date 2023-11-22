
using HexaCoreVillage.Framework;
using HexaCoreVillage.Utility;
using Newtonsoft.Json;

namespace HexaCoreVillage.Lobby;

public class Store : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.STORE;

    static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    static string ItemListPath = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", "..", "..", "TeamDungeon", "HexaCoreVillage", "Utility", "ItemList.json"));
    private static List<Item> items = new List<Item>();
    private static List<Item>? itemdata;
    private static Player  player = new Player();
    private static Item item = new Item();

    public override void Start()
    {
       StreamReader json = new(ItemListPath);
        string jsonString = json.ReadToEnd();
        itemdata = JsonConvert.DeserializeObject<List<Item>>(jsonString);
        DisplayStore();
    }

    public override void Update()
    {

    }

    // 상점 입장했을 때
    private static void DisplayStore()
    {
        int selectOtion = 0;
        string[] storeOption = {"구매", "판매", "나가기"};

        while (true)
        {
            Clear();

            WriteLine("상점에 오신 것을 환영합니다.");
            WriteLine("구매 or 판매를 선택해주세요.");
            for (int i = 0; i < storeOption.Length; i++)
            {
                if (i == selectOtion)
                {
                    ForegroundColor = ConsoleColor.Blue;
                    Write($"->  {storeOption[i]}     ");
                    ResetColor();
                }
                else
                {
                    Write($"  {storeOption[i]}     ");
                }
            }
                ConsoleKeyInfo selectKey = ReadKey();
                if (selectKey.Key == ConsoleKey.LeftArrow)
                {
                    selectOtion = (selectOtion == 0) ? storeOption.Length - 1 : selectOtion - 1;
                }
                else if (selectKey.Key == ConsoleKey.RightArrow)
                {
                    selectOtion = (selectOtion == storeOption.Length-1) ? 0 : selectOtion + 1;
                }
                else if (selectKey.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
            switch ((StoreSelect)selectOtion)
            {
                case StoreSelect.StoreBuyScene:
                    StoreScene();
                    break;
                case StoreSelect.StoreSellScene:
                    StoreSellScene();
                    break;
                case StoreSelect.Exit:
                WriteLine("로비로!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    break;
            }
    }


    // Store에 입장했을 때 출력
    private static void StoreScene()
    {
        string typeSelect = "";
        int selectOtion = 0;
        string[] storeOption = { "Keyboard", "Mouse", "Monitor", "Notebook", "HeadSet", "EnergyDrink", "Exit" };
        while (true)
        {
            Clear();

            WriteLine("▣ 상점 ▣");
            WriteLine("필요한 아이템을 얻을 수 있는 상태입니다.");
            WriteLine();
            WriteLine("[ 보유 골드 ]");
            WriteLine($"{player.Gold} G");
            WriteLine();
            WriteLine("[ 아이템 목록 ]");
            WriteLine("----------------------------------------------------------------");
            for (int i = 0; i < itemdata.Count; i++)
            {
                WriteLine($"-  {itemdata[i].ItemName}     {itemdata[i].Type}     {itemdata[i].ItemOption}     {itemdata[i].Desc}");
                WriteLine("----------------------------------------------------------------");
            }
            WriteLine();

            for (int i = 0; i < storeOption.Length; i++)
            {
                if (i == selectOtion)
                {
                    ForegroundColor = ConsoleColor.Blue;
                    WriteLine("->" + storeOption[i]);
                    ResetColor();
                }
                else
                {
                    WriteLine(" " + storeOption[i]);
                }
            }
            ConsoleKeyInfo selectKey = ReadKey();
            if (selectKey.Key == ConsoleKey.UpArrow)
            {
                selectOtion = (selectOtion == 0) ? storeOption.Length - 1 : selectOtion - 1;
            }
            else if (selectKey.Key == ConsoleKey.DownArrow)
            {
                selectOtion = (selectOtion == storeOption.Length - 1) ? 0 : selectOtion + 1;
            }
            else if (selectKey.Key == ConsoleKey.Enter)
            {
                typeSelect = storeOption[selectOtion];
                break;
            }
        }

        switch ((ItemTypeSelet)selectOtion)
        {
            case ItemTypeSelet.Keyboard:
                StoreBuyScene(typeSelect);
                break;
            case ItemTypeSelet.Mouse:
                StoreBuyScene(typeSelect);
                break;
            case ItemTypeSelet.Monitor:
                StoreBuyScene(typeSelect);
                break;
            case ItemTypeSelet.Notebook:
                StoreBuyScene(typeSelect);
                break;
            case ItemTypeSelet.HeadSet:
                StoreBuyScene(typeSelect);
                break;
            case ItemTypeSelet.EnergyDrink:
                StoreBuyScene(typeSelect);
                break;
            case ItemTypeSelet.Exit:
                DisplayStore();
                break;
        }
    }

    private static void StoreBuyScene(string typeSelect="")
    {
        List<Item> diviList = new List<Item>();
        int selectOtion = 0;
        string[] storeOption = { };

        for(int i = 0; i < itemdata.Count; i++)
        {
            if(typeSelect == itemdata[i].Type.ToString())
            {
                storeOption = itemdata[i].ItemName.Split(',');
                diviList.Add(itemdata[i]);
            }
        }
        while (true)
        {
            Clear();

            WriteLine("▣ 상점 - 아이템 구매 ▣");
            WriteLine("필요한 아이템을 얻을 수 있는 상태입니다.");
            WriteLine();
            WriteLine("[ 보유 골드 ]");
            WriteLine($"{player.Gold} G");
            WriteLine();
            WriteLine("[ 아이템 목록 ]");

            // 아이템리스트를 불러오면 방향키로 이동하면서 선택
            for (int i = 0; i <diviList.Count; i++)
            {
                WriteLine("----------------------------------------------------------------");
                if (i == selectOtion)
                {
                    ForegroundColor = ConsoleColor.Blue;
                    WriteLine($"->  {diviList[i].ItemName}     {diviList[i].Type}     {diviList[i].ItemOption}     {diviList[i].Desc}    {diviList[i].Price}");
                    ResetColor();
                }
                else
                {
                    WriteLine($"  {diviList[i].ItemName}     {diviList[i].Type}     {diviList[i].ItemOption}     {diviList[i].Desc}    {diviList[i].Price}");
                }
            }
            WriteLine("----------------------------------------------------------------");

            ConsoleKeyInfo selectKey = ReadKey();
            if (selectKey.Key == ConsoleKey.UpArrow)
            {
                selectOtion = (selectOtion == 0) ? storeOption.Length  - 1 : selectOtion - 1;
            }
            else if (selectKey.Key == ConsoleKey.DownArrow)
            {
                selectOtion = (selectOtion == storeOption.Length ) ? 0 : selectOtion + 1;
            }
            else if (selectKey.Key == ConsoleKey.Escape)
            {
                StoreScene();
            }
            else if (selectKey.Key == ConsoleKey.Enter)
            {
                if (player.Gold >= diviList[selectOtion].Price)
                {
                    player.Gold -= diviList[selectOtion].Price;
                    player.Inventory.Add(new(diviList[selectOtion].ItemName, false, true, 1));
                    WriteLine($"{diviList[selectOtion].ItemName}을(를) 구매하셨습니다.");
                }
                else if(player.Gold < diviList[selectOtion].Price)
                {
                    WriteLine("소지금이 부족합니다.");
                }
                else
                {
                    WriteLine("잘못된 입력입니다.");
                }
                WriteLine();
                WriteLine($"현재 소지금 : {player.Gold}");
                WriteLine("아무 키나 누르면 상점으로 돌아갑니다.");
                ReadKey();
                StoreScene();
                break;
            }
        }

        switch ((StoreSelect)selectOtion)
        {
            case StoreSelect.Exit:
                StoreScene();
                break;
        }
    }

    private static void StoreSellScene()
    {
        // 아이템 판매 만들 곳
        // 인벤토리 데이터를 불러와야하는데 아직 데이터가 없어서 불러오지 못함
        // 인벤토리가 만들어지면 그 때 할 예정
        // UI자체는 상점 혹은 만들어지는 인벤과 비슷할 예정이기 때문에 순서는 뒤로 두었습니다.

        int selectOtion = 0;
        string[] storeOption = { };

        for(int i = 0; i < player.Inventory.Count; i++)
        {
            storeOption = player.Inventory[i].ItemName.Split(",");
        }

        while (true)
        {
            Clear();

            WriteLine("▣ 상점 - 아이템 판매▣");
            WriteLine("사용하지 않는 아이템을 판매하는 곳입니다.");
            WriteLine();
            WriteLine("[ 보유 골드 ]");
            WriteLine($"{player.Gold} G");
            WriteLine();
            WriteLine("[ 아이템 목록 ]");
            WriteLine("----------------------------------------------------------------");
            if (player.Inventory.Count > 0)
            {
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    if (i == selectOtion)
                    {
                        ForegroundColor = ConsoleColor.Blue;
                        WriteLine($"->  {player.Inventory[i].ItemName}");
                        ResetColor();
                    }
                    else
                    {
                        WriteLine($"-  {player.Inventory[i].ItemName}");
                    }
                }
            }
            else
            {
                WriteLine("소지하고 있는 아이템이 없습니다.");
            }
            WriteLine();

            ConsoleKeyInfo selectKey = ReadKey();
            if (selectKey.Key == ConsoleKey.UpArrow)
            {
                selectOtion = (selectOtion == 0) ? storeOption.Length - 1 : selectOtion - 1;
            }
            else if (selectKey.Key == ConsoleKey.DownArrow)
            {
                selectOtion = (selectOtion == storeOption.Length - 1) ? 0 : selectOtion + 1;
            }
            else if (selectKey.Key == ConsoleKey.Enter)
            {
                // 소지금 = 소지금 + 아이템 판매 가격
                // 인벤 - 선택아이템 Remove
                if (player.Inventory.Count > 0)
                {
                    // 소지금 = 소지금 + 아이템 판매 가격
                    // 인벤 - 선택아이템 Remove
                    player.Inventory.Remove(player.Inventory[selectOtion]);
                    WriteLine("판매가 완료되었습니다.");
                }
                else
                {
                    WriteLine("판매할 아이템이 없으므로 메인화면으로 넘어갑니다.");
                    ReadKey();
                    DisplayStore();
                }
                WriteLine($"현재 소지금 : {player.Gold}");
                WriteLine("아무 키나 누르면 상점으로 돌아갑니다.");
                ReadKey();
                DisplayStore();
                break;
            }
        }
        switch ((StoreSelect)selectOtion)
        {
            case StoreSelect.Exit:
                break;
        }
    }

    public override void Stop()
    {
        throw new NotImplementedException();
    }
}

enum StoreSelect
{
    StoreBuyScene,
    StoreSellScene,
    Exit
}

enum ItemTypeSelet
{
    Keyboard,
    Mouse,
    Monitor,
    Notebook,
    HeadSet,
    EnergyDrink,
    Exit
}