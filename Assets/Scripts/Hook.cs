using UnityEngine;

public class Hook : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private DistanceJoint2D _distanceJoint2D;
    private Rigidbody2D _rigidbody2D;
    private Node _selectedNode;
    public float releaseBoost = 0.6f;
    private Player _player;
    
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _distanceJoint2D = GetComponent<DistanceJoint2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _lineRenderer.enabled = false;
        _distanceJoint2D.enabled = false; 
        _selectedNode = null;
        _player=GetComponent<Player>();
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
        ReleaseWithMomentum();
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
    
    public bool IsHooked()
    {
        return _distanceJoint2D != null && _distanceJoint2D.enabled;
    }

    public void ReleaseWithMomentum()
    {
        if (_distanceJoint2D.enabled)
        {
            Vector2 releaseVelocity = _rigidbody2D.linearVelocity;
            
            Vector2 boost = new Vector2(releaseVelocity.x, releaseVelocity.y * 0.3f);

            _rigidbody2D.AddForce(boost * releaseBoost, ForceMode2D.Impulse);

            _distanceJoint2D.enabled = false;
            _lineRenderer.enabled = false;

            _distanceJoint2D.distance = 0f;

        }

        if (_player != null)
        {
            _player.NotifyHookReleased();
        }
    }
}
