using UnityEngine;

public class CameraScroller : MonoBehaviour {
    public float scrollSpeed = 2f;      // Speed of the camera movement
    public float verticalOffset = 4.5f;  // Vertical distance to move the camera

    private bool isScrolling = false;  // Check if the camera is currently scrolling
    private float targetY;             // Target Y position for the camera

    #region System Methods
    void Start() {
        targetY = transform.position.y; // Initialize target position
    }

    void Update() {
        if (isScrolling) {
            // Smoothly move the camera towards the target position
            float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * scrollSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // Stop scrolling when the target is reached
            if (Mathf.Abs(transform.position.y - targetY) < 0.1f) {
                isScrolling = false;
                EventManager.triggerEvent(AppEventsId.EggOnCameraScrollingEvent, gameObject, false);
            }
        }
    }
    #endregion

    #region Camera Scroller Methods
    public void ScrollUp() {
        // Trigger scrolling by updating the target position
        targetY += verticalOffset;
        isScrolling = true;
        EventManager.triggerEvent(AppEventsId.EggOnCameraScrollingEvent, gameObject, true);
    }
    #endregion
}