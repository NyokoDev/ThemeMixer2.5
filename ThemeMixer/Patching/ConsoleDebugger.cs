using System;
using System.IO;

public static class FileDebugger
{
    private static string debugFilePath;

    public static void Initialize()
    {

    }


    public static void Debug(string message)
    {

        UnityEngine.Debug.Log(message);
     
    }
}