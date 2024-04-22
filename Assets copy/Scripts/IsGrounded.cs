using UnityEngine;
public class IsGrounded : MonoBehaviour
{
    [SerializeField] private float rayLength = 0.3f;
    [SerializeField] private float rayRadius = 0.3f;
    [SerializeField] private LayerMask rayLayer ;
    private const float RAY_RADIUS = 0.2f;
    
    private void FixedUpdate()
    {
        Is_Grounded = IsTouchingGround(rayRadius) ||
                     IsTouchingGround(-rayRadius);
    }

    private bool IsTouchingGround(float radiusX)
    {
        Vector3 origin = transform.position;
        origin.x += radiusX;

        RaycastHit2D hit =
            Physics2D.Raycast(origin, Vector2.down, rayLength, rayLayer);

        Debug.DrawRay(origin, Vector3.down * rayLength, Color.magenta);

        return hit.collider != null;
    }

    public bool Is_Grounded { get; private set; }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        pos.y -= rayLength;
        Gizmos.color = Is_Grounded ? Color.cyan : Color.yellow;
        Gizmos.DrawSphere(transform.position, RAY_RADIUS);
    }
    
}
