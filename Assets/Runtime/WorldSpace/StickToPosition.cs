using UnityEngine;

[ExecuteAlways]
public class StickToPosition : MonoBehaviour {

    public enum HorizontalAnchor {
        None, Left, Right, Center, CenterPercentLeft, CenterPercentRight
    }

    public enum VerticalAnchor {
        None, Bottom, Top, Center, CenterPercentTop, CenterPercentBottom
    }

    [Header("Horizontal")]
    [SerializeField] private HorizontalAnchor horizontalAnchor = HorizontalAnchor.None;
    [SerializeField] private float horizontalOffset = 0f;
    [SerializeField, Range(0f, 1f)] private float horizontalPercent = 0f;
    [SerializeField] private bool horizonalAffectedByNotch = false;

    [Header("Vertical")]
    [SerializeField] private VerticalAnchor verticalAnchor = VerticalAnchor.None;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField, Range(0f, 1f)] private float verticalPercent = 0f;
    [SerializeField] private bool verticalAffectedByNotch = false;

    [Header("Container (optional)")]
    [SerializeField] private BoxCollider2D containerBox2d;

    [Header("Advanced")]
    [SerializeField] private bool usePivot = false;
    [SerializeField] private float notchHorizontalOffset = 0.65f;
    [SerializeField] private float notchVerticalOffset = 0.65f;

    private void Start() {
        UpdatePosition();
    }

#if UNITY_EDITOR
    //private void OnValidate()
    private void LateUpdate() {
        if (!Application.isPlaying)
            UpdatePosition(); // ensures layout is complete in edit mode
    }
#endif

    private void UpdatePosition() {
        if (!Camera.main) return;

        Rect rectArea = containerBox2d
            ? containerBox2d.bounds.GetRectFromBounds()
            : Camera.main.GetScreenWorldRect();

        float halfWidth = usePivot ? 0f : GetWorldWidth(transform) / 2f;
        float halfHeight = usePivot ? 0f : GetWorldHeight(transform) / 2f;

        Vector3 newPos = transform.position;

        // Horizontal alignment
        switch (horizontalAnchor) {
            case HorizontalAnchor.Left:
                newPos.x = rectArea.xMin + horizontalOffset + halfWidth + getHorizontalNotchOffset();
                break;
            case HorizontalAnchor.Right:
                newPos.x = rectArea.xMax - horizontalOffset - halfWidth - getHorizontalNotchOffset();
                break;
            case HorizontalAnchor.Center:
                newPos.x = rectArea.center.x + horizontalOffset + getHorizontalNotchOffset();
                break;
            case HorizontalAnchor.CenterPercentLeft:
                newPos.x = rectArea.xMin + rectArea.width * horizontalPercent + getHorizontalNotchOffset();
                break;
            case HorizontalAnchor.CenterPercentRight:
                newPos.x = rectArea.xMax - rectArea.width * horizontalPercent - getHorizontalNotchOffset();
                break;
        }

        // Vertical alignment
        switch (verticalAnchor) {
            case VerticalAnchor.Bottom:
                newPos.y = rectArea.yMin + verticalOffset + halfHeight + getVerticalNotchOffset();
                break;
            case VerticalAnchor.Top:
                newPos.y = rectArea.yMax - verticalOffset - halfHeight - getVerticalNotchOffset();
                break;
            case VerticalAnchor.Center:
                newPos.y = rectArea.center.y + verticalOffset + getVerticalNotchOffset();
                break;
            case VerticalAnchor.CenterPercentTop:
                newPos.y = rectArea.yMax - rectArea.height * verticalPercent - getVerticalNotchOffset();
                break;
            case VerticalAnchor.CenterPercentBottom:
                newPos.y = rectArea.yMin + rectArea.height * verticalPercent + getVerticalNotchOffset();
                break;
        }
        transform.position = newPos;
    }

    private float GetWorldWidth(Transform t) {
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.x : t.lossyScale.x;
    }

    private float GetWorldHeight(Transform t) {
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.y : t.lossyScale.y;
    }

    private float getHorizontalNotchOffset() {
        return horizonalAffectedByNotch && NotchDetector.DeviceHasNotch ? notchHorizontalOffset : 0;
    }

    private float getVerticalNotchOffset() {
        return verticalAffectedByNotch && NotchDetector.DeviceHasNotch ? notchVerticalOffset : 0;
    }
}