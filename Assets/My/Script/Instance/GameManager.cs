using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum AudioType
{
    BGM,
    SFX
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private AudioSource[] _audioSource = new AudioSource[2];
    [SerializeField]private List<Enemy> _enemy;
    private List<EnemyData> _enemyData;
    private bool _reloadingScene = true;
    private bool _dataLoaded = false;
    [SerializeField]private Slider  _slider;
    [SerializeField]private AudioMixer _audioMixer;
    private TotalData _totalData;
    private SliderChanger _sliderChanger;
    string jsonPath = Application.dataPath + "/Resources/Instance.json";
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneReload();
        }
    }
    

    public void AudioSourceConnect(AudioSource audioSource, AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.BGM:
                _audioSource[0]= audioSource;
                break;
            case AudioType.SFX:
                _audioSource[1]= audioSource;
                break;
            default:
                Debug.LogError("This Audio type not exist:" + audioType + "Connect");
                break;
        }
    }

    public void AudioPlayOneShot(AudioClip clip, AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.BGM:
                _audioSource[0].PlayOneShot(clip);
                break;
            case AudioType.SFX:
                _audioSource[1].PlayOneShot(clip);
                break;
            default:
                Debug.LogError("This Audio type not exist:" + audioType + "OneShot");
                break;
        }
    }

    private void SceneReload()
    {
        _reloadingScene = true;
        _dataLoaded = false; 
        SaveDataToJson();
        _enemy.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void WakeOnLoad()
    {
        if (!_reloadingScene) return;
        _reloadingScene = false;
        WaitToLoad().Forget();
    }

    private async UniTask WaitToLoad()
    {
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            _totalData = JsonConvert.DeserializeObject<TotalData>(json);
        }

        int expectedEnemies = _totalData?.Enemies?.Count ?? 0;
        
        while (_enemy.Count < expectedEnemies)
        {
            await UniTask.Yield();
        }

        if (_dataLoaded) return;
        _dataLoaded = true;
        
        ExecuteLoadLogic();
    }

    private void ExecuteLoadLogic()
    {
        if (_totalData == null) return;

        for (int i = 0; i < _totalData.Enemies.Count; i++)
        {
            if (i >= _enemy.Count) break; 
        
            ObjectDataConvector.ApplyTransformData(_totalData.Enemies[i].Transform, _enemy[i].EnemyTransform);
            ObjectDataConvector.ApplySpriteRenderer(_totalData.Enemies[i].Sprite, _enemy[i].SpriteRendererChange);
        }
    
        if (_slider != null) _slider.value = _totalData.volumeData.Volume;
    }
    public float GetSavedVolume()
    {
        if (_totalData != null && _totalData.volumeData != null)
        {
            return _totalData.volumeData.Volume;
        }
        return 0f;
    }

    
    private void SaveDataToJson()
    {
        var dataToSave = new List<EnemyData>();

        foreach (Enemy enemy in _enemy)
        {
            dataToSave.Add(new EnemyData(
                ObjectDataConvector.TransformToTransformData(enemy.EnemyTransform),
                ObjectDataConvector.GetSpriteRenderer(enemy.SpriteRendererChange)
            ));
        }

        TotalData data = new TotalData(_sliderChanger.slider.value, dataToSave);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(jsonPath, json);
        Debug.Log($"Saved {dataToSave.Count} enemies to {jsonPath}");
    }
    
    private void LoadDataFromJson()
    {
        if (!File.Exists(jsonPath))
        {
            Debug.LogWarning("Save file not found: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        _totalData = JsonConvert.DeserializeObject<TotalData>(json);
    
        for (int i = 0; i < _totalData.Enemies.Count; i++)
        {
            Debug.Log($"[{i}] enemy: {_enemy[i]?.name}, " +
                      $"EnemyTransform: {_enemy[i]?.EnemyTransform}, " +
                      $"SpriteRendererChange: {_enemy[i]?.SpriteRendererChange}");
        }
    
        for (int i = 0; i < _totalData.Enemies.Count; i++)
        {
            ObjectDataConvector.ApplyTransformData(_totalData.Enemies[i].Transform, _enemy[i].EnemyTransform);
            ObjectDataConvector.ApplySpriteRenderer(_totalData.Enemies[i].Sprite, _enemy[i].SpriteRendererChange);
        }
    
        _slider.value = _totalData.volumeData.Volume;
    }

    public void Subscribe(Enemy enemy)
    {
        _enemy.Add(enemy);
    }

    public void Subscribe(SliderChanger sliderChanger)
    {
        _sliderChanger = sliderChanger;
    }
    

}
