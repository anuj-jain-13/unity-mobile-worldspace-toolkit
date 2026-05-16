using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class Slider2D : MonoBehaviour
{
    [SerializeField] private Transform handle;
    [SerializeField] private Transform minPoint;
    [SerializeField] private Transform maxPoint;

    public event Action<float> OnValueChanged;

    private float currentValue;
    private float cachedUnmuteValue = 1f;

    private bool isDragging = false;
    private int activeTouchId = -1;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        isDragging = true;
        updateHandlePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wp.z = 0f;
            updateHandlePosition(wp, true);
        }
#endif

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                Vector3 wp = Camera.main.ScreenToWorldPoint(t.position);
                wp.z = 0f;

                // Start drag if touch began on this slider
                if (t.phase == TouchPhase.Began)
                {
                    if (col == Physics2D.OverlapPoint(wp))
                    {
                        activeTouchId = t.fingerId;
                        isDragging = true;
                        updateHandlePosition(wp, true);
                    }
                }
                else if (isDragging && t.fingerId == activeTouchId)
                {
                    if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                    {
                        updateHandlePosition(wp, true);
                    }
                    else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        isDragging = false;
                        activeTouchId = -1;
                    }
                }
            }
        }
    }

    private void updateHandlePosition(Vector3 worldPos, bool notify)
    {
        float t = Mathf.InverseLerp(minPoint.position.x, maxPoint.position.x, worldPos.x);
        SetValueInternal(Mathf.Clamp01(t), notify);
    }

    private void SetValueInternal(float value, bool notify)
    {
        currentValue = Mathf.Clamp01(value);

        if (currentValue > 0f)
            cachedUnmuteValue = currentValue;

        handle.position = Vector3.Lerp(minPoint.position, maxPoint.position, currentValue);

        if (notify)
            OnValueChanged?.Invoke(currentValue);
    }

    public float GetValue() => currentValue;

    public void SetValue(float value) => SetValueInternal(value, false);
    public void SetValueAndNotify(float value) => SetValueInternal(value, true);

    public void Mute() => SetValueAndNotify(0f);

    public void Unmute()
    {
        float restore = (cachedUnmuteValue > 0f) ? cachedUnmuteValue : 1f;
        SetValueAndNotify(restore);
    }

    public bool IsMuted => currentValue <= 0f;
}