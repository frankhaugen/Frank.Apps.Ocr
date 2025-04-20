using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Frank.Apps.Ocr.Api.Poppler;

public static class PopplerInstaller
{
    private const string WindowsPopplerVersion = "24.08.0-0";
    private const string WindowsPopplerZipFileName = $"Release-{WindowsPopplerVersion}.zip";
    private const string WindowsPopplerDownloadUrl = $"https://github.com/oschwartz10612/poppler-windows/releases/download/v{WindowsPopplerVersion}/{WindowsPopplerZipFileName}";

    public static async Task EnsurePopplerInstalledAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            await InstallOnWindowsAsync();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            InstallOnLinux();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            InstallOnMacOS();
        }
        else
        {
            throw new PlatformNotSupportedException("Unsupported operating system.");
        }
    }

    private static async Task InstallOnWindowsAsync()
    {
        string appBaseDir = AppContext.BaseDirectory;
        string popplerDir = Path.Combine(appBaseDir, "poppler");
        string popplerBinDir = Path.Combine(popplerDir, "bin");

        // Check if Poppler is already installed
        if (Directory.Exists(popplerBinDir) && File.Exists(Path.Combine(popplerBinDir, "pdftoppm.exe")))
        {
            Console.WriteLine("Poppler is already installed.");
            return;
        }

        // Download the Poppler ZIP archive
        string zipFilePath = Path.Combine(appBaseDir, WindowsPopplerZipFileName);
        if (!File.Exists(zipFilePath))
        {
            Console.WriteLine("Downloading Poppler...");
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(WindowsPopplerDownloadUrl))
                {
                    response.EnsureSuccessStatusCode();
                    using (FileStream fs = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
            }
        }

        // Extract the ZIP archive
        Console.WriteLine("Extracting Poppler...");
        
        if (!Directory.Exists(popplerDir))
        {
            Console.WriteLine("Creating Poppler directory...");
            Directory.CreateDirectory(popplerDir);
            ZipFile.ExtractToDirectory(zipFilePath, popplerDir);
        }

        // Optionally, delete the ZIP file after extraction
        if (File.Exists(zipFilePath)) Console.WriteLine("Deleting ZIP file...");
        File.Delete(zipFilePath);

        Console.WriteLine("Poppler installation completed.");
    }

    private static void InstallOnLinux()
    {
        if (!IsCommandAvailable("pdftoppm"))
        {
            Console.WriteLine("Installing Poppler using apt...");
            RunCommand("bash", "-c \"sudo apt-get update && sudo apt-get install -y poppler-utils\"");
        }
        else
        {
            Console.WriteLine("Poppler is already installed.");
        }
    }

    private static void InstallOnMacOS()
    {
        if (!IsCommandAvailable("pdftoppm"))
        {
            Console.WriteLine("Installing Poppler using Homebrew...");
            RunCommand("brew", "install poppler");
        }
        else
        {
            Console.WriteLine("Poppler is already installed.");
        }
    }

    private static bool IsCommandAvailable(string command)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "where" : "which",
                    Arguments = command,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return !string.IsNullOrWhiteSpace(result);
        }
        catch
        {
            return false;
        }
    }

    private static void RunCommand(string fileName, string arguments)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing command: {ex.Message}");
        }
    }
}