using UnityEngine;

public class SpawnTrap : ButtonParent
{
    private bool _isActiveted = false;
    [SerializeField]private Transform _spawnPoint;
    [SerializeField]private GameObject _trap;
    protected override void OnTriggerEnter(Collider other)
    {
        if (_isActiveted) return;
        base.OnTriggerEnter(other);
    }
    protected override void TriggerEnter()
    {
        _isActiveted = true;
        Instantiate(_trap, _spawnPoint.position, _spawnPoint.rotation);
    }
}
