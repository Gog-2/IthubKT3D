using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HolderItem : MonoBehaviour
{
    [SerializeField]private Image image;
    [SerializeField]private int imageId;
    [SerializeField]private TMP_Text _name, _description;
    public NameOfItems Index;
    
    public void Init(itemsData itemData,Sprite sprite)
    {
        image.sprite = sprite;
        imageId = itemData.SpriteId;
        _name.text = itemData.Name;
        _description.text = itemData.Description;
        Index = (NameOfItems)itemData.SpriteId;
    }
    public void DestroyThis() => Destroy(gameObject);
    private void OnDestroy() 
    {
        if (ManagerInventory.Instance != null) 
        {
            ManagerInventory.Instance.Unsubscribe(this);
        }
    }

}
