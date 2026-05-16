using DG.Tweening;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteGlowHighlighter : MonoBehaviour
{
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private float intensity = 0.35f;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private float idleDelay = 5f;

    private SpriteRenderer spriteRenderer;
    private Coroutine loopRoutine;
    private Tween activeTween;
    private Color baseColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        baseColor = spriteRenderer.color;
        if (playOnStart)
            Play();
    }

    public void Play()
    {
        Stop(); // ensure clean state
        loopRoutine = StartCoroutine(GlowLoop());


        //        activeTween = spriteRenderer
        //            .DOColor(highlightColor, duration)
        //            .SetEase(Ease.InOutSine)
        //            .SetLoops(-1, LoopType.Yoyo);
    }

    private IEnumerator GlowLoop()
    {
        while (true)
        {
            // Wait before each glow cycle
            yield return new WaitForSeconds(idleDelay);

            // Glow fade in
            Color highlightColor = baseColor * (1f - intensity);
            activeTween = spriteRenderer
                .DOColor(highlightColor, duration)
                .SetEase(Ease.InOutSine);

            yield return activeTween.WaitForCompletion();

            // Glow fade out
            activeTween = spriteRenderer
                .DOColor(baseColor, duration)
                .SetEase(Ease.InOutSine);

            yield return activeTween.WaitForCompletion();
        }
    }

    public void Stop()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }

        activeTween?.Kill();
        activeTween = null;

        if (spriteRenderer != null)
            spriteRenderer.color = baseColor;
    }

    //private void OnEnable() => Play();

    private void OnDisable() => Stop();

    private void OnDestroy() => Stop();
}