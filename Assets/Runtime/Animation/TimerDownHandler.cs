using UnityEngine;
using TMPro;
using System;

public class TimerDownHandler : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI timerTMPUI;
    [SerializeField] private TextMeshPro timerTMP;

    private int remainingTime;

    #region System Methods
    private void Awake() {
        registerEvents();
    }

    private void OnDestroy() {
        deregisterEvents();
    }
    #endregion

    #region Timer Methods
    private void setTimeAndStart(GameObject senderGO, object totalTime) {
        Debug.Log(" totalTime : " + totalTime);
        remainingTime = (int)totalTime;
        startTimer();
    }

    private void startTimer() {
        InvokeRepeating(nameof(updateTimeUI), 1, 1);
    }

    private void resumeTheTimer(GameObject senderGO) {
        startTimer();
    }

    private void stopTimerOnLevelUp(GameObject senderGO, object level) {
        CancelInvoke(nameof(updateTimeUI));
    }

    private void stopTimerOnGameOver(GameObject senderGO, object senderIdStr) {
        CancelInvoke(nameof(updateTimeUI));
    }

    private void endTimer() {
        CancelInvoke(nameof(updateTimeUI));
        EventManager.triggerEvent(AppEventsId.EggOnTimerEndedEvent, gameObject, typeof(TimerDownHandler).ToString());
    }

    private void updateTimeUI() {
        if (remainingTime >= 0) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime--);
            if (timerTMP) {
                timerTMP.text = timeSpan.FormattedTimer("M:S");
            } else if (timerTMPUI) {
                timerTMPUI.text = timeSpan.FormattedTimer("M:S");
            }
        } else {
            endTimer();
        }
    }
    #endregion

    #region Event Methods
    private void registerEvents() {
        EventManager.startListening(AppEventsId.EggOnSetTimeDownStartEvent, setTimeAndStart);
        EventManager.startListening(AppEventsId.EggOnSetTimeDownResumeEvent, resumeTheTimer);
        EventManager.startListening(AppEventsId.EggOnLevelUpEvent, stopTimerOnLevelUp);
        EventManager.startListening(AppEventsId.EggOnGameOverEvent, stopTimerOnGameOver);
    }

    private void deregisterEvents() {
        EventManager.stopListening(AppEventsId.EggOnSetTimeDownStartEvent, setTimeAndStart);
        EventManager.stopListening(AppEventsId.EggOnSetTimeDownResumeEvent, resumeTheTimer);
        EventManager.stopListening(AppEventsId.EggOnLevelUpEvent, stopTimerOnLevelUp);
        EventManager.stopListening(AppEventsId.EggOnGameOverEvent, stopTimerOnGameOver);
    }
    #endregion
}