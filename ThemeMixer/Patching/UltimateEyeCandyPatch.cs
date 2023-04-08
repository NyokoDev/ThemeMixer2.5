using System;
using System.Reflection;
using HarmonyLib;
using UltimateEyecandy.GUI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThemeMixer.Patching
{
    public class UltimateEyeCandyPatch
    {
        private static MethodInfo _postfixMethod;
        private static MethodInfo PostfixMethod => _postfixMethod ?? (_postfixMethod = MethodBase.GetCurrentMethod() as MethodInfo);

        private static MethodInfo _targetMethod;
        private static MethodInfo TargetMethod => _targetMethod ?? (_targetMethod = typeof(ColorManagementPanel).GetMethod(nameof(ColorManagementPanel.Awake), BindingFlags.Instance | BindingFlags.NonPublic));

        private static OptionsGraphicsPanel _ogp;
        private static OptionsGraphicsPanel Ogp => _ogp ?? (_ogp = Object.FindObjectOfType<OptionsGraphicsPanel>());

        public void Patch(Harmony harmonyInstance)
        {
            try
            {
                if (TargetMethod != null && PostfixMethod != null)
                {
                    harmonyInstance.Patch(TargetMethod, null, new HarmonyMethod(PostfixMethod));
                }
                else
                {
                    Debug.Log($"Failed to patch {nameof(UltimateEyeCandyPatch)}: target method or postfix method is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to patch {nameof(UltimateEyeCandyPatch)}: {ex}");
            }
        }

        public void Unpatch(Harmony harmonyInstance)
        {
            try
            {
                if (TargetMethod != null && PostfixMethod != null)
                {
                    harmonyInstance.Unpatch(TargetMethod, PostfixMethod);
                }
                else
                {
                    Debug.Log($"Failed to unpatch {nameof(UltimateEyeCandyPatch)}: target method or postfix method is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to unpatch {nameof(UltimateEyeCandyPatch)}: {ex}");
            }
        }

        private static void Postfix(ref ColorManagementPanel __instance)
        {
            try
            {
                if (__instance != null && __instance.loadLutButton != null)
                {
                    __instance.loadLutButton.eventClicked -= LoadLutButtonClickedDelegate;
                    __instance.loadLutButton.eventClicked += LoadLutButtonClickedDelegate;
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to add event handler: {ex}");
            }
        }

        private static void OnLoadLutButtonClicked(ColossalFramework.UI.UIComponent component, ColossalFramework.UI.UIMouseEventParameter eventParam)
        {
            try
            {
                Ogp?.SendMessage("RefreshColorCorrectionLUTs");
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to send message: {ex}");
            }
        }

        private static readonly ColossalFramework.UI.MouseEventHandler LoadLutButtonClickedDelegate = new ColossalFramework.UI.MouseEventHandler(OnLoadLutButtonClicked);
    }
}
