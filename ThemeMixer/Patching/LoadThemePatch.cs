using System;
using System.Linq;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.Packaging;
using ColossalFramework.UI;
using HarmonyLib;
using JetBrains.Annotations;

namespace ThemeMixer.Patching
{
    [UsedImplicitly]
    [HarmonyPatch(typeof(LoadThemePanel), "Refresh")]
    public static class LoadThemePatch
    {
        private static MethodInfo _getListingItems;
        private static MethodInfo GetListingItems
        {
            get
            {
                if (_getListingItems != null) return _getListingItems;
                _getListingItems = typeof(LoadThemePanel).GetMethod("GetListingItems", BindingFlags.Instance | BindingFlags.NonPublic);
                return _getListingItems;
            }
        }

        private static MethodInfo _getListingCount;
        private static MethodInfo GetListingCount
        {
            get
            {
                if (_getListingCount != null) return _getListingCount;
                _getListingCount = typeof(LoadThemePanel).GetMethod("GetListingCount", BindingFlags.Instance | BindingFlags.NonPublic);
                return _getListingCount;
            }
        }

        private static MethodInfo _findIndexOf;
        private static MethodInfo FindIndexOf
        {
            get
            {
                if (_findIndexOf != null) return _findIndexOf;
                _findIndexOf = typeof(LoadThemePanel).GetMethod("FindIndexOf", BindingFlags.Instance | BindingFlags.NonPublic);
                return _findIndexOf;
            }
        }

        private static MethodInfo _clearListing;
        private static MethodInfo ClearListing
        {
            get
            {
                if (_clearListing != null) return _clearListing;
                _clearListing = typeof(LoadThemePanel).GetMethod("ClearListing", BindingFlags.Instance | BindingFlags.NonPublic);
                return _clearListing;
            }
        }

        private static int _addToListingParamCount = 5;
        private static MethodInfo _addToListing;
        private static MethodInfo AddToListing
        {
            get
            {
                if (_addToListing != null) return _addToListing;
                var methods = typeof(LoadThemePanel).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList();
                _addToListing = methods.Find(mi => mi.Name == "AddToListing" && mi.GetParameters().Length == _addToListingParamCount);
                if (_addToListing == null) _addToListingParamCount = 6;
                _addToListing = methods.Find(mi => mi.Name == "AddToListing" && mi.GetParameters().Length == _addToListingParamCount);
                return _addToListing;
            }
        }

        private static readonly string[] Forbidden = {
            "CO-Boreal-Theme",
            "CO-Temperate-Theme",
            "CO-Winter-Theme",
            "CO-European-Theme",
            "CO-Tropical-Theme"
        };

        private static bool Prefix(ref LoadThemePanel __instance, ref UIListBox ___m_SaveList, ref UIButton ___m_LoadButton, string ___m_LastSaveName)
        {
            using (AutoProfile.Start("LoadMapPanel.Refresh()"))
            {

                ClearListing.Invoke(__instance, null);
                bool snowfallOwned = SteamHelper.IsDLCOwned(SteamHelper.DLC.SnowFallDLC);
                foreach (Package.Asset asset in PackageManager.FilterAssets(UserAssetType.MapThemeMetaData))
                {
                    if (asset == null || !asset.isEnabled) continue;
                    try
                    {
                        var mmd = asset.Instantiate<MapThemeMetaData>();
                        mmd.SetSelfRef(asset);
                        if (mmd.environment == "Winter" && !snowfallOwned) continue;
                        var forbid = false;
                        foreach (string s in Forbidden)
                            if (asset.fullName.Contains(s))
                                forbid = true;
                        if (forbid) continue;
                        var parameters = _addToListingParamCount == 5 ? new object[] { asset.name, mmd.timeStamp, asset, mmd, true } : new object[] { asset.name, mmd.timeStamp, asset, mmd, true, false };
                        AddToListing.Invoke(__instance, parameters);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                ___m_SaveList.items = GetListingItems.Invoke(__instance, null) as string[];
                var listingCount = (int)GetListingCount.Invoke(__instance, null);
                if (listingCount > 0)
                {
                    var idx = (int)FindIndexOf.Invoke(__instance, new object[] { ___m_LastSaveName });
                    ___m_SaveList.selectedIndex = (idx != -1) ? idx : 0;
                    ___m_LoadButton.isEnabled = true;
                }
                else
                {
                    ___m_LoadButton.isEnabled = false;
                }
            }
            return false;
        }
    }
}
