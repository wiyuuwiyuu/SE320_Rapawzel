using Unity.VisualScripting;
using UnityEngine;

public class ClawItem : MonoBehaviour
{
    public Player _player;
    void OnTriggerEnter2D(Collider2D Player)
    {
        _player.gotClaws = true;
        Destroy(this.gameObject);
    }
}
