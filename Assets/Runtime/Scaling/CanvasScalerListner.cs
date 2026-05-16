using UnityEngine;

public class CanvasScalarListner : MonoBehaviour {

    #region System Methods
    private void Awake() {
        registerEvents();
    }

    private void OnDestroy() {
        deregisterEvents();
    }
    #endregion

    #region CanvasScalarListner Methods
    private void onCanvasScalarScaled(GameObject senderGO) {
        CanvasScalar2D scalar2D = senderGO.GetComponent<CanvasScalar2D>();
        float oriYPos = transform.position.y;
        float scaledYPos = oriYPos * scalar2D.canvasScale;
        float newYPer = (oriYPos - scaledYPos) * 100 / scaledYPos;
        float newYPos = oriYPos + oriYPos * newYPer / 100;
        transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
        Debug.Log($"CanvasScalarListner.onCanvasScalarScaled()...canvasScale : {scalar2D.canvasScale}, oriYPos : {oriYPos}, scaledYPos : {scaledYPos}, newYPer : {newYPer}, newYPos : {newYPos}");
    }
    #endregion

    #region Event Methods
    private void registerEvents() {
        EventManager.startListening(AppEventsId.CanvasScalar2DScaled, onCanvasScalarScaled);
    }

    private void deregisterEvents() {
        EventManager.stopListening(AppEventsId.CanvasScalar2DScaled, onCanvasScalarScaled);
    }
    #endregion
}