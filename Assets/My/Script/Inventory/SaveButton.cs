using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public void SaveData()
    {
        ManagerInventory.Instance.SaveData();
    }
}
