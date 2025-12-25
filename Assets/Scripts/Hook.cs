using UnityEngine;

public class Hook : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private DistanceJoint2D _distanceJoint2D;
    private Rigidbody2D _rigidbody2D;
    private Player _player;
    
    public float releaseBoost = 0.6f;
    public float hookRange = 10f;
    public LayerMask layerMask;  //tutmasını isteidğimiz layer inspectordan verilcek
    
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _distanceJoint2D = GetComponent<DistanceJoint2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _player=GetComponent<Player>();
        
        _lineRenderer.enabled = false;
        _distanceJoint2D.enabled = false; 
        
        _distanceJoint2D.autoConfigureDistance = false;
        
        
    }

    
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.L) && _player.gotMagicHair)
        {
            if (!_distanceJoint2D.enabled)
            {
                ShootHook();
            }
        }

        if (Input.GetKeyUp(KeyCode.L) || _player.checkGrounded || _player.nextToWall)
        {
            if (_distanceJoint2D.enabled)
            {
                ReleaseWithMomentum();
                _player.doubleJump = true;
            }
        }

        UpdateRope();
       
        
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
    
    public void ShootHook()
    {
        if (_player == null) return;

        int dir = _player.facingDirection;   //1=sağ -1=sol
        //Vector2 shootDirection=new Vector2(dir,1f).normalized;  //45derece
        Vector2 shootDirection=new Vector2(dir,1.7f).normalized; //60derece
        
        Debug.DrawRay(transform.position, shootDirection * hookRange, Color.yellow, 1f);


        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            shootDirection,
            hookRange,
            layerMask
        );

        if (!hit) return;
        if (hit.normal.y > -0.5f)
            return;
        
        Rigidbody2D hitRb = hit.collider.attachedRigidbody;
        if (hitRb == null) return;
        

        _distanceJoint2D.connectedBody = hitRb;
        _distanceJoint2D.connectedAnchor = hitRb.transform.InverseTransformPoint(hit.point);

        _distanceJoint2D.distance = Vector2.Distance(transform.position, hit.point);

        _distanceJoint2D.enabled = true;

        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, hit.point);
        


    }

    private void UpdateRope()
    {
        
        if (_lineRenderer == null) return;
        if (_distanceJoint2D == null) return;
        if (!_distanceJoint2D.enabled) return;
        if (_distanceJoint2D.connectedBody == null) return;

        Vector2 worldAnchor = _distanceJoint2D.connectedBody.transform.TransformPoint(_distanceJoint2D.connectedAnchor);
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, worldAnchor);
    }
}
