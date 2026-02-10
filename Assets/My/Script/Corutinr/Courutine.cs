using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Corutinrs
{
    public class Courutine : MonoBehaviour
    {
        [Header("Sprech Settings")]
        [SerializeField] private float _typeSpeed;
        [SerializeField] private SO_DialogueSorter _dialogueSorter;
        [SerializeField] private int _textQueue;
        [Header("Loaded dialog")]
        [SerializeField] private int _dialogeLoadedOn;
        [SerializeField] private bool _pause;
        [Header("Setup")]
        [SerializeField] private TMP_Text _text1;

        private string activeDialoge;

        private void Start()
        {
            activeDialoge = $"{_dialogueSorter.dialogues[0].Name}\n{_dialogueSorter.dialogues[0].Dialogue}";
        }

        private void Dialog()
        {
            if (_text1.text == activeDialoge)
            {
                _textQueue++;
                activeDialoge = $"{_dialogueSorter.dialogues[_textQueue].Name}\n{_dialogueSorter.dialogues[_textQueue].Dialogue}";
                CourutineTextWrite().Forget();
            }
            else SkipDialogue();
        }
        
        private async UniTask CourutineTextWrite()
        {
            if (_dialogeLoadedOn == 0) _text1.text = "";
            for (int i = _dialogeLoadedOn; i < activeDialoge.Length; i++)
            {
                _text1.text += activeDialoge[i];
                _dialogeLoadedOn = i;
                await UniTask.WaitForSeconds(_typeSpeed);
            }
        }

        private void SkipDialogue()
        {
            _text1.text = activeDialoge;
            _dialogeLoadedOn = activeDialoge.Length;
        }

        private void DialogueResume()
        {
            if (_pause)
            { 
                _pause = false;
                  
            }
            else
            {
                _pause = true;  
            }
        }
    }
}
