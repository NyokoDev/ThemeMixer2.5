using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AlgernonCommons;
using ColossalFramework.IO;
using ColossalFramework.PlatformServices;
using HarmonyLib;
using UnityEngine;

namespace ThemeMixer.Patching
{
  

    [HarmonyPatch(typeof(Workshop), "UpdateItem", typeof(PublishedFileId), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string[]))]
    public static class WorkshopUpdateItemPatch
    {
   
        private static void Prefix(string contentPath, ref string[] tags)
        {
     
            if (File.Exists(Path.Combine(contentPath, "ThemeMix.xml")))
            {
      
                tags = new[] { SteamHelper.kSteamTagMapTheme, "Theme Mix" };
                


            }
        }


        [HarmonyPatch(typeof(WorkshopModUploadPanel), "PrepareStagingArea")]
        public static class StagingPatch
        {
            public static void Postfix()
            {
                string folders = string.Join("\\", new[] { "Colossal Order", "Cities_Skylines", "WorkshopStagingArea" });
           
                string workshopStagingAreaPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folders);
                FileDebugger.Debug(workshopStagingAreaPath);

                // Get the latest directory based on creation time
                DirectoryInfo latestDirectory = new DirectoryInfo(workshopStagingAreaPath)
                    .GetDirectories()
                    .OrderByDescending(d => d.CreationTime)
                    .FirstOrDefault();

                if (latestDirectory != null)
                {
                    string inPath = Path.Combine(latestDirectory.FullName, "Content");
                    if (File.Exists(Path.Combine(inPath, "ThemeMix.xml")))
                    {
                        // Check if workshopStagingAreaPath is null or empty
                        if (string.IsNullOrEmpty(workshopStagingAreaPath))
                        {
                            Debug.Log("Invalid Workshop Staging Area path.");
                            FileDebugger.Debug("Invalid Workshop Staging Area path.");
                            return;
                        }

                        // Check if the directory exists
                        if (!Directory.Exists(workshopStagingAreaPath))
                        {
                            Debug.Log("Workshop Staging Area path not found.");

                            // Workaround: Create the directory
                            try
                            {
                                Directory.CreateDirectory(workshopStagingAreaPath);
                                Debug.Log("Workshop Staging Area path created: " + workshopStagingAreaPath);
                            }
                            catch (Exception ex)
                            {
                                Debug.LogError("Error creating Workshop Staging Area path: " + ex.Message);
                                FileDebugger.Debug("Error creating Workshop Staging Area path: " + ex.Message);
                                return;
                            }
                        }

                        FileDebugger.Debug(workshopStagingAreaPath);


                        // Get the latest directory based on creation time
                        DirectoryInfo latestDirectoryOm = new DirectoryInfo(workshopStagingAreaPath)
                            .GetDirectories()
                            .OrderByDescending(d => d.CreationTime)
                            .FirstOrDefault();

                        if (latestDirectory == null)
                        {
                            Debug.Log("No directories found in Workshop Staging Area.");
                            FileDebugger.Debug("No directories found in Workshop Staging Area.");
                            return;
                        }

                        // Path to the initial image (ThemeMix.png)
                        string folders2 = string.Join("\\", new[] { "Resources", "ThemeMix.png" });
                        FileDebugger.Debug(folders2);
                        string initialImagePath = Path.Combine(AssemblyUtils.AssemblyPath, folders2);
                        FileDebugger.Debug(initialImagePath);
                        FileDebugger.Debug("Initial path = " + initialImagePath);

                        // Copy the initial image into the latest directory
                        try
                        {
                            string destinationImagePath = Path.Combine(latestDirectory.FullName, "PreviewImage.png");
                            File.Copy(initialImagePath, destinationImagePath, true);
                            FileDebugger.Debug("Initial image (ThemeMix.png) added successfully.");
                        }
                        catch (Exception ex)
                        {
                            FileDebugger.Debug($"Failed to copy initial image (ThemeMix.png): {ex.Message}");
                        }
                    }
                    else
                    {
                        FileDebugger.Debug("Not a theme mix?");
                    }
                }
                else
                {
                    // Handle case when no directories are found
                }


            }
        }
    }
}