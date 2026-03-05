using Cysharp.Threading.Tasks;
using My.Script._3D;
using UnityEngine;

public class ButtonTeleport : ButtonParent
{
    [SerializeField] private Transform _toTeleport;
    [SerializeField] private int _timeToCloseMsg;
    [SerializeField] private GameObject _msg;
    [SerializeField] private TMPro.TMP_Text _text;
    [SerializeField] private string _message;

    private bool _isTeleporting = false;

    protected override void OnTriggerEnter(Collider other)
    {
        if (_isTeleporting) return;

        ICanBeActiveted _canBeActiveted = other.GetComponent<ICanBeActiveted>();
        if (_canBeActiveted != null)
        {
            TriggerEnter();
            Teleport(other);
            CloseMsg().Forget();
        }
    }

    protected override void TriggerEnter()
    {
        _text.text = _message;
        _msg.SetActive(true);
    }

    private void Teleport(Collider other)
    {
        _isTeleporting = true;
    
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            other.transform.position = _toTeleport.position;
            cc.enabled = true;
        }
        else
        {
            other.transform.position = _toTeleport.position;
        }
    }

    private async UniTask CloseMsg()
    {
        await UniTask.WaitForSeconds(_timeToCloseMsg);
        _msg.SetActive(false);
        _isTeleporting = false;
    }
}