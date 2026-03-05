using UnityEngine;

public class ButtonFalseCol : ButtonParent
{
    [SerializeField]private MeshCollider _collider;
    protected override void TriggerEnter()
    {
        _collider.enabled = false;
    } 
}
