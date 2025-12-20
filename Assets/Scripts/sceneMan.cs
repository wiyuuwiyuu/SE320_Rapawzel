using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneMan : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("level picker");
    }
}
