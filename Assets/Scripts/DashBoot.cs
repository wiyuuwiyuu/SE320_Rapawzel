using UnityEngine;

public class DashBoot : MonoBehaviour
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
        _player.gotDashBoots = true;
        Destroy(this.gameObject);
    }
}
