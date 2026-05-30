using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private HashSet<string> collectedCards = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasCard(string cardId) => collectedCards.Contains(cardId);

    public void AddCard(string cardId)
    {
        if (collectedCards.Add(cardId))
        {
            Save();
            Debug.Log($"[Инвентарь] Карта {cardId} добавлена.");
        }
    }

    private void Save()
    {
        string data = string.Join(",", collectedCards);
        PlayerPrefs.SetString("CollectedCards", data);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        string data = PlayerPrefs.GetString("CollectedCards", "");
        if (!string.IsNullOrEmpty(data))
        {
            foreach (var id in data.Split(','))
                collectedCards.Add(id);
        }
    }
}