using UnityEngine;

[ExecuteAlways]
public class WorldObjectAutoScaler : MonoBehaviour
{
    [Tooltip("Minimum allowed scale factor.")]
    [SerializeField] private float minScale = 0.5f;

    [Tooltip("Maximum allowed scale factor.")]
    [SerializeField] private float maxScale = 2.0f;

    [SerializeField] private Vector3 baseScale;

    public float FinalScaleFactor { get; private set; }


    private void Awake()
    {
        ApplyScaling();
    }

#if UNITY_EDITOR
    void LateUpdate()
    {
        if (!Application.isPlaying)
            transform.localScale = baseScale;
            //ApplyScaling();
    }
#endif

    private void ApplyScaling()
    {
        if (ResolutionManager.Instance == null) return;

        // Get scale ratio based on height
        FinalScaleFactor = ResolutionManager.Instance.HeightScaleMultiplier;

        // Clamp scale factor
        FinalScaleFactor = Mathf.Clamp(FinalScaleFactor, minScale, maxScale);

        // Apply uniform scale
        transform.localScale = baseScale * FinalScaleFactor;
    }
}
