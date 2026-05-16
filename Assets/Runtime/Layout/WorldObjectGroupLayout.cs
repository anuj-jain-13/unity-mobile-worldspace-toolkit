using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic world-space child layout component.
/// Works for any group of objects (tubes, add-ons, icons) in 2D space.
/// Automatically spaces children horizontally or vertically, with optional SpriteRenderer size awareness.
/// </summary>
[ExecuteAlways]
public class WorldObjectGroupLayout : MonoBehaviour
{
    [Header("Layout Settings")]
    [Tooltip("True = horizontal layout, False = vertical layout.")]
    [SerializeField] private bool layoutHorizontal = true;

    [Tooltip("Minimum spacing between children (world units).")]
    [SerializeField] private float minSpacing = 1.0f;

    [Tooltip("Maximum spacing between children (world units).")]
    [SerializeField] private float maxSpacing = 3.0f;

    [Range(0f, 0.4f)]
    [Tooltip("Percentage of visible width/height to reserve as margin.")]
    [SerializeField] private float marginPercent = 0.1f;

    [Header("Size Mode")]
    [Tooltip("If enabled, spacing will consider SpriteRenderer.size (local units) instead of just placing centers evenly.")]
    [SerializeField] private bool useSpriteRendererSize = false;

    public float TotalLayoutWidth { get; private set; }

    private void Start()
    {
        UpdateLayout();
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
        if (!Application.isPlaying)
            UpdateLayout();
    }
#endif

    /// <summary>
    /// Lays out all active children based on current settings and ResolutionManager.
    /// </summary>
    public void UpdateLayout()
    {
        if (ResolutionManager.Instance == null) return;

        // Collect active children and measure size if needed
        var activeChildren = new List<(Transform child, float size)>();
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;

            float size = 1f; // Default fallback
            if (useSpriteRendererSize)
            {
                var sr = child.GetComponent<SpriteRenderer>();
                if (sr != null)
                    size = layoutHorizontal ? sr.size.x : sr.size.y;
            }

            activeChildren.Add((child, size));
        }

        int count = activeChildren.Count;
        if (count == 0) { TotalLayoutWidth = 0f; return; }

        // Calculate available world space once
        float available = layoutHorizontal
            ? ResolutionManager.Instance.VisibleWorldWidth * (1f - marginPercent * 2f)
            : ResolutionManager.Instance.VisibleWorldHeight * (1f - marginPercent * 2f);

        // Choose layout mode
        if (!useSpriteRendererSize)
            LayoutPivotBased(activeChildren, available);
        else
            LayoutWidthBased(activeChildren, available);
    }

    /// <summary>
    /// Layout based on evenly spaced centers.
    /// </summary>
    private void LayoutPivotBased(List<(Transform child, float size)> children, float available)
    {
        float rawSpacing = (children.Count > 1) ? available / (children.Count - 1) : 0f;
        float spacing = Mathf.Clamp(rawSpacing, minSpacing, maxSpacing);

        TotalLayoutWidth = spacing * (children.Count - 1);
        float startOffset = -TotalLayoutWidth / 2f;

        for (int i = 0; i < children.Count; i++)
        {
            Vector3 pos = children[i].child.localPosition;
            if (layoutHorizontal)
                pos.x = startOffset + (i * spacing);
            else
                pos.y = startOffset + (i * spacing);

            children[i].child.localPosition = pos;
        }
    }

    /// <summary>
    /// Layout based on SpriteRenderer.size (edge-to-edge spacing).
    /// </summary>
    private void LayoutWidthBased(List<(Transform child, float size)> children, float available)
    {
        float totalChildSize = 0f;
        foreach (var (_, size) in children)
            totalChildSize += size;

        float rawSpacing = (children.Count > 1) ? (available - totalChildSize) / (children.Count - 1) : 0f;
        float spacing = Mathf.Clamp(rawSpacing, minSpacing, maxSpacing);

        TotalLayoutWidth = totalChildSize + spacing * (children.Count - 1);
        float startOffset = -TotalLayoutWidth / 2f;

        float cursor = startOffset;
        foreach (var (child, size) in children)
        {
            Vector3 pos = child.localPosition;
            if (layoutHorizontal)
            {
                pos.x = cursor + size / 2f;
                cursor += size + spacing;
            }
            else
            {
                pos.y = cursor + size / 2f;
                cursor += size + spacing;
            }

            child.localPosition = pos;
        }
    }

}
