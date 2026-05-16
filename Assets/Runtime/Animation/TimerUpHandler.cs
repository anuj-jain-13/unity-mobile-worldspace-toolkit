using UnityEngine;
using TMPro;
using System;

public class TimerUpHandler : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI timerTMPUI;
    [SerializeField] private TextMeshPro timerTMP;

    private int totalTime;

    #region System Methods
    private void Awake() {
        registerEvents();
    }

    private void OnDestroy() {
        deregisterEvents();
    }
    #endregion
    
    #region Timer Methods
    private void setTimeAndStart(GameObject senderGO, object startTime) {
        totalTime = (int)startTime;
        startTimer();
    }

    private void startTimer() {
        InvokeRepeating(nameof(updateTimeUI), 1, 1);
    }

    private void endTimer(GameObject senderGO) {
        CancelInvoke(nameof(updateTimeUI));
        EventManager.triggerEvent("TimerDownHandler.TimeEnded", gameObject);
    }

    private void updateTimeUI() {
        if (totalTime >= 0) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalTime++);
            if (timerTMP) {
                timerTMP.text = timeSpan.FormattedTimer("M:S");
            } else if (timerTMPUI) {
                timerTMP.text = timeSpan.FormattedTimer("M:S");
            }
        }
    }
    #endregion

    #region Event Methods
    private void registerEvents() {
        EventManager.startListening(AppEventsId.EggOnSetTimeUpEvent, setTimeAndStart);
        EventManager.startListening(AppEventsId.EggOnEndTheTimerEvent, endTimer);
    }

    private void deregisterEvents() {
        EventManager.stopListening(AppEventsId.EggOnSetTimeUpEvent, setTimeAndStart);
        EventManager.stopListening(AppEventsId.EggOnEndTheTimerEvent, endTimer);
    }
    #endregion
}