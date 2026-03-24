using UnityEngine;

public class ClearButton : MonoBehaviour
{
   public void Clear() => ManagerInventory.Instance.ClearAll();
}
