using System;
using System.IO;

public static class FileDebugger
{
    private static string debugFilePath;

    public static void Initialize()
    {
        string assemblyLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string assemblyDirectory = assemblyLocation;

        // Log assembly directory for debugging
        UnityEngine.Debug.Log("Assembly Directory: " + assemblyDirectory);

        // Ensure the directory exists, create it if it doesn't
        if (!Directory.Exists(assemblyDirectory))
        {
            Directory.CreateDirectory(assemblyDirectory);
        }

        debugFilePath = Path.Combine(assemblyDirectory, "debug_log.txt");

        // Ensure the file is created or overwritten
        File.WriteAllText(debugFilePath, string.Empty);
    }


    public static void Debug(string message)
    {
        Initialize();
        UnityEngine.Debug.Log(message);
        if (string.IsNullOrEmpty(debugFilePath))
        {
            throw new InvalidOperationException("Debugger has not been initialized. Call Initialize() first.");
        }

        try
        {
            using (StreamWriter writer = File.AppendText(debugFilePath))
            {
                writer.WriteLine($"{DateTime.Now} - {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to debug log: {ex.Message}");
        }
    }
}