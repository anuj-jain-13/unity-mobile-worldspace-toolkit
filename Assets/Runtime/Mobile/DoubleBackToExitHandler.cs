using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Text

public class DoubleBackToExitHandler : MonoBehaviourSingleton<DoubleBackToExitHandler> {

    [SerializeField] private float timeBetweenPresses = 2.0f; // Time window for double tap
    [SerializeField] private float autoHideDelay = 2.5f; // Time window for double tap
    [SerializeField] private GameObject exitPromptPanel; // Assign your UI Panel for the prompt here

    private float lastBackPressTime = 0f;
    private bool promptActive = false;

    void Update() {
        // Check for Android back button (mapped to Escape key in Unity)
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Time.time - lastBackPressTime < timeBetweenPresses) {   // Second back press within the time window                
                hideExitPrompt();
                confirmExit(); // Quit the application
            } else {    // First back press, show prompt                
                showExitPrompt();
                lastBackPressTime = Time.time;
            }
        }
    }

    private void showExitPrompt() {
        if (exitPromptPanel != null) {
            exitPromptPanel.SetActive(true);
            promptActive = true;
            Invoke(nameof(hideExitPrompt), autoHideDelay);
            // You might want to start a coroutine here to automatically hide the prompt after a few seconds
            // if the user doesn't press back again.
        }
    }

    private void hideExitPrompt() {
        if (exitPromptPanel != null) {
            CancelInvoke(nameof(hideExitPrompt));
            exitPromptPanel.SetActive(false);
            promptActive = false;
        }
    }

    public void confirmExit() {
        Application.Quit();
    }
}