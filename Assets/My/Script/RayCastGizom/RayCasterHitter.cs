using UnityEngine;
using UnityEngine.InputSystem;

public class RayCasterHitter : MonoBehaviour
{
    [SerializeField] private float rayDistance = 15f;
    [SerializeField] private float pushForce = 12f;
    [SerializeField] private Transform ShootPos;

    private TargetObject currentTarget;

    void FixedUpdate()
    {
        Ray ray = new Ray(ShootPos.position, ShootPos.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            TargetObject target = hit.collider.GetComponent<TargetObject>();
            if (target != null)
            {
                Debug.DrawRay(ShootPos.position, ShootPos.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                if (currentTarget != target)
                {
                    if (currentTarget != null) currentTarget.isHovered = false;
                    currentTarget = target;
                    currentTarget.isHovered = true;
                }
            }
            else { ClearTarget(); }
        }
        else
        {
            Debug.DrawRay(ShootPos.position, ShootPos.TransformDirection(Vector3.forward) * hit.distance, Color.white);
            ClearTarget();
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentTarget != null)
        {
            Vector3 dir = currentTarget.transform.position - ShootPos.position;
            currentTarget.PushAway(dir, pushForce);
        }
    }

    void ClearTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.isHovered = false;
            currentTarget = null;
        }
    }
}