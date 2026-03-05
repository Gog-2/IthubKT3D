using My.Script._3D;
using UnityEngine;

public abstract class ButtonParent : MonoBehaviour
{
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        ICanBeActiveted _canBeActiveted = other.GetComponent<ICanBeActiveted>();
        if (_canBeActiveted != null) TriggerEnter();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        ICanBeActiveted _canBeActiveted = other.GetComponent<ICanBeActiveted>();
        if (_canBeActiveted != null) TriggerExit();
    }

    protected abstract void TriggerEnter();

    protected virtual void TriggerExit() { }
}
