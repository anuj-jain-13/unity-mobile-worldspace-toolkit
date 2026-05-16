using UnityEngine;

public class CanvasScalar2D : MonoBehaviour {

    [SerializeField] private Vector2 referenceResolution;

    public float canvasScale { get; private set; }

    void Start() {
        float refAspect = referenceResolution.x / referenceResolution.y;
        float screenAspect = (float)Screen.width / Screen.height;
        canvasScale = screenAspect / refAspect;
        transform.localScale = canvasScale * Vector3.one;
        EventManager.triggerEvent(AppEventsId.CanvasScalar2DScaled, gameObject);
    }
}