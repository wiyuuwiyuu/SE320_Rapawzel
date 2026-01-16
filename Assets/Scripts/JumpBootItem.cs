using UnityEngine;
using UnityEngine.UI;

public class JumpBootItem : MonoBehaviour
{
    public Player _player;
    public Text uiText;
    void Start()
    {
  
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D Player)
    {
        uiText.gameObject.SetActive(true);
        uiText.text = "Picked up: " + gameObject.name +   "\n" + "You can now use Double Jump!" ;   
        
        _player.gotJumpBoots = true;
        Destroy(this.gameObject);
    }
}
