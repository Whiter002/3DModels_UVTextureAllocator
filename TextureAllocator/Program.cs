using TextureAllocator.Core;

namespace TextureAllocator;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        bool checkUpdateFlag = (args?.Length??0) < 1 || args[0].ToLower() == "true";
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(checkUpdateFlag));
    }
}