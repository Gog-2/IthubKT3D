using UnityEngine;

public class SpawnButtons : MonoBehaviour
{
   public void Spawn(int name) => ManagerInventory.Instance.Spawn((NameOfItems)name);
}
