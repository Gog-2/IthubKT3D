using Unity.VisualScripting;
using UnityEngine;

public class Coin : ButtonParent
{
    [SerializeField] private int _coinAmount;
    private float time;
    [SerializeField]private float _animationSpeed;
    protected override void TriggerEnter()
    {
        CoinHolder.instance.AddCoin(_coinAmount);
        Destroy(gameObject);
    }

    private void Update()
    {
        time += Time.deltaTime * _animationSpeed;
        this.transform.Rotate(new Vector3(0, -time, 90));
    }
}
