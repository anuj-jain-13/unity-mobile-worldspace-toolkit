using EditorUtill;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher {

    #region Orientation Methods
    public static void SwitchOrientation(bool isPortrait) {
        Debug.Log("SceneSwitcher.SwitchOrientation()...isPortrait : " + isPortrait);
        LoaderView.StartLoader("Loading...", isPortrait);
        if (isPortrait) {
            SwitchOrientationToPortrait();
        } else {
            SwitchOrientationToLandscape();
        }
    }

    private static void SwitchOrientationToPortrait() {
        Screen.orientation = ScreenOrientation.Portrait;
        EditorGameViewUtill.SetPortraitWindow();
        Debug.Log("SceneSwitcher.SwitchOrientationToPortrait()...Screen.orientation : " + Screen.orientation);
    }

    private static void SwitchOrientationToLandscape() {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        EditorGameViewUtill.SetLandscapeWindow();
        Debug.Log("SceneSwitcher.SwitchOrientationToLandscape()...Screen.orientation : " + Screen.orientation);
    }
    #endregion

    #region Scene Loader Methods
    public static void LoadSceneByName(string sceneName) {        
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.completed += OnSceneFinishLoading;
    }    

    private static void OnSceneFinishLoading(AsyncOperation operation) {
        operation.completed -= OnSceneFinishLoading;
        LoaderView.StopLoader(false);
    }
    #endregion
}