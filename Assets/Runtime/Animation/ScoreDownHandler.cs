using TMPro;
using UnityEngine;

public class ScoreDownHandler : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreTMPUI;
    [SerializeField] private TextMeshPro scoreTMP;    

    #region System Methods
    private void Awake() {
        registerEvents();
    }

    private void OnDestroy() {
        deregisterEvents();
    }
    #endregion

    #region ScoreDownHandler Methods
    private void updateScoreRoundUI(GameObject senderGO, object score, object round) {
        if (scoreTMP) {
            scoreTMP.text = score + " of " + round;
        } else if (scoreTMPUI) {
            scoreTMP.text = score + " of " + round;
        }
    }

    private void updateScoreUI(GameObject senderGO, object score) {
        if (scoreTMP) {
            scoreTMP.text = score.ToString();
        } else if (scoreTMPUI) {
            scoreTMPUI.text = score.ToString();
        }
    }
    #endregion

    #region Event Methods
    private void registerEvents() {
        EventManager.startListening(AppEventsId.EggOnScoreDownStartEvent, updateScoreRoundUI);
        EventManager.startListening(AppEventsId.EggOnScoreDownStartEvent, updateScoreUI);
    }

    private void deregisterEvents() {
        EventManager.stopListening(AppEventsId.EggOnScoreDownStartEvent, updateScoreRoundUI);
        EventManager.stopListening(AppEventsId.EggOnScoreDownStartEvent, updateScoreUI);
    }
    #endregion
}