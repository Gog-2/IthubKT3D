using My.Script._3D;
using UnityEngine;

public abstract class ButtonParent : MonoBehaviour
{
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        CanBeActiveted _canBeActiveted = other.GetComponent<CanBeActiveted>();
        if (_canBeActiveted != null) TriggerEnter();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        CanBeActiveted _canBeActiveted = other.GetComponent<CanBeActiveted>();
        if (_canBeActiveted != null) TriggerExit();
    }

    protected abstract void TriggerEnter();

    protected abstract void TriggerExit();

}
