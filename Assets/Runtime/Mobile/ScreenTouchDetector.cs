using UnityEngine;

public class ScreenTouchDetector {

    private static float TouchBeganTime;

    #region Touch Related Methods
    public static bool IsTouchOverLayer(string layerName) {   // Perform a raycast to check if the touch is on a given Layer element                
        if (IsValidEditorTouch() || IsValidDeviceTouch()) {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(AppUtill.IsEditor ? Input.mousePosition : Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
            Debug.Log("ScreenTouchDetector.IsTouchOverLayer()...layerName : " + layerName + ", hit.collider : <b>" + hit.collider + "</b>" + ", WorldTouchPosition : " + touchPosition + ", Input.mousePosition : " + Input.mousePosition);
            if (hit.collider != null) {
                Debug.Log("ScreenTouchDetector.IsTouchOverLayer()...<b><color=magenta>Touched Layer Name :</color> '" + LayerMask.LayerToName(hit.collider.gameObject.layer) + "'</b>");                
                return hit.collider.gameObject.layer == LayerMask.NameToLayer(layerName);
            }
        }
        return false;
    }

    private static bool IsValidDeviceTouch() {
        if (!AppUtill.IsEditor) {
            Touch touch = Input.GetTouch(0);    //Get very first touch on screen.
            if (touch.phase == TouchPhase.Began) {
                TouchBeganTime = Time.time;
            } else if (touch.phase == TouchPhase.Moved) {
                return false;
            } else if (touch.phase == TouchPhase.Ended) {
                float timeDiff = Time.time - TouchBeganTime;
                if (timeDiff <= 0.2f) { //Time Difference between Touch Began and End sould be less than 0.1f for valid touch...
                    return true;
                }
            }
        }
        return false;
    }

    private static bool IsValidEditorTouch() {
        if (AppUtill.IsEditor) {
            if (Input.GetMouseButtonDown(0)) {
                TouchBeganTime = Time.time;
            } else if (Input.GetMouseButtonUp(0)) {
                float timeDiff = Time.time - TouchBeganTime;
                if (timeDiff <= 0.2f) { //Time Difference between Touch Began and End sould be less than 0.1f for valid touch...
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
}