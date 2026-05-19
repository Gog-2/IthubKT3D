using UnityEngine;

public class TargetObject : MonoBehaviour
{
    private Rigidbody rb;
    public bool isHovered = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PushAway(Vector3 direction, float force)
    {
        if (rb != null)
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
    
    private void OnDrawGizmos()
    {
        if (isHovered)
        {
            Gizmos.color = Color.cyan;
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                Gizmos.DrawWireCube(col.bounds.center, col.bounds.size * 1.1f);
            }
            else
            {
                Gizmos.DrawWireCube(transform.position, transform.localScale * 1.1f);
            }
            Gizmos.DrawIcon(transform.position + Vector3.up, "TargetIcon", true);
        }
    }
}