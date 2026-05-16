using System;
using UnityEngine;

public class Button2D : MonoBehaviour {

    [SerializeField] private string actionKey;  //Used to uniquely idendify the button...
    [SerializeField] private int keyIndex;  //Used to uniquely idendify same group of 'actionKey' button...
    [SerializeField] private bool interactable = true;  //This field maintains button interactibility...
    [SerializeField] private bool enableEventSystem = true; //If Event system is enabled then only OnMouseDown method will trigger an Event...
    private float dragThreshold = 10f;

    private Vector3 mouseDownPosition;
    private bool isDraggingBeyondThreshold;
    private Action tapListener;

    #region System Methods
    private void Awake() {
        registerEvents();
    }

    private void OnDestroy() {
        deregisterEvents();
    }

    public void SetKey(int idx)
    {
        keyIndex = idx;
    }

    //private void OnMouseDown() {
    //    if (interactable) {
    //        if (enableEventSystem) {
    //            throwAnEvent();
    //        } else {
    //            onButtonTapped(actionKey, keyIndex);
    //        }
    //    }
    //}

    private void OnMouseDown()
    {
        if (!interactable) return;

        mouseDownPosition = Input.mousePosition;
        isDraggingBeyondThreshold = false;
    }

    private void OnMouseDrag()
    {
        if (!interactable) return;

        float dragDistance = Vector3.Distance(mouseDownPosition, Input.mousePosition);
        if (dragDistance > dragThreshold)
        {
            isDraggingBeyondThreshold = true;
        }
    }

    private void OnMouseUp()
    {
        if (!interactable || isDraggingBeyondThreshold) return;

        if (enableEventSystem)
        {
            throwAnEvent();
        }
        else
        {
            onButtonTapped(actionKey, keyIndex);
        }
    }

    #endregion

    #region Button UI Methods
    public virtual void onButtonTapped(string tapKey, int keyIdx) {
        //Use this method to get the user tap in the absense of enableEventSystem variable...
        //Override this method in the subclass to get the tap...
        triggerTapEvent();
    }

    public void SetTapCallback(Action tapCB) {
        tapListener = tapCB;
    }

    protected void triggerTapEvent() {
        tapListener?.Invoke();
    }

    public void toggleInteraction(bool enable) {
        interactable = enable;
    }

    private void toggleButtonInteraction(GameObject senderGO, object key, object enable) {
        if (key.ToString() == actionKey) {
            interactable = (bool)enable;
        }
    }

    public void toggleVisibility(bool show) {
        gameObject.SetActive(show);
    }

    private void toggleButtonVisibility(GameObject senderGO, object key, object show) {
        if (key.ToString() == actionKey) {
            gameObject.SetActive((bool)show);
        }
    }
    #endregion

    #region Event Methods
    private void throwAnEvent() {
        EventManager.triggerEvent(AppEventsId.Button2DOnMouseDown, gameObject, actionKey, keyIndex);
    }

    private void registerEvents() {
        if (enableEventSystem) {
            EventManager.startListening(AppEventsId.Button2DInteraction, toggleButtonInteraction);
            EventManager.startListening(AppEventsId.Button2DVisibility, toggleButtonVisibility);
        }
    }

    private void deregisterEvents() {
        if (enableEventSystem) {
            EventManager.stopListening(AppEventsId.Button2DInteraction, toggleButtonInteraction);
            EventManager.stopListening(AppEventsId.Button2DVisibility, toggleButtonVisibility);
        }
    }
    #endregion
}
