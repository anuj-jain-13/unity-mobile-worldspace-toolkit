using UnityEngine;

/// <summary>
/// Positions a world-space object relative to the visible area,
/// based on ResolutionManager's calculated world bounds.
/// Works for both gameplay objects and world-space UI.
/// </summary>
[ExecuteAlways]
public class WorldObjectPosition : MonoBehaviour
{
    public enum HorizontalAnchor
    {
        None, Left, Right, Center, CenterPercentFromLeft, CenterPercentFromRight
    }

    public enum VerticalAnchor
    {
        None, Bottom, Top, Center, CenterPercentFromTop, CenterPercentFromBottom
    }

    [Header("Horizontal")]
    [SerializeField] private HorizontalAnchor horizontalAnchor = HorizontalAnchor.None;
    [SerializeField] private float horizontalOffset = 0f;
    [Range(0f, 1f)]
    [SerializeField] private float horizontalPercent = 0f;

    [Header("Vertical")]
    [SerializeField] private VerticalAnchor verticalAnchor = VerticalAnchor.None;
    [SerializeField] private float verticalOffset = 0f;
    [Range(0f, 1f)]
    [SerializeField] private float verticalPercent = 0f;

    [Header("Advanced")]
    [Tooltip("If enabled, the pivot point of the object will be used instead of half its size.")]
    [SerializeField] private bool usePivot = false;

    private void Start()
    {
        UpdateAnchorPosition();
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
        if (!Application.isPlaying)
            UpdateAnchorPosition();
    }
#endif

    public void UpdateAnchorPosition()
    {
        if (ResolutionManager.Instance == null) return;

        Vector2 bottomLeft = ResolutionManager.Instance.GetBottomLeftWorldPos();
        Vector2 topRight = ResolutionManager.Instance.GetTopRightWorldPos();
        Vector2 center = (bottomLeft + topRight) * 0.5f;

        WorldObjectAutoScaler autoScalerComp = transform.GetComponent<WorldObjectAutoScaler>();
        float scaleFactor = autoScalerComp != null ? autoScalerComp.FinalScaleFactor : ResolutionManager.Instance.HeightScaleMultiplier;

        float halfWidth = usePivot ? 0f : GetWorldWidth(transform) / 2f;
        float halfHeight = usePivot ? 0f : GetWorldHeight(transform) / 2f;

        Vector3 newPos = transform.position;

        // Horizontal alignment
        switch (horizontalAnchor)
        {
            case HorizontalAnchor.Left:
                newPos.x = bottomLeft.x + horizontalOffset * scaleFactor + halfWidth;
                break;
            case HorizontalAnchor.Right:
                newPos.x = topRight.x - horizontalOffset * scaleFactor - halfWidth;
                break;
            case HorizontalAnchor.Center:
                newPos.x = center.x + horizontalOffset * scaleFactor;
                break;
            case HorizontalAnchor.CenterPercentFromLeft:
                newPos.x = bottomLeft.x + (topRight.x - bottomLeft.x) * horizontalPercent;
                break;
            case HorizontalAnchor.CenterPercentFromRight:
                newPos.x = topRight.x - (topRight.x - bottomLeft.x) * horizontalPercent;
                break;
        }

        // Vertical alignment
        switch (verticalAnchor)
        {
            case VerticalAnchor.Bottom:
                newPos.y = bottomLeft.y + verticalOffset * scaleFactor + halfHeight;
                break;
            case VerticalAnchor.Top:
                newPos.y = topRight.y - verticalOffset * scaleFactor - halfHeight;
                break;
            case VerticalAnchor.Center:
                newPos.y = center.y + verticalOffset * scaleFactor;
                break;
            case VerticalAnchor.CenterPercentFromTop:
                newPos.y = topRight.y - (topRight.y - bottomLeft.y) * verticalPercent;
                break;
            case VerticalAnchor.CenterPercentFromBottom:
                newPos.y = bottomLeft.y + (topRight.y - bottomLeft.y) * verticalPercent;
                break;
        }

        transform.position = newPos;
    }

    private float GetWorldWidth(Transform t)
    {
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.x : t.localScale.x;
    }

    private float GetWorldHeight(Transform t)
    {
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.y : t.localScale.y;
    }
}
