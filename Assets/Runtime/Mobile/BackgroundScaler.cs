using UnityEngine;

public class BackgroundScaler : MonoBehaviour {

    void Start() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        BoxCollider2D bc2d = GetComponent<BoxCollider2D>();
        Transform t = transform;
        float spriteWidth, spriteHeight;
        if (sr != null) {
            spriteWidth = sr.bounds.size.x;
            spriteHeight = sr.bounds.size.y;
        } else {
            spriteWidth = bc2d.size.x;
            spriteHeight = bc2d.size.y;
        }
        Rect screenRect = Camera.main.GetScreenWorldRect();        
        t.localScale = new Vector3(screenRect.width / spriteWidth, screenRect.height / spriteHeight, 1);
        Debug.Log($"BackgroundScaler({gameObject.name})...WorldScreenRect : {screenRect}, Sprite-Width : {spriteWidth}, Sprite-Height : {spriteHeight}");
    }
}