using UnityEngine;

public class jumpBoost : MonoBehaviour
    
{
    public SpriteRenderer _spriteRenderer;
    
    void Start()
    {   
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, 1);
    }

    
    void Update()
    {   _spriteRenderer.size *= new Vector2(1.005f, 1.005f);
        _spriteRenderer.color *= new Color(1f, 1f, 1f, 0.99f);
    }
}
