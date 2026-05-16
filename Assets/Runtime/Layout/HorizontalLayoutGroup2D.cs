using UnityEngine;

public class HorizontalLayoutGroup2D : MonoBehaviour {

    [SerializeField] private float spacing = 1f;
    [SerializeField] BoxCollider2D box2d;

    void Start() {
        Rect rectArea;
        if (box2d) {
            rectArea = box2d.bounds.GetRectFromBounds();
        } else {
            rectArea = Camera.main.GetViewportWorldRect();
        }
        float distance = (rectArea.width - spacing * (transform.childCount - 1)) / (transform.childCount + 1);
        for (int idx = 0; idx < transform.childCount; idx++) {
            Transform child = transform.GetChild(idx);
            Vector3 newPos = new(rectArea.xMin + spacing * idx + (distance * (idx + 1)), child.position.y, child.position.z);
            child.position = newPos;
        }
    }    
}