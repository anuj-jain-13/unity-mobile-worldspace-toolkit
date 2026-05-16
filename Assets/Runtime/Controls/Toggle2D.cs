using UnityEngine;
using System;

public class Toggle2D : MonoBehaviour
{
    [Header("Toggle Mode")]
    [SerializeField] private bool useColorMode = true; // false = GameObject swap

    [Header("Color Mode")]
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private Color onColor = Color.white;
    [SerializeField] private Color offColor = Color.gray;

    [Header("UI Style - GameObject Mode")]
    [SerializeField] private GameObject onStateObject;
    [SerializeField] private GameObject offStateObject;

    public event Action<bool> OnToggleChanged;
    private bool isOn;

    // Initializes state without firing events (for saved values).
    //public void Initialize(bool value)
    //{
    //    isOn = value;
    //    updateVisuals();
    //    // no event here
    //}

    private void OnMouseDown()
    {
        SetState(!isOn);
        OnToggleChanged?.Invoke(isOn);
    }

    public void SetState(bool value)
    {
        //if (isOn == value) return; // ✅ skip redundant updates

        isOn = value;
        updateVisuals();
    }

    private void updateVisuals()
    {
        if (useColorMode)
        {
            if (iconRenderer != null)
                iconRenderer.color = isOn ? onColor : offColor;
        }
        else
        {
            if (onStateObject != null) onStateObject.SetActive(isOn);
            if (offStateObject != null) offStateObject.SetActive(!isOn);
        }
    }

    public bool GetState() => isOn;
}