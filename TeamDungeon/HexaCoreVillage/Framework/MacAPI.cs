
using System.Runtime.InteropServices;

namespace HexaCoreVillage.Framework;

public class MacAPI
{
    #region Member Variables

    #endregion




    #region Extern Function
    [DllImport("libc")] private static extern int system(string exec);
    #endregion




    #region User Function
    public void ProgressConsoleSetting()
    {
        // Adjust the window size for Unix-like systems
        system($@"printf '\e[8;{Renderer.FixedYRows};{Renderer.FixedXColumn}t'");
    }
    #endregion
}
