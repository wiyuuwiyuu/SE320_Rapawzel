using UnityEngine;

public class JumpBootItem : MonoBehaviour
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
        _player.gotJumpBoots = true;
        Destroy(this.gameObject);
    }
}
