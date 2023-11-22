using HexaCoreVillage.Utility;
using HexaCoreVillage.Framework;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using Newtonsoft.Json;

namespace HexaCoreVillage.Lobby;

public class Status : Scene
{
    public override SCENE_NAME SceneName => SCENE_NAME.STATUS;
    private Player _player = Login.Login._player;
    private List<Item> itemList = new List<Item>();
    bool isInventoryScene = false;
    Item itemInform = new Item();
 
    public override void Start()
    {
        Console.Clear();
        Console.CursorVisible = false;
        SetItem();
    }

    public override void Update()
    {
        StatusScene();
    }
    //게임 내 사용할 아이템 호출
    private void SetItem()
    {
        string Json = Managers.Resource.GetTextResource(ResourceKeys.ItemList);
        itemList = JsonConvert.DeserializeObject<List<Item>>(Json); //게임 내 사용할 수 있는 모든 ITEM
    }


    private void InventoryScene()
    {
        int itemStart_x = 8;    //아이템 시작 x 좌표
        int itemStart_y = 24;   //아이템 시작 y 좌표
        int idx = 1;
        int selectedOption = 0;

        if(_player.Inventory.Count==0)//만약 아이템이 없다면 그냥 status메뉴로 바로 이동
        {
            isInventoryScene = false;
        }

        while (true)
        {
            itemStart_x = 8;
            itemStart_y = 24;
            SetCursorPosition(itemStart_x, itemStart_y);
            for (int i = 0; i < _player.Inventory.Count; i++)
            {
                foreach(var item in _player.Inventory)
                {
                    if (_player.Inventory[i].ItemName == item.ItemName)
                    {
                        itemInform = GetItemInform(item.ItemName);
                    }
                }

                if (i == 4)
                {
                    //아이템 목록이 4개가 아이템 오른쪽 창으로 넘기기
                    itemStart_x = 99;
                    itemStart_y = 24;
                }

                if (i == selectedOption)
                {
                    ForegroundColor = ConsoleColor.Green;
                    SetCursorPosition(itemStart_x, itemStart_y + 3 * (i % 4));
                    Write($"-> {_player.Inventory[i].ItemName} : {GetItemStatsType(itemInform.Type)} +{itemInform.ItemOption} - {itemInform.Desc}");
                    ResetColor();
                }
                else
                {
                    if (_player.Inventory[i].IsEquipment==true)
                        ForegroundColor= ConsoleColor.Green;
                    SetCursorPosition(itemStart_x, itemStart_y + 3 * (i % 4));
                    Write($"-> {_player.Inventory[i].ItemName} : {GetItemStatsType(itemInform.Type)} +{itemInform.ItemOption} - {itemInform.Desc}");
                    ResetColor();
                }
            }
            ConsoleKeyInfo keyInfo = ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedOption = (selectedOption == 0) ? _player.Inventory.Count - 1 : selectedOption - 1;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedOption = (selectedOption == _player.Inventory.Count - 1) ? 0 : selectedOption + 1;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        //아이템 장착,해제 기능구현
        if (_player.Inventory[selectedOption].IsEquipment==false)    //아이템이 장착되어 있지 않다면
        {
            foreach (var item in itemList)  //아이템 리스트에서 플레이어가 가지고 있는 아이템 정보 찾기
            {
                if (item.ItemName == _player.Inventory[selectedOption].ItemName) 
                {
                    
                    switch (GetItemStatsType(item.Type))
                    {
                        case "DEF":
                            _player.BonusDef += item.ItemOption;
                            break;
                        case "ATK":
                            _player.BonusDmg += item.ItemOption;
                            break;
                        case "HP":
                            _player.CurrentHp += item.ItemOption;
                            break;
                        case "SP":
                            if (_player.Inventory[selectedOption].Quantity > 0)
                            {
                                _player.CurrentMental += item.ItemOption;
                                _player.Inventory[selectedOption].Quantity--;
                            }
                            break;
                    }
                    _player.Inventory[selectedOption].IsEquipment = true;
                    Utility.Data.SavePlayerData(_player);
                }

            }
        }
        else        //아이템이 장착되어 있다면
        {
            foreach (var item in itemList)  //아이템 리스트에서 플레이어가 가지고 있는 아이템 정보 찾기
            {
                if (item.ItemName == _player.Inventory[selectedOption].ItemName)
                {

                    switch (GetItemStatsType(item.Type))
                    {
                        case "DEF":
                            _player.BonusDef -= item.ItemOption;
                            break;
                        case "ATK":
                            _player.BonusDmg -= item.ItemOption;
                            break;
                        case "HP":
                            _player.CurrentHp -= item.ItemOption;
                            break;
                        case "SP":
                            _player.CurrentMental -= item.ItemOption;
                            break;
                    }
                    _player.Inventory[selectedOption].IsEquipment = false;
                    Utility.Data.SavePlayerData(_player);
                }

            }
        }
        isInventoryScene = false;
    }

    private void StatusScene()
    {
        int selectedOption = 0;
        string[] options = { "INVENTORY", "LOBBY" };
        string[] plusOpt = { "아이템 장착 관리를 위한 인벤토리", "로비로 돌아갑니다." };

        PrintPlayer();
        DrawLine();
        if (isInventoryScene)
            InventoryScene();
        PrintStatusMenu();

        while (true)
        {
            SetCursorPosition(0, 35);
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedOption)
                {
                    ForegroundColor = ConsoleColor.Blue;

                    SetCursorPosition(CursorLeft + 1, CursorTop);
                    Write($"-> {options[i]}");
                    SetCursorPosition(CursorLeft + 1, CursorTop);
                    WriteLine("   - " + plusOpt[i]);
                    ResetColor();
                }
                else
                {
                    SetCursorPosition(CursorLeft + 1, CursorTop);
                    WriteLine($"   {options[i]}                                      ");
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

        switch ((SelectOption)selectedOption)
        {
            case SelectOption.Inventory:
                isInventoryScene = true;
                break;
            case SelectOption.Lobby:
                Managers.Scene.LoadScene(SCENE_NAME.LOBBY);
                break;
        }
    }

    private void DrawLine()
    {
        Console.SetCursorPosition(1, 20);
        Console.BackgroundColor = ConsoleColor.Yellow;
        for (int i = 0; i < Renderer.FixedXColumn - 2; i++)
            Console.Write(" ");

        for (int i = 0; i < Renderer.FixedYRows - 21; i++)
        {
            Console.SetCursorPosition(90, 20 + i);
            Console.WriteLine(" ");
        }

        Console.ResetColor();
    }
    //플레이어 모습 아스키 코드 그림
    private void PrintPlayer()
    {
        string[] asciPlayer =
        {
        "      @!;-  !!      \r\n",
        "     @       -:     \r\n",
        "   . #       .@    .\r\n",
        "   . @       .@    .\r\n",
        "   . @       .@     \r\n",
        ". .   @   .  @  .   \r\n",
        "      #,    ,#      \r\n",
        "       @    @       \r\n",
        "     ;@!=@@=!@;     \r\n",
        "   .@~#  @@  @,@,   \r\n",
        ". -@  .- @- *   @-  \r\n",
        "  $.   !;@@;~.   $  \r\n",
        " $      *@@*      # \r\n",

        };

        int startPoint_x = 3;
        int startPoint_y = 2;
        int height = 18;
        int width = 30;
        //캐릭터 위 테두리 그리는 로직
        BackgroundColor = ConsoleColor.White;
        SetCursorPosition(startPoint_x, startPoint_y);
        Write(new string(' ', width));

        //캐릭터 양 옆의 테두리를 그리는 로직
        for (int i = startPoint_x; i < height; i++)
        {
            SetCursorPosition(startPoint_x, i);
            Write(' ');
            SetCursorPosition(startPoint_x + width - 1, i);
            Write(' ');
        }

        //캐릭터 아래 테두리 그리는 로직
        SetCursorPosition(startPoint_x, height);
        Write(new string(' ', width));
        Console.BackgroundColor = ConsoleColor.Black;

        //캐릭터 그리는 로직
        for (int i = 0; i < asciPlayer.Length; i++)
        {
            SetCursorPosition(startPoint_x + 4, startPoint_y + 2 + i);
            WriteLine(asciPlayer[i]);
        }

    }
    //플레이어 상태창 출력 함수
    private void PrintStatusMenu()
    {
        int itemStart_x = 8;    //아이템 시작 x 좌표
        int itemStart_y = 24;   //아이템 시작 y 좌표

        SetCursorPosition(42, 5);
        Write("LV");
        SetCursorPosition(42, 7);
        Write("이름\n");
        SetCursorPosition(42, 9);
        Write("ID\n");
        SetCursorPosition(42, 11);
        Write("직업\n");
        SetCursorPosition(42, 13);
        Write("골드");

        BackgroundColor = ConsoleColor.DarkGray;
        SetCursorPosition(47, 5);
        Write($"{PadCenterForMixedText($"{_player.Level.ToString("00")}", 30)}");
        SetCursorPosition(47, 7);
        Write($"{PadCenterForMixedText($"{_player.NickName}", 30)}");
        SetCursorPosition(47, 9);
        Write($"{PadCenterForMixedText($"{_player.ID}", 30)}");
        SetCursorPosition(47, 11);
        Write($"{PadCenterForMixedText($"{_player.Job}", 30)}");
        SetCursorPosition(47, 13);
        Write($"{PadCenterForMixedText($"{_player.Gold}G", 30)}");
        ResetColor();

        SetCursorPosition(87, 5);
        Write($"{PadRightForMixedText($"경험치  {_player.Exp}/100", 30)}");  ///100 => 해당 레벨의 최대값
        PrintBar(100, _player.Exp);

        SetCursorPosition(87, 7);
        Write($"{PadRightForMixedText($"체력  {_player.CurrentHp}/{_player.HP}  ", 30)}");
        PrintBar(_player.HP, _player.CurrentHp);

        SetCursorPosition(87, 9);
        Write($"{PadRightForMixedText($"멘탈  {_player.CurrentMental}/{_player.CurrentMental}  ", 30)}");
        PrintBar(_player.Mental, _player.CurrentMental);

        SetCursorPosition(133, 11);
        Write("♣기본정보♣");    

        SetCursorPosition(87, 13);
        Write($"{PadRightForMixedText($"타이핑 속력(ATK)  {_player.TypingSpeed+_player.BonusDmg}/100  ", 30)}");
        PrintBar(100, _player.TypingSpeed+_player.BonusDmg);

        SetCursorPosition(87, 15);
        Write($"{PadRightForMixedText($"C# 언어 능력(DEF)  {_player.C+_player.BonusDef}/100  ", 30)}");
        PrintBar(100, _player.C+_player.BonusDef);


        //여기서부터 아이템
        SetCursorPosition(82, 22);
        ForegroundColor = ConsoleColor.DarkCyan;
        //Write("Equipment");
        ResetColor();

        foreach (var item in _player.Inventory)
        {
            //여기다가 아이템 별로 출력하면 되고 만약 y값이 정해진 label 보다 크면 안 x좌표 처음부터 y좌표 처음부터
            if (itemStart_y > 33)
            {
                itemStart_x = 99;
                itemStart_y = 24;
            }
            SetCursorPosition(itemStart_x, itemStart_y);
            itemInform = GetItemInform(item.ItemName);

            if (item.IsEquipment == true)
                 Console.ForegroundColor = ConsoleColor.Green;

            Write($"◆ {itemInform.ItemName} : {GetItemStatsType(itemInform.Type)} +{itemInform.ItemOption} - {itemInform.Desc}");
            ResetColor();
            itemStart_y += 3;   //한번 출력할 때마다 y좌표 3내려서 출력
        }
    }
    //아이템 리스트 중에 있는 아이템 정보 가져오는 함수
    private Item GetItemInform(string itemName)
    {
        foreach (Item item in itemList)
        {
            if (itemName == item.ItemName)
                return item;
        }
        return null;
    }
    //아이템 스탯 타입 가져오는 함수
    private string GetItemStatsType(ItemType type)
    {
        string result = "";
        if (type == ItemType.Keyboard || type == ItemType.Monitor)
            result = "DEF";
        else if (type == ItemType.Mouse || type == ItemType.Notebook)
            result = "ATK";
        else if (type == ItemType.HeadSet)
            result = "HP";
        else
            result = "SP";
        return result;
    }
    
    //문자열을 가운데 정렬 해주는 함수
    public static string PadCenterForMixedText(string str, int totalLength)
    {
        string resultStr = "";
        string paddingStr = "";
        int currentLength = GetPrintableLegnth(str);
        int padding = (str.Length % 2 == 0)  ? (totalLength - currentLength) / 2 : currentLength==str.Length ?  (totalLength - currentLength) / 2 + 1 : (totalLength - currentLength) / 2;
        paddingStr = new string(' ', padding);
        resultStr += currentLength != str.Length ? String.Format("{0}", str).PadLeft(totalLength - ((currentLength - str.Length) + (totalLength / 2) - (currentLength / 2))) : String.Format("{0}", str).PadLeft(totalLength - ((totalLength / 2) - (currentLength / 2)));
        resultStr += paddingStr;
        return resultStr;
    }

    //문자열을 오른쪽 정렬 해주는 함수
    public static string PadRightForMixedText(string str, int totalLength)
    {
        int currentLength = GetPrintableLegnth(str);
        int padding = totalLength - currentLength;
        return str.PadRight(str.Length + padding);
    }

    //특수문자인지 구분해주는 함수
    public static int GetPrintableLegnth(string str)
    {
        int length = 0;
        foreach (char c in str)
        {
            if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                length += 2;
            else
                length += 1;
        }
        return length;
    }
    //바 출력하는 함수
    private void PrintBar(int maxValue, int currentValue)
    {
        int maxBarLength = 50;
        float percent = (float)currentValue / maxValue;
        int barLength = (int)(percent * maxBarLength);

        Console.BackgroundColor = ConsoleColor.DarkYellow;
        string bar = new string(' ', barLength);
        Console.Write(bar);
        Console.BackgroundColor = ConsoleColor.Gray;
        bar = new string(' ', maxBarLength - barLength);
        Console.WriteLine(bar);
        Console.ResetColor();
    }

    public override void Stop()
    {
        Clear();
    }
}
enum SelectOption
{
    Inventory,
    Lobby
}


