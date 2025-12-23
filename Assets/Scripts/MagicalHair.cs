using UnityEngine;

public class MagicalHair : MonoBehaviour
{
    public Player _player;
    void Start()
    {
  
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D Player)
    {
        _player.gotMagicHair = true;
        Destroy(this.gameObject);
    }
}
