using UnityEngine;

public class SpawnRandomButton : MonoBehaviour
{
    public void SpawnRandom() => ManagerInventory.Instance.SpawnRandom();
}
