
using HexaCoreVillage.Utility;

namespace HexaCoreVillage.Lobby;

public class Lobby : Scene
{
    #region Member Variables
    public override SCENE_NAME SceneName => SCENE_NAME.LOBBY;

    public enum LobbySelectMenu
    {
        Status,
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

    /* Sound Resource */
    private string _newWorldBGM = Managers.Resource.GetSoundResource(ResourceKeys.newWorldBGM);

    #endregion



    #region Flow Methods
    public override void Start()
    {
        // Cursor Setting
        CursorVisible = false;

        // Draw
        Renderer.Instance.DrawConsoleBorder();
        this.DrawMenuBox();
        this.DrawandPrintContextBox();

        // Init Title Text
        InitAsciiTitleText();

        // Init Variables
        InitalizeVariables();

        // Init Thread
        InitThread();

        // BGM start
        AudioPlayer.AudioController(_newWorldBGM, AudioPlayer.PlayOption.LoopStart);

        // LoadToResourcePlayerData - 새로 플레이어 생성됐을 시 리소스 매니저가 지니고 있는 리소스 갱신
        Managers.Resource.LoadResourcePlayerData();
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

    private void InitThread()
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
            Managers.UI.ClearRowsColSelect(StartPosY, 32, 60, 116);

        this.InputSelectMenuItem();
    }

    private void PrintMenuItems()
    {
        string[] menuItemTexts =
        {
            "개발자 스펙/기기 - (스탯/인벤)",
            "헥사 IT 스토어 - (상점)",
            "디버깅 미궁 - (던전)",
            "월차 내러 가기 - (휴식)",
            "처음으로 - (타이틀 화면)",
            "종료"
        };

        lock(_lock)
        {
            Managers.UI.PrintMsgAlignCenter("< H E X A   M E N U >", StartPosY - 3, ConsoleColor.Blue);
            for(int idx = 0; idx < menuItemTexts.Length; ++idx)
            {
                ConsoleColor color = ConsoleColor.Gray;
                if(idx == ((int)_menuSelector))
                {
                    string newItemText = new string(">         " + menuItemTexts[idx] + "         <");
                    menuItemTexts[idx] = newItemText;
                    color = ConsoleColor.Blue;
                }

                Managers.UI.PrintMsgAlignCenter(menuItemTexts[idx], (StartPosY+2) + (idx * 3), color);
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
                Stop();
                Thread.Sleep(1000);
                Environment.Exit(0);
                break;
        }
    }

    private void DrawMenuBox()
    {
        int startPosX, startPosY;

        startPosX = Renderer.FixedXColumn / 3;
        startPosY = StartPosY - 2;
        Managers.UI.DrawBox(startPosX, startPosY, 60, 22, ConsoleColor.Blue);
    }

    /// <summary>
    /// # 사용 설명을 출력
    /// </summary>
    private void DrawandPrintContextBox()
    {
        int startPosX = Renderer.FixedXColumn / 6;
        int startPosY = Renderer.FixedYRows - 4;
        int width = Renderer.FixedXColumn - (startPosX * 2);
        COORD[] columnPos = Managers.UI.DrawColumnBox(startPosX, startPosY, width, 2, 2, ConsoleColor.Yellow);

        Managers.UI.PrintMsgCoordbyColor("[사용방법]  [이동 - ↑↓ | ⓦⓢ]  [선택 - Space | Enter]", columnPos[0].X, columnPos[0].Y, ConsoleColor.Yellow);
        Managers.UI.PrintMsgCoordbyColor("[Copyright] ⓒ HexaCore Dungeon. All right reseved.", columnPos[1].X, columnPos[1].Y, ConsoleColor.Yellow);
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