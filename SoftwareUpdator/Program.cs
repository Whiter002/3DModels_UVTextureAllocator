using CommandLine;
using SoftwareUpdator;
using System.IO.Compression;
using System.Reflection;

var pr = Parser.Default.ParseArguments<Argument>(args);
if (pr.Errors.Count() > 0) throw new ArgumentException(pr.Errors.ElementAt(0).ToString());
if(pr.Tag != ParserResultType.NotParsed) throw new ArgumentException("Invalid arguments");

var parsedArgs = ((Parsed<Argument>)pr).Value;

var getCommonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "Dr_Updator";
var compressedFileName = Path.GetFileNameWithoutExtension(parsedArgs.UpdateZipSource);

var tmpFileDirectory = Path.Combine(getCommonAppDataPath, assemblyName, compressedFileName);

if(Directory.Exists(tmpFileDirectory)) Directory.Delete(tmpFileDirectory, true);
Directory.CreateDirectory(tmpFileDirectory);

try
{
    ZipFile.ExtractToDirectory(parsedArgs.UpdateZipSource, tmpFileDirectory);
    CopyDirectorysOnlyDifferentSHA256(tmpFileDirectory, parsedArgs.UpdateExtractDestination, parsedArgs.Mirror);
    Directory.Delete(tmpFileDirectory, true);
    if(!string.IsNullOrEmpty(parsedArgs.PowerShellCommand))
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

void CopyDirectorysOnlyDifferentSHA256(string fromdir,string todir,bool? mirror)
{
    var srcFiles = Directory.GetFiles(fromdir);
    if (mirror ?? false)
    {
        var toFiles = Directory.GetFiles(todir).Where(file => !srcFiles.Contains(file)).ToArray();
        foreach(var file in toFiles)
        {
            if (File.Exists(file)) File.Delete(file);
        }
    }
    
    foreach (var file in srcFiles)
    {
        var fileName = Path.GetFileName(file);
        var fromHash = GetFileHash(file);
        var toPath = Path.Combine(todir, fileName);
        var toHash = GetFileHash(toPath);
        if (String.Equals(fromHash, toHash)) continue;
        File.Copy(file, toPath, true);
    }
    foreach(var directory in Directory.GetDirectories(fromdir))
    {
        var dirName = Path.GetFileName(directory);
        var toPath = Path.Combine(todir, dirName);
        if (!Directory.Exists(toPath)) Directory.CreateDirectory(toPath);
        CopyDirectorysOnlyDifferentSHA256(directory, toPath,mirror);
    }
}
string? GetFileHash(string path)
{
    if(!File.Exists(path)) return null;
    using var sha256 = System.Security.Cryptography.SHA256.Create();
    var hashBytes = sha256.ComputeHash(new FileStream(path,FileMode.Open));
    return BitConverter.ToString(hashBytes);
}