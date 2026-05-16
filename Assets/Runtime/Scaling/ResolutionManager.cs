using UnityEngine;

[ExecuteAlways]
[DefaultExecutionOrder(-100)]
public class ResolutionManager : MonoBehaviour
{
    public static ResolutionManager Instance { get; private set; }

    [Header("Reference Settings")]
    [Tooltip("Reference width in world units for your design aspect ratio.")]
    [SerializeField] private float referenceWidth = 16f;

    [Tooltip("Reference height in world units for your design aspect ratio.")]
    [SerializeField] private float referenceHeight = 9f;

    [Tooltip("Orthographic size at the reference aspect ratio (usually referenceHeight / 2).")]
    [SerializeField] private float baseOrthographicSize = 4.5f;

    [Header("Runtime Values (ReadOnly)")]
    [ReadOnly, SerializeField] private string orientation;
    [ReadOnly, SerializeField] private float screenAspect;
    [ReadOnly, SerializeField] private float referenceAspect;
    [ReadOnly, SerializeField] private float visibleWorldWidth;
    [ReadOnly, SerializeField] private float visibleWorldHeight;
    [ReadOnly, SerializeField] private float widthScaleMultiplier;
    [ReadOnly, SerializeField] private float heightScaleMultiplier;
    [ReadOnly, SerializeField] private Vector2 safeAreaPadding;

    public string Orientation => orientation;
    public float ScreenAspect => screenAspect;
    public float ReferenceAspect => referenceAspect;
    public float VisibleWorldWidth => visibleWorldWidth;
    public float VisibleWorldHeight => visibleWorldHeight;
    public float ContentScaleMultiplier => widthScaleMultiplier;
    public float HeightScaleMultiplier => heightScaleMultiplier;
    public Vector2 SafeAreaPadding => safeAreaPadding;

    private Camera mainCam;

    void Awake()
    {
        Instance = this;
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null || !mainCam.orthographic)
        {
            Debug.LogError("ResolutionManager requires an Orthographic Camera in the scene!");
        }

        ApplyResolution();
    }

#if UNITY_EDITOR
    void Update()
    {

        if (!Application.isPlaying)
        {
            if (mainCam == null) mainCam = Camera.main;
            if (mainCam != null) ApplyResolution();
        }

    }
#endif

    void ApplyResolution()
    {
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        // Determine orientation
        orientation = (referenceWidth >= referenceHeight) ? "Landscape" : "Portrait";

        // Aspect ratios
        screenAspect = (Screen.height == 0) ? 1f : (float)Screen.width / Screen.height;
        referenceAspect = (referenceHeight == 0) ? 1f : referenceWidth / referenceHeight;

        // Maintain baseOrthographicSize at reference aspect
        float aspectRatioScale = (screenAspect <= 0f) ? 1f : (referenceAspect / screenAspect);
        mainCam.orthographicSize = baseOrthographicSize * aspectRatioScale;

        // Visible world dimensions
        visibleWorldHeight = mainCam.orthographicSize * 2f;
        visibleWorldWidth = visibleWorldHeight * screenAspect;

        // Scaling factor for world-space elements
        widthScaleMultiplier = (referenceWidth == 0f) ? 1f : (visibleWorldWidth / referenceWidth);
        heightScaleMultiplier = (referenceHeight == 0f) ? 1f : (visibleWorldHeight / referenceHeight);

        // Safe area padding
        safeAreaPadding = new Vector2(
            (visibleWorldWidth - referenceWidth) / 2f,
            (visibleWorldHeight - referenceHeight) / 2f
        );
    }

    public Vector2 GetTopRightWorldPos()
    {
        if (mainCam == null) mainCam = Camera.main;
        return mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));
    }

    public Vector2 GetBottomLeftWorldPos()
    {
        if (mainCam == null) mainCam = Camera.main;
        return mainCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
    }
}
