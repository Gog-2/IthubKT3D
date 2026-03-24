using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public enum NameOfItems
{
    Apple = 0,
    Banana = 1,
    Pear = 2
}

public class ManagerInventory : MonoBehaviour
{
    public static ManagerInventory Instance;

    [SerializeField] private HolderItem prefab;
    [SerializeField] private SO_item[] itemsPrefabData;
    [SerializeField] private Transform[] itemPos;

    private List<HolderItem> _items = new List<HolderItem>();

    private string _jsonPath;

    public void Awake()
    {
        _jsonPath = Application.dataPath + "/Resources/Inventory.json";

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadData();
    }

    public void Spawn(NameOfItems name)
    {
        switch (name)
        {
            case NameOfItems.Apple: SpawnerItems(0); break;
            case NameOfItems.Banana: SpawnerItems(1); break;
            case NameOfItems.Pear: SpawnerItems(2); break;
        }
    }

    private void SpawnerItems(int id)
    {
        HolderItem init = Instantiate(prefab, GivePosToSpawn());
        init.Init(new itemsData(id, itemsPrefabData[id].name, itemsPrefabData[id].description),
            itemsPrefabData[id].sprite);
        _items.Add(init);
    }

    private void SpawnerItems(int id, Transform zone)
    {
        HolderItem init = Instantiate(prefab, zone);
        init.Init(new itemsData(id, itemsPrefabData[id].name, itemsPrefabData[id].description),
            itemsPrefabData[id].sprite);
        _items.Add(init);
    }

    private Transform GivePosToSpawn()
    {
        int howMany = _items.Count;
        if (howMany < 5) return itemPos[0];
        if (howMany < 10) return itemPos[1];
        return itemPos[2];
    }

    public Transform GetZoneUnderPoint(Vector2 screenPoint)
    {
        for (int i = 0; i < itemPos.Length; i++)
        {
            RectTransform rect = itemPos[i] as RectTransform;
            if (rect != null && RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint))
                return itemPos[i];
        }

        return null;
    }

    public void MoveItemToZone(HolderItem item, Transform newZone, Vector2 screenPoint)
    {
        item.transform.SetParent(newZone, false);

        int insertIndex = GetInsertIndex(newZone, screenPoint, item.transform);
        item.transform.SetSiblingIndex(insertIndex);

        SortByHierarchy();
    }

    private int GetInsertIndex(Transform zone, Vector2 screenPoint, Transform dragged)
    {
        Camera cam = null;

        for (int i = 0; i < zone.childCount; i++)
        {
            Transform child = zone.GetChild(i);
            if (child == dragged) continue;

            RectTransform childRect = child as RectTransform;
            if (childRect == null) continue;

            Vector2 childCenter = RectTransformUtility.WorldToScreenPoint(cam, childRect.position);

            if (screenPoint.x < childCenter.x)
                return i;
        }

        return zone.childCount;
    }

    public void Unsubscribe(HolderItem item) => _items.Remove(item);

    private void SortByHierarchy() =>
        _items.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

    public void SaveData()
    {
        SavedData data = new SavedData();
        data.items = new List<NameOfItems>();
        SortByHierarchy();

        foreach (HolderItem item in _items)
            data.items.Add(item.Index);

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(_jsonPath, json);
        Debug.Log($"Saved {data.items.Count} items to {_jsonPath}");
    }

    private void LoadData()
    {
        if (!File.Exists(_jsonPath))
        {
            Debug.Log("No save file found, skipping load.");
            return;
        }

        string json = File.ReadAllText(_jsonPath);
        SavedData data = JsonConvert.DeserializeObject<SavedData>(json);

        if (data.items == null || data.items.Count == 0) return;
        for (int i = 0; i < data.items.Count; i++)
        {
            int id = (int)data.items[i];
            Transform zone = GetZoneByIndex(i);
            SpawnerItems(id, zone);
        }

        Debug.Log($"Loaded {data.items.Count} items from {_jsonPath}");
    }

    private Transform GetZoneByIndex(int index)
    {
        if (index < 5) return itemPos[0];
        if (index < 10) return itemPos[1];
        return itemPos[2];
    }
}