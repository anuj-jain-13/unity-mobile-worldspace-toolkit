using System;
using UnityEngine;
using System.Collections.Generic;

public static class EventManager {

    #region (No) Arg Methods
    private static Dictionary<string, Action<GameObject>> noArgDic = new Dictionary<string, Action<GameObject>>();

    public static void startListening(string eventName, Action<GameObject> listener) {
        //Debug.Log("<b><color=grey>EventManager.startListening()...EventName : " + eventName + "</color></b>");
        if (noArgDic.TryGetValue(eventName, out Action<GameObject> thisEvent)) {
            thisEvent += listener;
            noArgDic[eventName] = thisEvent;
        } else {
            noArgDic.Add(eventName, listener);
        }
    }

    public static void stopListening(string eventName, Action<GameObject> listener) {
        if (noArgDic.TryGetValue(eventName, out Action<GameObject> thisEvent)) {
            thisEvent -= listener;
            noArgDic[eventName] = thisEvent;
        }
    }

    public static void triggerEvent(string eventName, GameObject senderGO) {
        Debug.Log("EventManager.triggerEvent()...<b><color=red>EventName</color> : '" + eventName + "'</b>");
        if (noArgDic.TryGetValue(eventName, out Action<GameObject> thisEvent)) {
            thisEvent?.Invoke(senderGO);
        }
    }
    #endregion

    #region (object) Arg Methods
    private static Dictionary<string, Action<GameObject, object>> objectDic = new Dictionary<string, Action<GameObject, object>>();

    public static void startListening(string eventName, Action<GameObject, object> listener) {
        if (objectDic.TryGetValue(eventName, out Action<GameObject, object> thisEvent)) {
            thisEvent += listener;
            objectDic[eventName] = thisEvent;
        } else {
            objectDic.Add(eventName, listener);
        }
    }

    public static void stopListening(string eventName, Action<GameObject, object> listener) {
        if (objectDic.TryGetValue(eventName, out Action<GameObject, object> thisEvent)) {
            thisEvent -= listener;
            objectDic[eventName] = thisEvent;
        }
    }

    public static void triggerEvent(string eventName, GameObject senderGO, object val) {
        Debug.Log("EventManager.triggerEvent()...<b><color=red>EventName</color> : '" + eventName + "', <color=red>Param1</color> : '" + val + "'</b>");
        if (objectDic.TryGetValue(eventName, out Action<GameObject, object> thisEvent)) {
            thisEvent?.Invoke(senderGO, val);
        }
    }
    #endregion

    #region (object, object) Args Methods
    private static Dictionary<string, Action<GameObject, object, object>> objObjDic = new Dictionary<string, Action<GameObject, object, object>>();

    public static void startListening(string eventName, Action<GameObject, object, object> listener) {
        if (objObjDic.TryGetValue(eventName, out Action<GameObject, object, object> thisEvent)) {
            thisEvent += listener;
            objObjDic[eventName] = thisEvent;
        } else {
            objObjDic.Add(eventName, listener);
        }
    }

    public static void stopListening(string eventName, Action<GameObject, object, object> listener) {
        if (objObjDic.TryGetValue(eventName, out Action<GameObject, object, object> thisEvent)) {
            thisEvent -= listener;
            objObjDic[eventName] = thisEvent;
        }
    }

    public static void triggerEvent(string eventName, GameObject senderGO, object val1, object val2) {
        Debug.Log("EventManager.triggerEvent()...<b><color=red>EventName</color> : '" + eventName + "', <color=red>Param1</color> : '" + val1 + "', <color=red>Param2</color> : '" + val2 + "'</b>");
        if (objObjDic.TryGetValue(eventName, out Action<GameObject, object, object> thisEvent)) {
            thisEvent?.Invoke(senderGO, val1, val2);
        }
    }
    #endregion
}