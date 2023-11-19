
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
    }

    public override void Update()
    {
        StoreScene();
    }

    // Store에 입장했을 때 출력
    public void StoreScene()
    {
        int selectOtion = 0;
        string[] StoreOption = { "아이템 구매", "아이템 판매", "나가기" };
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

            for (int i = 0; i < StoreOption.Length; i++)
            {
                if (i == selectOtion)
                {
                    ForegroundColor = ConsoleColor.Blue;
                    WriteLine("->" + StoreOption[i]);
                    ResetColor();
                }
                else
                {
                    WriteLine(" " + StoreOption[i]);
                }
            }
            ConsoleKeyInfo selectKey = ReadKey();
            if (selectKey.Key == ConsoleKey.UpArrow)
            {
                selectOtion = (selectOtion == 0) ? StoreOption.Length - 1 : selectOtion - 1;
            }
            else if (selectKey.Key == ConsoleKey.DownArrow)
            {
                selectOtion = (selectOtion == StoreOption.Length - 1) ? 0 : selectOtion + 1;
            }
            else if (selectKey.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        switch ((StoreSelect)selectOtion)
        {
            case StoreSelect.StoreBuyScene:
                StoreBuyScene();
                break;
            case StoreSelect.StoreSellScene:
                break;
            case StoreSelect.Exit:
                break;
        }
    }

    private static void StoreBuyScene()
    {
        int CurrentPage = 1;
        int MaxPage = itemdata.Count % 3;
        int selectOtion = 0;
        // ItemList.json을 가져올 예정
        int StoreOption = itemdata.Count;
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
            for (int i = 0; i < itemdata.Count; i++)
            {
                WriteLine("----------------------------------------------------------------");
                if (i == selectOtion)
                {
                    ForegroundColor = ConsoleColor.Blue;
                    WriteLine($"->  {itemdata[i].ItemName}     {itemdata[i].Type}     {itemdata[i].ItemOption}     {itemdata[i].Desc}");
                    ResetColor();
                }
                else
                {
                    WriteLine($"  {itemdata[i].ItemName}     {itemdata[i].Type}     {itemdata[i].ItemOption}     {itemdata[i].Desc}");
                }
            }
            WriteLine("----------------------------------------------------------------");
            // 좌, 우 아이템 페이지전환을 구현할 예정이었으나 아직 구현되지 못했음.
            Write($"                        ◀  {CurrentPage} / {MaxPage}  ▶              " );
            ConsoleKeyInfo selectKey = ReadKey();
            if (selectKey.Key == ConsoleKey.UpArrow)
            {
                selectOtion = (selectOtion == 0) ? StoreOption  - 1 : selectOtion - 1;
            }
            else if (selectKey.Key == ConsoleKey.DownArrow)
            {
                selectOtion = (selectOtion == StoreOption ) ? 0 : selectOtion + 1;
            }
            else if (selectKey.Key == ConsoleKey.LeftArrow)
            {
                selectOtion = (selectOtion == StoreOption - 1) ? 0 : selectOtion;
                CurrentPage--;
                if (CurrentPage < 1)
                {
                    CurrentPage = 1;
                }
            }
            else if (selectKey.Key == ConsoleKey.RightArrow)
            {
                selectOtion = (selectOtion == StoreOption - 1) ? 0 : selectOtion;
                CurrentPage++;
                if (CurrentPage > MaxPage)
                {
                    CurrentPage = MaxPage;
                }
            }
            else if (selectKey.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        switch ((StoreSelect)selectOtion)
        {
            case StoreSelect.StoreSellScene:
                break;
            case StoreSelect.Exit:
                break;
        }
    }

    private static void StoreSellScene()
    {
        // 아이템 판매 만들 곳
        // 인벤토리 데이터를 불러와야하는데 아직 데이터가 없어서 불러오지 못함
        // 인벤토리가 만들어지면 그 때 할 예정
        // UI자체는 상점 혹은 만들어지는 인벤과 비슷할 예정이기 때문에 순서는 뒤로 두었습니다.
    }
}

enum StoreSelect
{
    StoreBuyScene,
    StoreSellScene,
    Exit
}
