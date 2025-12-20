using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLevel1()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void loadLevel2()
    {
        //SceneManager.LoadScene("lvl2");
    }
    public void loadLevel3()
    {
       // SceneManager.LoadScene("lvl3");
    }
}
