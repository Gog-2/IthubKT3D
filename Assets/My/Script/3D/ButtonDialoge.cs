using UnityEngine;

namespace My.Script._3D
{
    public class ButtonDialoge : ButtonParent
    {
        [SerializeField] private GameObject _textSpace;
        [SerializeField] private TMPro.TMP_Text _textObject;
        [SerializeField] private string _text;
        protected override void TriggerEnter()
        {
            _textObject.text = _text;
            _textSpace.gameObject.SetActive(true);
        }

        protected override void TriggerExit()
        {
            _textSpace.gameObject.SetActive(false);
        }
    }
}