using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    
    public float delay = 1f;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected, loading...");
            Invoke(nameof(LoadNextLevel), delay);
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Scenes/level2");
    }
}
