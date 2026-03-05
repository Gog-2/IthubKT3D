using TMPro;
using UnityEngine;

public class CoinHolder : MonoBehaviour
{
    public static CoinHolder instance;
    [SerializeField] private GameObject _UI;
    [SerializeField] private TMP_Text _CoinAmount;
    [SerializeField] private int _coinAmount;
    private bool _openCoinsUI = false;

    private void Awake()
    {
        instance = this;
    }

    public void AddCoin(int amount)
    {
        if (!_openCoinsUI) _UI.SetActive(true);
        _coinAmount += amount;
        _CoinAmount.text = _coinAmount.ToString();
    }
}
