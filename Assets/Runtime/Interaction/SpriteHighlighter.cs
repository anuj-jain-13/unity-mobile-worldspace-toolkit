using DG.Tweening;
using UnityEngine;
using System.Collections;

/// <summary>
/// Adds optional Glow and/or Scale highlight animations to a SpriteRenderer.
/// Both effects can loop with idle delays between cycles.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public class SpriteHighlighter : MonoBehaviour
{
    [Header("Playback")]
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private float idleDelay = 4f;

    [Header("Glow Effect")]
    [SerializeField] private bool enableGlow = true;
    [SerializeField, Range(0f, 1f)] private float glowIntensity = 0.3f;
    [SerializeField] private float glowDuration = 0.8f;
    [SerializeField] private Ease glowEase = Ease.InOutSine;

    [Header("Scale Effect")]
    [SerializeField] private bool enableScale = false;
    [SerializeField, Range(0f, 2f)] private float scaleIntensity = 0.2f;
    [SerializeField] private float scaleDuration = 0.6f;
    [SerializeField] private Ease scaleEase = Ease.InOutSine;

    private SpriteRenderer spriteRenderer;
    private Color baseColor;
    private Vector3 baseScale;

    private Coroutine loopRoutine;
    private Tween glowTween;
    private Tween scaleTween;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
        baseScale = transform.localScale;
    }

    private void Start()
    {
        if (playOnStart)
            Play();
    }

    public void Play()
    {
        Stop(); // clean state before starting
        loopRoutine = StartCoroutine(HighlightLoop());
    }

    private IEnumerator HighlightLoop()
    {
        while (true)
        {
            // Wait between highlight cycles
            yield return new WaitForSeconds(idleDelay);

            // --- Glow animation ---
            if (enableGlow)
            {
                Color highlightColor = baseColor * (1f - glowIntensity);
                glowTween = spriteRenderer
                    .DOColor(highlightColor, glowDuration)
                    .SetEase(glowEase);
            }

            // --- Scale animation ---
            if (enableScale)
            {
                Vector3 targetScale = baseScale * (1f + scaleIntensity);
                scaleTween = transform
                    .DOScale(targetScale, scaleDuration)
                    .SetEase(scaleEase);
            }

            // Wait for both tweens to finish (whichever lasts longer)
            float maxDuration = Mathf.Max(glowDuration, scaleDuration);
            yield return new WaitForSeconds(maxDuration);

            // --- Return to original state ---
            if (enableGlow)
            {
                glowTween = spriteRenderer
                    .DOColor(baseColor, glowDuration)
                    .SetEase(glowEase);
            }

            if (enableScale)
            {
                scaleTween = transform
                    .DOScale(baseScale, scaleDuration)
                    .SetEase(scaleEase);
            }

            yield return new WaitForSeconds(maxDuration);
        }
    }

    public void Stop()
    {
        // Stop loop
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }

        // Stop tweens
        glowTween?.Kill();
        scaleTween?.Kill();

        // Reset visuals
        spriteRenderer.color = baseColor;
        transform.localScale = baseScale;
    }

    private void OnDisable() => Stop();
    private void OnDestroy() => Stop();
}
