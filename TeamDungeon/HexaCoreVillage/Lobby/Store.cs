
using HexaCoreVillage.Framework;
using HexaCoreVillage.Utility;
using static HexaCoreVillage.Utility.DividerLineUtility;
using Newtonsoft.Json;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace HexaCoreVillage.Lobby;

public class Store : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.STORE;

    private static List<Item> items = new List<Item>();
    private static List<Item>? itemdata;
    private static Player _player = Login.Login._player;
    private static Item item = new Item();

    private static bool _isLoadScene = false;

    public override void Start()
    {
        Clear();
        CursorVisible = false;
        LoadItem();
        CreativeUI();
        _isEscape = false;
    }

    public override void Update()
    {
        if (_isEscape) return;
        DisplayStore();
        if (_isLoadScene) return;
        StoreScene();
        StoreBuyScene();
        StoreSellScene();
    }

    public override void Stop()
    {
        Clear();
    }

    private static void LoadItem()
    {
        string json = Managers.Resource.GetTextResource(ResourceKeys.ItemList);
        itemdata = JsonConvert.DeserializeObject<List<Item>>(json);
    }

    // 상점 입장했을 때
    private static void DisplayStore()
    {
        int selectOtion = 0;
        string[] storeOption = Enum.GetNames(typeof(StartStore));


        while (true)
        {
            if (_isLoadScene) return;
            Gold();
            for (int i = 0; i < storeOption.Length; i++)
            {
                SetCursorPosition(55 + (i * 40), 6);
                if (i == selectOtion)
                {
                    ForegroundColor = ConsoleColor.Blue;
                    Write($"{storeOption[i]}");
                    ResetColor();
                }
                else
                {
                    Write($"{storeOption[i]}");
                }
            }
            //아이템 목록 클리어
            diviClear();
            ListClear();

            // 골드 표시
            Gold();

            ConsoleKeyInfo selectKey = ReadKey();
            if (selectKey.Key == ConsoleKey.LeftArrow)
            {
                selectOtion = (selectOtion == 0) ? storeOption.Length - 1 : selectOtion - 1;
            }
            else if (selectKey.Key == ConsoleKey.RightArrow)
            {
                selectOtion = (selectOtion == storeOption.Length - 1) ? 0 : selectOtion + 1;
            }
            else if (selectKey.Key == ConsoleKey.Enter)
            {
                break;
            }
        }
        switch ((StartStore)selectOtion)
        {
            case StartStore.Buy:
                StoreScene();
                break;
            case StartStore.Sell:
                StoreSellScene();
                break;
            case StartStore.Exit:
                _isLoadScene = true;
                Managers.Scene.LoadScene(SCENE_NAME.LOBBY);
                break;
        }
    }


    // Store에 입장했을 때 출력
    private static void StoreScene()
    {
        string typeSelect = "";
        int selectOtion = 0;
        string[] storeOption = Enum.GetNames(typeof(ItemTypeSelet));

        while (true)
        {
            for (int i = 0; i < storeOption.Length; i++)
            {
                SetCursorPosition(30, 12 + i);
                if (i == selectOtion)
                {
                    ForegroundColor = ConsoleColor.Blue;
                    WriteLine(storeOption[i]);
                    ResetColor();
                }
                else
                {
                    WriteLine(storeOption[i]);
                }
            }
            ListClear();

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
            else if (selectKey.Key == ConsoleKey.Escape)
            {
                DisplayStore();
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

    private static void StoreBuyScene(string typeSelect = "")
    {
        List<Item> diviList = new List<Item>();
        int selectOtion = 0;
        string[] storeOption = { };

        ListClear();
        for (int i = 0; i < itemdata.Count; i++)
        {
            if (typeSelect == itemdata[i].Type.ToString())
            {
                storeOption = itemdata[i].ItemName.Split(',');
                diviList.Add(itemdata[i]);
            }
        }
        while (true)
        {

            // 아이템리스트를 불러오면 방향키로 이동하면서 선택
            for (int i = 0; i < diviList.Count; i++)
            {
                SetCursorPosition(50, 12 + i);
                if (i == selectOtion)
                {
                    ForegroundColor = ConsoleColor.Blue;
                    SetCursorPosition(50, 12 + i);
                    Write($"-  {diviList[i].ItemName}");
                    SetCursorPosition(70, 12 + i);
                    Write($"{diviList[i].Type}");
                    SetCursorPosition(90, 12 + i);
                    Write($"{diviList[i].ItemOption}");
                    SetCursorPosition(110, 12 + i);
                    Write($"{diviList[i].Price}");
                    SetCursorPosition(130, 12 + i);
                    for (int j = 0; j < _player.Inventory.Count; j++)
                    {
                        if (_player.Inventory[j].ItemName == diviList[i].ItemName)
                        {
                            SetCursorPosition(130, 12 + i);
                            Write($"O");
                        }
                        else
                        {
                            SetCursorPosition(130, 12 + i);
                            Write($"X");
                        }
                    }
                    ResetColor();
                    SetCursorPosition(50, 24);
                    Write(new string(' ', 100));
                    SetCursorPosition(50, 24);
                    Write($"{diviList[i].Desc}");
                }
                else
                {
                    SetCursorPosition(50, 12 + i);
                    Write($"-  {diviList[i].ItemName}");
                    SetCursorPosition(70, 12 + i);
                    Write($"{diviList[i].Type}");
                    SetCursorPosition(90, 12 + i);
                    Write($"{diviList[i].ItemOption}");
                    SetCursorPosition(110, 12 + i);
                    Write($"{diviList[i].Price}");
                    for (int j = 0; j < _player.Inventory.Count; j++)
                    {
                        if (_player.Inventory[j].ItemName == diviList[i].ItemName)
                        {
                            SetCursorPosition(130, 12 + i);
                            Write($"O");
                        }
                        else
                        {
                            SetCursorPosition(130, 12 + i);
                            Write($"X");
                        }
                    }
                }
            }
            

            ConsoleKeyInfo selectKey = ReadKey();
            if (selectKey.Key == ConsoleKey.UpArrow)
            {
                selectOtion = (selectOtion == 0) ? storeOption.Length - 1 : selectOtion - 1;
            }
            else if (selectKey.Key == ConsoleKey.DownArrow)
            {
                selectOtion = (selectOtion == storeOption.Length) ? 0 : selectOtion + 1;
            }
            else if (selectKey.Key == ConsoleKey.Escape)
            {
                DisplayStore();
                break;
            }
            else if (selectKey.Key == ConsoleKey.Enter)
            {
                if (_player.Gold >= diviList[selectOtion].Price)
                {
                    _player.Gold -= diviList[selectOtion].Price;
                    _player.Inventory.Add(new(diviList[selectOtion].ItemName, false, true, 1));
                    SetCursorPosition(50, 24);
                    Write(new string(" "), 60);
                    SetCursorPosition(50, 24);
                    WriteLine($"{diviList[selectOtion].ItemName}을(를) 구매하셨습니다.");
                    Gold();
                    Data.SavePlayerData(_player);
                }
                else if (_player.Gold < diviList[selectOtion].Price)
                {
                    SetCursorPosition(50, 26);
                    WriteLine("소지금이 부족합니다.");
                }
                else
                {
                    WriteLine("잘못된 입력입니다.");
                }
                SetCursorPosition(50, 27);
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
                ListClear();
                break;
        }
    }

    private static void StoreSellScene()
    {

        int selectOption = 0;
        string[] storeOption = new string[_player.Inventory.Count];

        for (int i = 0; i < _player.Inventory.Count; i++)
        {
            storeOption[i] = _player.Inventory[i].ItemName;
        }

        while (true)
        {
            if (_player.Inventory.Count > 0)
            {
                for (int i = 0; i < _player.Inventory.Count; i++)
                {

                    if (i == selectOption)
                    {
                        ForegroundColor = ConsoleColor.Blue;
                        SetCursorPosition(50, 12 + i);
                        Write($"-  {_player.Inventory[i].ItemName}");
                        for (int j = 0; j < itemdata.Count; j++)
                        {
                            if (_player.Inventory[i].ItemName == itemdata[j].ItemName)
                            {
                                SetCursorPosition(70, 12 + i);
                                Write($"{itemdata[j].Type}");
                                SetCursorPosition(90, 12 + i);
                                Write($"{itemdata[j].ItemOption}");
                                SetCursorPosition(110, 12 + i);
                                Write($"{itemdata[j].Price}");
                            }
                        }
                        ResetColor();
                        SetCursorPosition(50, 24);
                        Write($"{itemdata[i].Desc}");
                    }
                    else
                    {
                        SetCursorPosition(50, 12 + i);
                        Write($"-  {_player.Inventory[i].ItemName}");
                        for (int j = 0; j < itemdata.Count; j++)
                        {
                            if (_player.Inventory[i].ItemName == itemdata[j].ItemName)
                            {
                                SetCursorPosition(70, 12 + i);
                                Write($"{itemdata[j].Type}");
                                SetCursorPosition(90, 12 + i);
                                Write($"{itemdata[j].ItemOption}");
                                SetCursorPosition(110, 12 + i);
                                Write($"{itemdata[j].Price}");
                            }
                        }
                    }
                }
            }
            else
            {
                SetCursorPosition(50, 12);
                WriteLine("- 소지하고 있는 아이템이 없습니다.");
            }
            WriteLine();

            ConsoleKeyInfo selectKey = ReadKey();
            if (selectKey.Key == ConsoleKey.UpArrow)
            {
                selectOption = (selectOption == 0) ? storeOption.Length - 1 : selectOption - 1;
            }
            else if (selectKey.Key == ConsoleKey.DownArrow)
            {
                selectOption = (selectOption == storeOption.Length - 1) ? 0 : selectOption + 1;
            }
            else if(selectKey.Key == ConsoleKey.Escape)
            {
                EscapeOut();
                break;
            }
            else if (selectKey.Key == ConsoleKey.Enter)
            {
                // 소지금 = 소지금 + 아이템 판매 가격
                // 인벤 - 선택아이템 Remove
                if (_player.Inventory.Count > 0)
                {
                    // 소지금 = 소지금 + 아이템 판매 가격
                    // 인벤 - 선택아이템 Remove
                    for (int j = 0; j < itemdata.Count; j++)
                    {
                        if (_player.Inventory[selectOption].ItemName == itemdata[j].ItemName)
                        {
                            _player.Gold += itemdata[j].Price;
                            Gold();
                        }
                    }
                    _player.Inventory.Remove(_player.Inventory[selectOption]);
                    SetCursorPosition(50, 24);
                    WriteLine("판매가 완료되었습니다.");
                    Data.SavePlayerData(_player);
                }
                else
                {
                    SetCursorPosition(50, 24);
                    WriteLine("판매할 아이템이 없으므로 메인화면으로 넘어갑니다.");
                    ReadKey();
                    ClearRange(12, 19);
                    ClearRange(24, 29);
                    DisplayStore();
                }
                SetCursorPosition(50, 25);
                WriteLine("아무 키나 누르면 상점으로 돌아갑니다.");
                ReadKey();
                ListClear();
                DisplayStore();
                break;
            }
        }
        switch ((StoreSelect)selectOption)
        {
            case StoreSelect.Exit:
                StoreScene();
                ListClear();
                break;
        }
    }

    private static void CreativeUI()
    {
        Clear();
        Renderer.Instance.DrawConsoleBorder();

        SetCursorPosition(30, 6);
        Write("STORE");

        const int splitPosition1 = Renderer.FixedXColumn / 8;
        for (int i = 0; i < Renderer.FixedYRows - 12; i++)
        {
            SetCursorPosition(splitPosition1, i + 3);
            BackgroundColor = ConsoleColor.Yellow;
            Write(" "); // 분할선 그리기
            ResetColor();
        }

        const int splitPosition2 = Renderer.FixedXColumn / 4;
        for (int i = 0; i < Renderer.FixedYRows - 12; i++)
        {
            SetCursorPosition(splitPosition2, i + 3);
            BackgroundColor = ConsoleColor.Yellow;
            Write(" ");
            ResetColor();
        }

        const int splitPosition3 = Renderer.FixedXColumn - 28;
        for (int i = 0; i < Renderer.FixedYRows - 12; i++)
        {
            SetCursorPosition(splitPosition3, i + 3);
            BackgroundColor = ConsoleColor.Yellow;
            Write(" ");
            ResetColor();
        }

        SetCursorPosition(22, 3);
        for (int i = 0; i < 130; i++)
        {
            BackgroundColor = ConsoleColor.Yellow;
            Write(" ");
            ResetColor();
        }

        SetCursorPosition(22, 9);
        for (int i = 0; i < 130; i++)
        {
            BackgroundColor = ConsoleColor.Yellow;
            Write(" ");
            ResetColor();
        }

        SetCursorPosition(22, 21);
        for (int i = 0; i < 130; i++)
        {
            BackgroundColor = ConsoleColor.Yellow;
            Write(" ");
            ResetColor();
        }

        SetCursorPosition(22, 30);
        for (int i = 0; i < 130; i++)
        {
            BackgroundColor = ConsoleColor.Yellow;
            Write(" ");
            ResetColor();
        }

       
        #region
        // UI하는 게 어려워서 그냥 노가다로 하겠습니다.

        ///// <summary>
        ///// 첫번째 칸 TITLE
        ///// <summary>
        //int row = 5;
        //int width = 150;
        //SetCursorPosition(10, 2);
        //for (int i = 0; i <= width; i++)
        //{
        //    if (i == 0) Write("┌");
        //    Write("─");
        //    if (i == width) Write("┐");
        //}
        //for (int j = 1; j < row; j++)
        //{
        //    SetCursorPosition(10, j);
        //    for (int i = 0; i <= width; i++)
        //    {
        //        if (i == 0) Write("│");
        //        Write(" ");
        //        if (i == width) Write("│");
        //    }
        //}

        //SetCursorPosition(10, 5);
        //for (int i = 0; i <= width; i++)
        //{
        //    if (i == 0) Write("├");
        //    Write("─");
        //    if (i == width) Write("┤");
        //}

        //WriteLine();
        //SetCursorPosition(67, 3);
        //Write("Store - 아이템을 사고 파는 상점입니다.");

        ///// <summary>
        ///// 두번째 칸 TITLE
        ///// <summary>
        //for (int j = 6; j < row * 2; j++)
        //{
        //    SetCursorPosition(10, j);
        //    for (int i = 0; i <= width; i++)
        //    {
        //        if (i == 0) Write("│");
        //        Write(" ");
        //        if (i == width) Write("│");
        //    }
        //}

        //SetCursorPosition(10, 10);
        //for (int i = 0; i <= width; i++)
        //{
        //    if (i == 0) Write("├");
        //    Write("─");
        //    if (i == width) Write("┤");
        //}

        ///// <summary>
        ///// 세번째 칸 TITLE
        ///// <summary>

        //for (int j = 11; j < row * 4; j++)
        //{
        //    SetCursorPosition(10, j);
        //    for (int i = 0; i <= width; i++)
        //    {
        //        if (i == 0) Write("│");
        //        Write(" ");
        //        if (i == width) Write("│");
        //    }
        //}

        //SetCursorPosition(10, 20);
        //for (int i = 0; i <= width; i++)
        //{
        //    if (i == 0) Write("├");
        //    Write("─");
        //    if (i == width) Write("┤");
        //}

        ///// <summary>
        ///// 네번째 칸 TITLE
        ///// <summary>

        //for (int j = 21; j < row * 6; j++)
        //{
        //    SetCursorPosition(10, j);
        //    for (int i = 0; i <= width; i++)
        //    {
        //        if (i == 0) Write("│");
        //        Write(" ");
        //        if (i == width) Write("│");
        //    }
        //}

        //SetCursorPosition(10, 30);
        //for (int i = 0; i <= width; i++)
        //{
        //    if (i == 0) Write("└");
        //    Write("─");
        //    if (i == width) Write("┘");
        //}
        //WriteLine();
        //ResetColor();
    }
    #endregion

    private static bool _isEscape = false;
    public static void EscapeOut()
    {
        _isEscape = true;
        DisplayStore();
    }
    public static void ListClear()
    {
        ClearRange(12, 19);
        ClearRange(24, 29);
    }
    public static void diviClear()
    {
        for (int i = 12; i < 19; i++)
        {
            SetCursorPosition(30, i);
            Write(new string(' ', 12));
        }
    }
    public static void Gold()
    {
        // 골드 표시
        SetCursorPosition(30, 26);
        Write(new string(' ', 10));
        SetCursorPosition(30, 26);
        Write($"{_player.Gold} G");
    }

    // 정훈님 코드 참고
    // y기준 - start -> end 꺄지 범위를 설정해주면 ' '로 덮어주는 듯
    public static void ClearRange(int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            SetCursorPosition(50, i);
            Write(new string(' ', 100));
        }
    }
}

enum StartStore
{
    Buy,
    Sell,
    Exit
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