using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float _timer = 0;
    [SerializeField]private int _seconds;
    [SerializeField]private Image _image;
    [SerializeField]private TMP_Text _timerText;
    [SerializeField]private float _duration;

    private void Start()
    {
        _timer = _seconds;
    }

    private void Update()
    {
        TimerBomb();
    }

    private void TimerBomb()
    {
        if (_timer <= 0) return;
        _timer -= Time.deltaTime;
        _image.DOFillAmount(_timer / _seconds, _duration).SetEase(Ease.Linear);
        _timerText.text = $"{Mathf.FloorToInt(_timer / 60) :00}:{_timer % 60 :00}";
    }
}
