using TMPro;
using UnityEngine;

public class ScoreUpHandler : MonoBehaviour {

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

    #region ScoreHandler Methods
    private void updateScoreAndRoundUI(GameObject senderGO, object score, object round) {
        if (scoreTMP) {
            scoreTMP.text = (int)score + " of " + (int)round;
        } else if (scoreTMPUI) {
            scoreTMPUI.text = (int)score + " of " + (int)round;
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
        EventManager.startListening(AppEventsId.TrainOnScoreAndRoundUpEvent, updateScoreAndRoundUI);
        EventManager.startListening(AppEventsId.EggOnScoreUpEvent, updateScoreUI);
    }

    private void deregisterEvents() {
        EventManager.stopListening(AppEventsId.TrainOnScoreAndRoundUpEvent, updateScoreAndRoundUI);
        EventManager.stopListening(AppEventsId.EggOnScoreUpEvent, updateScoreUI);
    }
    #endregion
}