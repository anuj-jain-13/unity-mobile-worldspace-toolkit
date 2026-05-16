using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize;

    private Queue<GameObject> pool;
    private int currentIdx = 0;

    private void Awake() {
        pool = new Queue<GameObject>();
        for (; currentIdx < initialSize; currentIdx++) {
            GameObject newGO = Instantiate(prefab, transform);
            setCustomName(newGO);
            newGO.SetActive(false);
            pool.Enqueue(newGO);
        }
    }

    public GameObject GetObjectFromPool() {
        GameObject obj;
        if (pool.Count > 0) {
            obj = pool.Dequeue();
        } else {
            obj = Instantiate(prefab, transform);
            setCustomName(obj);
            currentIdx++;
        }
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj) {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    private void setCustomName(GameObject newGO) {
        newGO.name = "[" + prefab.name + currentIdx + "]";
    }
}
