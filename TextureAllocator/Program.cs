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

        if (args.Length < 1 || args[0] != "true")
        {//Run Update Check

            var result = UpdateChecker.AutoCheckForUpdate().Result;
            if(result.IsAvailable && !String.IsNullOrEmpty(result.DownloadUrl))
            {
                MessageBox.Show("アップデートがあります。");
            }
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}