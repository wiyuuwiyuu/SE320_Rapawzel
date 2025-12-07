using UnityEngine;

public class Node : MonoBehaviour
{
    private GameObject cat;
    private Hook hookScript;
    //Hook->Hook.cs 
    private Node node;
    
    
    void Start()
    {
        cat = GameObject.FindGameObjectWithTag("Player");
        hookScript = cat.GetComponent<Hook>();
        node = null;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        node = this;
        hookScript.SelectedNode(node);
        
    }

    public void OnMouseUp()
    {
        node = null;
        hookScript.DeselectedNode();
    }
    
}
