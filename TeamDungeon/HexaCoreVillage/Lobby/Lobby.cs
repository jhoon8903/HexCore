
using HexaCoreVillage.Utility;

namespace HexaCoreVillage.Lobby;

public class Lobby : Scene
{
    #region Member Variables
    public override SCENE_NAME SceneName => SCENE_NAME.LOBBY;

    public enum LobbySelectMenu
    {
        Status,
        Inventory,
        Store,
        Dungeon,
        Rest,
        GoTitle,
        Exit
    }

    /* Thread */
    private const int DrawTitleInterval = 500;
    private Thread _titleUpdateThread;
    private bool _isRunning = true;
    private readonly object _lock = new object();

    /* Title Text */
    private ConsoleColor[] _colorArray;
    private int _colorIndex;
    private string[] _titleTextSplit;
    private const int StartPosY = 14;

    /* Lobby Selector */
    private LobbySelectMenu _menuSelector = LobbySelectMenu.Status;

    #endregion



    #region Flow Methods
    public override void Start()
    {
        // Cursor Setting
        CursorVisible = false;

        // Border Draw
        Renderer.Instance.DrawConsoleBorder();

        // Init Title Text
        InitAsciiTitleText();

        // Init Variables
        InitalizeVariables();

        // Init Timer
        InitTimer();

        //BGM start
        AudioPlayer.AudioController(Managers.Resource.GetSoundResource(ResourceKeys.newWorldBGM), AudioPlayer.PlayOption.LoopStart);
    }

    public override void Update()
    {
        this.DrawUpdate();
    }

    public override void Stop()
    {
        _isRunning = false;
        _titleUpdateThread.Join();
    }
    #endregion



    #region Initalize
    private void InitAsciiTitleText()
    {
        string asciiArtResource = Managers.Resource.GetTextResource(ResourceKeys.TextLobbyAscii);
        _titleTextSplit = asciiArtResource.Split(new[] { "\n" }, StringSplitOptions.None);
    }

    private void InitalizeVariables()
    {
        _colorIndex = 0;

        _colorArray = new ConsoleColor[]
        {
            ConsoleColor.Red,
            ConsoleColor.Yellow,
            ConsoleColor.Green,
            ConsoleColor.Cyan,
            ConsoleColor.Magenta,
            ConsoleColor.Yellow
        };
    }

    private void InitTimer()
    {
        _titleUpdateThread = new Thread(TitleUpdateLoop);
        _titleUpdateThread.Start();
    }
    #endregion



    #region Logic Methods
    private void DrawUpdate()
    {
        while (!KeyAvailable)
            this.PrintMenuItems();
        if (KeyAvailable)
            Managers.UI.ClearRows(StartPosY, Managers.UI.EndPosY);

        this.InputSelectMenuItem();
    }

    private void PrintMenuItems()
    {
        string[] menuItemTexts =
        {
            "개발자 스펙 - (스테이터스)",
            "개발자 기기 현황 - (인벤토리)",
            "헥사 IT 스토어 - (상점)",
            "디버깅 미궁 - (던전)",
            "월차 내러 가기 - (휴식)",
            "처음으로 - (타이틀 화면)",
            "종료"
        };

        lock(_lock)
        {
            for(int idx = 0; idx < menuItemTexts.Length; ++idx)
            {
                ConsoleColor color = ConsoleColor.Gray;
                if(idx == ((int)_menuSelector))
                {
                    string newItemText = new string(">         " + menuItemTexts[idx] + "         <");
                    menuItemTexts[idx] = newItemText;
                    color = ConsoleColor.Blue;
                }

                Managers.UI.PrintMsgAlignCenter(menuItemTexts[idx], StartPosY + (idx * 3), color);
            }
        }
    }

    private void InputSelectMenuItem()
    {
        var keyInfo = ReadKey(true);

        switch(keyInfo.Key)
        {
            case ConsoleKey.W:
            case ConsoleKey.UpArrow:
                _menuSelector = (_menuSelector - 1 < 0) ?
                    (LobbySelectMenu)(Enum.GetValues(typeof(LobbySelectMenu)).Length - 1) :
                    _menuSelector - 1;
                break;
            case ConsoleKey.S:
            case ConsoleKey.DownArrow:
                _menuSelector = (_menuSelector + 1 >= (LobbySelectMenu)Enum.GetValues(typeof(LobbySelectMenu)).Length) ?
                    LobbySelectMenu.Status :
                    _menuSelector + 1;
                break;
            case ConsoleKey.Spacebar:
            case ConsoleKey.Enter:
                this.EnterSpacebarProcess();
                break;
        }
    }

    private void EnterSpacebarProcess()
    {
        switch(_menuSelector)
        {
            case LobbySelectMenu.Status:
                Managers.Scene.LoadScene(SCENE_NAME.STATUS);
                break;
            case LobbySelectMenu.Inventory:
                //Managers.Scene.LoadScene(SCENE_NAME.INVEN);
                break;
            case LobbySelectMenu.Store:
                Managers.Scene.LoadScene(SCENE_NAME.STORE);
                break;
            case LobbySelectMenu.Dungeon:
                Managers.Scene.LoadScene(SCENE_NAME.BATTLE);
                break;
            case LobbySelectMenu.Rest:
                break;
            case LobbySelectMenu.GoTitle:
                Managers.Scene.LoadScene(SCENE_NAME.TITLE);
                break;
            case LobbySelectMenu.Exit:
                Clear();
                Thread.Sleep(1000);
                Environment.Exit(0);
                break;
        }
    }

    private void DrawMenuBox()
    {

    }
    #endregion



    #region Thread Methods
    private void TitleUpdateLoop()
    {
        while(_isRunning)
        {
            DrawLobbyTitleTextbyThread();
            Thread.Sleep(DrawTitleInterval);
        }
    }

    private void DrawLobbyTitleTextbyThread()
    {
        lock (_lock)
        {
            int startPosY = Managers.UI.StartPosY + 1;

            Managers.UI.ClearRows(1, 10);

            for (int row = 0; row < _titleTextSplit.Length; ++row)
            {
                ConsoleColor currentColor = _colorArray[_colorIndex % _colorArray.Length];
                Managers.UI.PrintMsgAlignCenter(_titleTextSplit[row], row + startPosY, currentColor);

                if (row % 2 == 0 && row != 0)
                    ++_colorIndex;
            }
        }
    }
    #endregion
}