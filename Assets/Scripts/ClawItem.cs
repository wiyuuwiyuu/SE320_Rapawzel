using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClawItem : MonoBehaviour
{
    public Player _player;
    public Text uiText;
    void OnTriggerEnter2D(Collider2D Player)
    {
        uiText.gameObject.SetActive(true);
        uiText.text = "Picked up: " + gameObject.name;
        
        _player.gotClaws = true;
        Destroy(this.gameObject);
    }
}
