// <copyright file="FileWatcherPatches.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace ThemeMixer.Patching
{
    using System.Collections.Generic;
    using System.Reflection;
    using ColossalFramework;
    using ColossalFramework.Plugins;
    using HarmonyLib;

    /// <summary>
    /// Harmony patches to prevent game from trying to instantiate "mods" (.dll files) created when the game is running (e.g. new themes).
    /// </summary>
    [HarmonyPatch]
    internal static class FileWatcherPatches
    {
        /// <summary>
        /// Determines list of target methods to patch.
        /// </summary>
        /// <returns>List of target methods to patch.</returns>
        internal static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(PluginManager), "OnFileWatcherEventChanged");
            yield return AccessTools.Method(typeof(PluginManager), "OnFileWatcherEventCreated");
            yield return AccessTools.Method(typeof(PluginManager), "OnFileWatcherEventRenamed");
        }

        /// <summary>
        /// Harmonmy pre-emptive prefix for overridden file watcher methods.
        /// </summary>
        /// <returns>True (execute orignal method) if mod isn't active or not in-game, false otherwise.</returns>
        internal static bool Prefix() => !Singleton<LoadingManager>.instance.m_loadingComplete;
    }
}
