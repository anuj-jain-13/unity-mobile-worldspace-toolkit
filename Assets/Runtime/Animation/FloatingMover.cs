using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class FloatingMover : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float delayBeforeStart = 0f;
    [SerializeField] private float moveDuration = 0.6f;
    [SerializeField] private Ease moveEase = Ease.OutCubic;

    [Header("Arc Path ControlPoints WorldSpace")]
    [SerializeField] private bool useArcPath = false;
    [SerializeField] private List<Vector3> customControlPoints = new List<Vector3>();
    
    [Header("Scale Animation")]
    [SerializeField] private bool enableScale = false;
    [SerializeField] private Vector3 startScale = Vector3.one;
    [SerializeField] private Vector3 endScale = Vector3.one;
    [SerializeField] private float scaleDuration = 0.4f;

    [Header("Fade Animation")]
    [SerializeField] private bool enableFade = false;
    [SerializeField] private float fadeInTime = 0.2f;
    [SerializeField] private float fadeOutTime = 0.3f;

    [Header("Auto Cleanup")]
    [SerializeField] private bool autoDestroyOnComplete = false;

    private SpriteRenderer spriteRenderer;
    private TextMeshPro tmp;

    private Action onComplete;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tmp = GetComponent<TextMeshPro>();
    }

    public void SetTextVal(string valToDisp)
    {
        if (tmp != null)
            tmp.text = valToDisp;
    }

    public void Play(Vector3 startPos, Vector3 targetPos, Action onCompleteCallback = null)
    {
        onComplete = onCompleteCallback;

        transform.position = startPos;

        if (enableFade)
            SetAlpha(0f);

        if (enableScale)
            transform.localScale = startScale;

        Sequence seq = DOTween.Sequence();

        if (delayBeforeStart > 0f)
            seq.AppendInterval(delayBeforeStart);

        // Move
        if (useArcPath && customControlPoints != null && customControlPoints.Count > 0)
        {
            List<Vector3> fullPath = new List<Vector3>();
            fullPath.Add(startPos);
            fullPath.AddRange(customControlPoints);
            fullPath.Add(targetPos);

            seq.Append(transform.DOPath(fullPath.ToArray(), moveDuration, PathType.CatmullRom)
                .SetEase(moveEase));
        }
        else
        {
            seq.Append(transform.DOMove(targetPos, moveDuration).SetEase(moveEase));
        }

        // Scale
        if (enableScale)
        {
            seq.Join(transform.DOScale(endScale, scaleDuration).SetEase(Ease.OutBack));
        }

        // Fade
        if (enableFade)
        {
            seq.Join(DOFadeTo(1f, fadeInTime)); // Fade in
            seq.Append(DOFadeTo(0f, fadeOutTime)); // Fade out
        }

        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
            if (autoDestroyOnComplete)
                Destroy(gameObject);
        });
    }

    private Tween DOFadeTo(float alpha, float duration)
    {
        if (spriteRenderer != null)
            return spriteRenderer.DOFade(alpha, duration);

        if (tmp != null)
            return tmp.DOFade(alpha, duration);

        return null;
    }

    private void SetAlpha(float a)
    {
        if (spriteRenderer != null)
        {
            var c = spriteRenderer.color;
            spriteRenderer.color = new Color(c.r, c.g, c.b, a);
        }
        else if (tmp != null)
        {
            var c = tmp.color;
            tmp.color = new Color(c.r, c.g, c.b, a);
        }
    }
}
