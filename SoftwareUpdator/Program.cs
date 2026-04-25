using CommandLine;
using SoftwareUpdator;
using System.IO.Compression;
using System.Reflection;

var pr = Parser.Default.ParseArguments<Argument>(args);
if (pr.Errors.Count() > 0) Environment.Exit(0);
if (pr.Tag != ParserResultType.Parsed) Environment.Exit(0);

var parsedArgs = ((Parsed<Argument>)pr).Value;
var TargetSourceDefFile = parsedArgs.TargetSourceDefFile;
if (!File.Exists(TargetSourceDefFile))
{
    Console.WriteLine("Error: Target source definition file does not exist.");
    Environment.Exit(-1);
}
var compressedFileName = Path.GetFileNameWithoutExtension(parsedArgs.UpdateZipSource);
var getCommonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "Dr_Updator";
var tmpFileDirectory = Path.Combine(getCommonAppDataPath, assemblyName, compressedFileName);
if (!Path.GetFullPath(tmpFileDirectory).Contains("SoftwareUpdator"))
{
    // This is a safety check to prevent accidental deletion of important directories. The temporary file directory must be within a path that contains "SoftwareUpdator".
    Console.WriteLine("Error: Invalid temporary file directory.");
    Environment.Exit(-1);
}
if (Directory.Exists(tmpFileDirectory)) Directory.Delete(tmpFileDirectory, true);
Directory.CreateDirectory(tmpFileDirectory);

var updateTargetFileNames = File.ReadAllLines(TargetSourceDefFile).Select(file => Path.GetFileName(file) ?? "").Where(Name => !String.IsNullOrEmpty(Name)).ToArray() ?? [];

try
{
    ZipFile.ExtractToDirectory(parsedArgs.UpdateZipSource, tmpFileDirectory);
    ResultCounter counter = new();
    CopyDirectorysOnlyDifferentSHA256(tmpFileDirectory, parsedArgs.UpdateExtractDestination, updateTargetFileNames, parsedArgs.Mirror,ref counter);
    Console.WriteLine($"Update Successed.");
    Console.WriteLine($"total:{counter.Total()} skipped: {counter.SkippedCount}, updated: {counter.SuccessedCount}");

    bool is_delete = false;
    int tryCount = 0;
    while (tryCount <= 3)
    {
        try
        {
            if(Directory.GetFiles(tmpFileDirectory,"*.*",SearchOption.AllDirectories).Length > 0)
            {
                Console.WriteLine("更新用一時フォルダの削除に失敗しました。(フォルダの中身がからではありません)");
                break;
            }
            Directory.Delete(tmpFileDirectory,true);
            is_delete = true;
            break;

        }
        catch
        {
            tryCount++;
            Console.WriteLine($"更新用一時フォルダの削除に失敗しました。リトライ回数: {tryCount}");
            System.Threading.Thread.Sleep(tryCount * 1000);
        }
    }
    if (!string.IsNullOrEmpty(parsedArgs.PowerShellCommand))
    {
        var psi = new System.Diagnostics.ProcessStartInfo("powershell", parsedArgs.PowerShellCommand)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var process = System.Diagnostics.Process.Start(psi);
    }
    Environment.Exit(0);
}
catch(Exception ex)
{
    Console.WriteLine($"Error during update process: {ex.Message}");
    return;
}

void CopyDirectorysOnlyDifferentSHA256(string fromdir, string todir, string[] targetFileNames, bool? mirror, ref ResultCounter counter)
{
    var srcFiles = Directory.GetFiles(tmpFileDirectory, "*.*", SearchOption.AllDirectories)
        .Select(item => Path.GetRelativePath(fromdir,item))
        .ToArray();

    if (mirror ?? false)
    {
        var toFiles = Directory.GetFiles(todir)
            .Select(file => Path.GetRelativePath(todir,file))
            .Where(file => targetFileNames.Contains(Path.GetFileName(file)))
            .Where(file => !srcFiles.Contains(file)).ToArray();
        foreach(var file in toFiles)
        {
            if (File.Exists(file)) File.Delete(file);
        }
    }
    
    foreach (var file in srcFiles)
    {
        var absoluteFromPath = Path.GetFullPath(file, fromdir);
        var fileName = Path.GetFileName(absoluteFromPath);
        var fromHash = GetFileHash(absoluteFromPath);
        var absoluteToPath = Path.GetFullPath(file,todir);
        var toHash = GetFileHash(absoluteToPath);
        if (String.Equals(fromHash, toHash))
        {
            Console.WriteLine($"{Path.Combine(".",file)} => Skipped (SHA256 match)");
            counter.Skip();
            File.Delete(absoluteFromPath);
            continue;
        }
        string saveDir = Path.GetDirectoryName(absoluteToPath);
        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
        counter.Sccess();
        Console.WriteLine($"{Path.Combine(".", file)} => Copied");
        File.Move(absoluteFromPath, absoluteToPath, true);
    }
}
string? GetFileHash(string path)
{
    if(!File.Exists(path)) return null;
    using var sha256 = System.Security.Cryptography.SHA256.Create();
    using var fs = new FileStream(path, FileMode.Open);
    var hashBytes = sha256.ComputeHash(fs);
    return BitConverter.ToString(hashBytes);
}