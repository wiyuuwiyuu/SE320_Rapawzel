using UnityEngine;
using UnityEngine.UI;

public class DashBoot : MonoBehaviour
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
        uiText.text = "Picked up: " + gameObject.name;
        
        _player.gotDashBoots = true;
        Destroy(this.gameObject);
    }
}
