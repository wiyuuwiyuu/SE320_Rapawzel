using UnityEngine.UI;
using UnityEngine;

public class MagicalHair : MonoBehaviour
{
    public Player _player;
    public Text uiText;
    void OnTriggerEnter2D(Collider2D Player)
    {
        
        if (uiText != null)
        {
            uiText.gameObject.SetActive(true);
            uiText.text = "Picked up: " + gameObject.name;
                
            _player.gotMagicHair = true;
            Destroy(this.gameObject);
            
        }
        
        
    }
}
