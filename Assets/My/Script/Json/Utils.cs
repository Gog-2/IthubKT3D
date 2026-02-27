using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
public class Utils : MonoBehaviour
{
    public TextAsset textAssetData;
    private DataBase _dataBase = new DataBase();

    public void ParseCSV()
    {
        string jsonPath = Application.dataPath + "/Resources/data.json";
        ParserCVSToMassive();
        string json = JsonConvert.SerializeObject(_dataBase, Formatting.Indented);
        Debug.Log($"[Utils] JSON:\n{json}");
        File.WriteAllText(jsonPath, json);
        Debug.Log($"[Utils] Сохранено: {jsonPath}");
        
    }

    private void ParserCVSToMassive()
    {
        string[] lines = textAssetData.text.Split(
            new string[] { "\r\n", "\n", "\r" },
            StringSplitOptions.RemoveEmptyEntries
        );

        Debug.Log($"[Utils] Строк в CSV: {lines.Length}");
        
        int tableSize = lines.Length - 1;

        if (tableSize <= 0)
        {
            Debug.LogError("[Utils] CSV пустой или только заголовки!");
            return;
        }

        _dataBase.DataMassive = new Data[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            string[] values = lines[i + 1].Split(';');

            Debug.Log($"[Utils] Строка {i + 1}: [{string.Join(" | ", values)}]");

            if (values.Length < 3)
            {
                Debug.LogWarning($"[Utils] Строка {i + 1} содержит меньше 3 колонок, пропускаем.");
                continue;
            }

            _dataBase.DataMassive[i] = new Data();
            _dataBase.DataMassive[i].Name = values[0].Trim();
            _dataBase.DataMassive[i].Description = values[1].Trim();

            if (int.TryParse(values[2].Trim(), out int parsedId))
                _dataBase.DataMassive[i].id = parsedId;
            else
                Debug.LogWarning($"[Utils] Не удалось распарсить id: '{values[2]}' в строке {i + 1}");
        }
    }

    public void ParseJson()
    {
            string path = Application.dataPath + "/Resources/data.json";
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                DataBase db = JsonConvert.DeserializeObject<DataBase>(json);
                _dataBase = db;
            }
            foreach (var data in _dataBase.DataMassive)
            {
                Debug.Log($"{data.Name}, {data.Description}. {data.id}");
            }
    }

}
