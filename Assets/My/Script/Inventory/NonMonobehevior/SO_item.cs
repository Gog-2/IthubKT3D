using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/item")]
public class SO_item : ScriptableObject
{
    public Sprite sprite;
    public string name;
    public string description;
}
