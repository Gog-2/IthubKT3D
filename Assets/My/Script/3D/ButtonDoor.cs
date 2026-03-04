using UnityEngine;

namespace My.Script._3D
{
    public class ButtonDoor : ButtonParent
    {
        [SerializeField]private GameObject _door;
        [SerializeField]private int _canBeActiveLayer;

        protected override void OnTriggerEnter(Collider other)
        {
            CanBeActiveted _canBeActiveted = other.GetComponent<CanBeActiveted>();
            if (_canBeActiveted != null && _canBeActiveted.Layer == _canBeActiveLayer) TriggerEnter();
        }

        protected override void OnTriggerExit(Collider other)
        {
            CanBeActiveted _canBeActiveted = other.GetComponent<CanBeActiveted>();
            if (_canBeActiveted != null && _canBeActiveted.Layer == _canBeActiveLayer)  TriggerExit();
        }
        protected override void TriggerEnter()
        {
            _door.SetActive(true);
        }

        protected override void TriggerExit()
        {
            _door.SetActive(false);
        }
    }
}