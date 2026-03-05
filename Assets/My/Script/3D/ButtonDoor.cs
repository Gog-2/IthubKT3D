using UnityEngine;

namespace My.Script._3D
{
    public class ButtonDoor : ButtonParent
    {
        [SerializeField]private GameObject _door;
        [SerializeField]private int _canBeActiveLayer;

        protected override void OnTriggerEnter(Collider other)
        {
            ICanBeActiveted _canBeActiveted = other.GetComponent<ICanBeActiveted>();
            if (_canBeActiveted != null && _canBeActiveted.Layer == _canBeActiveLayer) TriggerEnter();
        }

        protected override void OnTriggerExit(Collider other)
        {
            ICanBeActiveted _canBeActiveted = other.GetComponent<ICanBeActiveted>();
            if (_canBeActiveted != null && _canBeActiveted.Layer == _canBeActiveLayer)  TriggerExit();
        }
        protected override void TriggerEnter()
        {
            _door.SetActive(false);
        }

        protected override void TriggerExit()
        {
            _door.SetActive(true);
        }
    }
}