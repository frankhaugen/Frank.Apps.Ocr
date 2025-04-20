using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Frank.Apps.Ocr.Api.Poppler;

public static class PopplerHelper
{
    public static async Task<Dictionary<int, byte[]>> ConvertPdfToPngPagesAsync(byte[] pdfBytes)
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        string pdfPath = Path.Combine(tempDir, "input.pdf");
        await File.WriteAllBytesAsync(pdfPath, pdfBytes);

        string outputPrefix = Path.Combine(tempDir, "page");

        string pdftoppmPath = GetPdftoppmExecutablePath();

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = pdftoppmPath,
                Arguments = $"-png -r 300 \"{pdfPath}\" \"{outputPrefix}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
        string stderr = await process.StandardError.ReadToEndAsync();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"pdftoppm failed: {stderr}");
        }

        var result = new Dictionary<int, byte[]>();
        var pngFiles = Directory.GetFiles(tempDir, "page-*.png");

        foreach (var file in pngFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            if (int.TryParse(fileName.Split('-')[1], out int pageNumber))
            {
                byte[] imageBytes = await File.ReadAllBytesAsync(file);
                result[pageNumber] = imageBytes;
            }
        }

        // Clean up temporary files
        Directory.Delete(tempDir, true);

        return result;
    }

    private static string GetPdftoppmExecutablePath()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string appBaseDir = AppContext.BaseDirectory;
            string popplerBinDir = Path.Combine(appBaseDir, "poppler", "poppler-24.08.0", "Library", "bin");
            string pdftoppmPath = Path.Combine(popplerBinDir, "pdftoppm.exe");

            if (!File.Exists(pdftoppmPath))
            {
                throw new FileNotFoundException($"pdftoppm.exe not found in directory: {popplerBinDir}");
            }

            return pdftoppmPath;
        }
        else
        {
            // Assumes pdftoppm is in the system PATH
            return "pdftoppm";
        }
    }
}