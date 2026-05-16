using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorUtill {

    public static class EditorGameViewUtill {

        static object gameViewInstance;

        static Type gameViewType;
        static Type gameViewSizeSingleton;
        static Type gameViewSizesType;

        static MethodInfo gameViewSizeCBInfo;        
        static MethodInfo gameViewSizesInfo;
        
        static int portraitGameViewIdx = -1;
        static string[] gameViewSizeTexts;
#if UNITY_EDITOR
        static EditorGameViewUtill() {
            gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            gameViewSizeCBInfo = gameViewType.GetMethod("SizeSelectionCallback", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);             
            gameViewSizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            gameViewSizeSingleton = typeof(ScriptableSingleton<>).MakeGenericType(gameViewSizesType);
            gameViewSizesInfo = gameViewSizesType.GetMethod("GetGroup");
            gameViewInstance = gameViewSizeSingleton.GetProperty("instance").GetValue(null, null);
        }

        public static int TrySetGameViewSize(string sizeText) {
            GameViewSizeGroupType currentGroup = GetCurrentGroupType();
            int foundIndex = FindSize(currentGroup, sizeText);
            if (foundIndex < 0) {
                Debug.Log("Size " + sizeText + " was not found in game view settings");
                return -1;
            }
            SetGameViewIndex(foundIndex);
            return foundIndex;
        }

        public static void SetGameViewIndex(int index) {                      
            EditorWindow currentWindow = EditorWindow.focusedWindow;
            SceneView lastSceneView = SceneView.lastActiveSceneView;

            EditorWindow gv = EditorWindow.GetWindow(gameViewType);
            gameViewSizeCBInfo.Invoke(gv, new object[] { index, null });    // Calling GameView.gameViewSizeCBInfo will also auto focus game view...

            // We will restore focus if it is something else
            if (lastSceneView != null)
                lastSceneView.Focus();

            if (currentWindow != null)
                currentWindow.Focus();
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, string text) {
            if (gameViewSizeTexts == null) {
                var group = GetGroup(sizeGroupType);
                var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
                gameViewSizeTexts = getDisplayTexts.Invoke(group, null) as string[];
            }
            for (int i = 0; i < gameViewSizeTexts.Length; i++) {
                string display = gameViewSizeTexts[i];

                bool found = display.Contains(text);
                if (found)
                    return i;
            }
            return -1;
        }

        public static bool IsCorrectSizeAtIndex(int idx, string toFind) {
            if (gameViewSizeTexts.Length > idx && gameViewSizeTexts[idx].Contains(toFind)) {
                return true;
            }
            return false;
        }

        static object GetGroup(GameViewSizeGroupType type) {
            return gameViewSizesInfo.Invoke(gameViewInstance, new object[] { (int)type });
        }

        public static GameViewSizeGroupType GetCurrentGroupType() {
#if UNITY_STANDALONE
            return GameViewSizeGroupType.Standalone;
#elif UNITY_IOS
            return GameViewSizeGroupType.iOS;
#elif UNITY_ANDROID
            return GameViewSizeGroupType.Android;
#endif
        }
#endif

        public static void SetPortraitWindow() {
#if UNITY_EDITOR            
            portraitGameViewIdx = TrySetGameViewSize(Display.displays[0].renderingWidth + "x" + Display.displays[0].renderingHeight);
            portraitGameViewIdx = portraitGameViewIdx == -1 ? TrySetGameViewSize(Display.displays[0].renderingWidth + ":" + Display.displays[0].renderingHeight) : portraitGameViewIdx;
            if (portraitGameViewIdx > 0) {
                if (!IsCorrectSizeAtIndex(portraitGameViewIdx, "Portrait")) {
                    portraitGameViewIdx++;
                    SetGameViewIndex(portraitGameViewIdx);
                }
            }
            Debug.Log("SetPortraitWindow()...portraitGameViewIdx : " + portraitGameViewIdx);
#endif
        }

        public static void SetLandscapeWindow() {
#if UNITY_EDITOR
            if (portraitGameViewIdx > 0) {
                if (IsCorrectSizeAtIndex(portraitGameViewIdx + 1, "Landscape")) {
                    SetGameViewIndex(portraitGameViewIdx + 1);
                } else {
                    SetGameViewIndex(portraitGameViewIdx - 1);
                }
            }
#endif
        }
    }

}