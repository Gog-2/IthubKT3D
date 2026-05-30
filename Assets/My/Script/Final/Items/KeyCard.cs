using UnityEngine;

public class KeyCard : MonoBehaviour, IInteractable
{
    [SerializeField, Tooltip("Уникальный ID карты")] 
    private string cardId = "card_01";
    
    [SerializeField] 
    private string prompt = "Подобрать ключ-карту";

    void Start()
    {
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasCard(cardId))
        {
            gameObject.SetActive(false);
        }
    }

    public string GetInteractionPrompt() => prompt;

    public void Interact()
    {
        if (InventoryManager.Instance == null) return;

        InventoryManager.Instance.AddCard(cardId);
        gameObject.SetActive(false);
    }
}