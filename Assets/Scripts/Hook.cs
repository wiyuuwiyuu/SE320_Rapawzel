using UnityEngine;

public class Hook : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private DistanceJoint2D _distanceJoint2D;
    private Rigidbody2D _rigidbody2D;
    private Node _selectedNode;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _distanceJoint2D = GetComponent<DistanceJoint2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _lineRenderer.enabled = false;
        _distanceJoint2D.enabled = false; 
        _selectedNode = null;
    }

    // Update is called once per frame
    void Update()
    {
        NodeBehaviour(); 
        
    }

    public void SelectedNode(Node node)
    {
        _selectedNode = node;
    }

    public void DeselectedNode()
    {
        _selectedNode = null;
    }

    private void NodeBehaviour()
    {
        if (_selectedNode == null)
        {
            _lineRenderer.enabled = false;
            _distanceJoint2D.enabled = false;
            return;
        }
        _lineRenderer.enabled = true;
        _distanceJoint2D.enabled = true;
        
        _distanceJoint2D.connectedBody = _selectedNode.GetComponent<Rigidbody2D>();

        if (_selectedNode != null)
        {
            _lineRenderer.SetPosition(0,transform.position); 
            _lineRenderer.SetPosition(1,_selectedNode.transform.position);
        }
         
    }
}
