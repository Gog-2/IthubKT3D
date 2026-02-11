using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Corutinrs
{
    public class Courutine : MonoBehaviour
    {
        [Header("Sprech Settings")]
        [SerializeField] private float _typeSpeed;
        [SerializeField] private SO_Dialogue[] _dialogues;
        [SerializeField] private int _textQueue;
        [Header("Loaded dialog")]
        [SerializeField] private int _dialogeLoadedOn;
        [SerializeField] private bool _pause;
        [Header("Setup")]
        [SerializeField] private TMP_Text _text1;
        private CancellationTokenSource _cts;
        private bool _isTyping; 
        private string activeDialoge;

        private void Start()
        {
            NewDialogue();
        }

        public void StartDialog()
        {
            if (_text1.text == activeDialoge)
            {
                NewDialogue();
            }
            else SkipDialogue();
        }

        private void NewDialogue()
        {
            StopCurrentTyping();
            _textQueue++;
            _dialogeLoadedOn = 0;
            activeDialoge = $"{_dialogues[_textQueue - 1].Name}\n{_dialogues[_textQueue - 1].Dialogue}";
            _cts = new CancellationTokenSource();
            CourutineTextWrite(_cts.Token).Forget();  
        }
        
        private async UniTask CourutineTextWrite(CancellationToken ct)
        {
            _isTyping = true;
            if (_dialogeLoadedOn == 0) _text1.text = "";
            for (int i = _dialogeLoadedOn; i < activeDialoge.Length; i++)
            {
                ct.ThrowIfCancellationRequested();
                _text1.text += activeDialoge[i];
                _dialogeLoadedOn = i + 1;
                await UniTask.WaitForSeconds(_typeSpeed, cancellationToken: ct);
            }
            _isTyping = false;
        }

        private void SkipDialogue()
        {
            StopCurrentTyping();
            _text1.text = activeDialoge;
            _dialogeLoadedOn = _text1.text.Length;
        }
        private void StopCurrentTyping()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public void DialoguePause()
        {
            if (_pause)
            { 
                _pause = false;
                _cts = new CancellationTokenSource();
                CourutineTextWrite(_cts.Token).Forget();
            }
            else
            {
                _pause = true;
                StopCurrentTyping();
            }
        }
        private void OnDestroy()
        {
            StopCurrentTyping();
        }
    }
}
