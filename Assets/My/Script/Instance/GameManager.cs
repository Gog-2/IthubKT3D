using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
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
    private Enemy[] _enemy;
    private List<EnemyData> _enemyData;
    private bool _reloadingScene = true;
    private bool _isLoading = false;
    [SerializeField]private Slider  _slider;
    [SerializeField]private AudioMixer _audioMixer;
    private EnemyData[] _loadedEnemies;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        LoadData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneReload();
        }
    }

    private void LoadDataFromJson()
    {
        _isLoading = true;
        string jsonPath = Application.dataPath + "/Resources/Instance.json";
        if (!File.Exists(jsonPath))return;
        _isLoading = true;
        using (StreamReader reader = new StreamReader(jsonPath))
        {
            string json = reader.ReadToEnd();
            _loadedEnemies = JsonConvert.DeserializeObject<EnemyData[]>(json);
            
        }
        _isLoading = false; 
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
        ParseInfoToData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ParseInfoToData()
    {
        if (_reloadingScene) return;
        foreach (Enemy enemy in _enemy)
        {
            EnemyData enemyToList = new EnemyData (
                ObjectDataConvector.TransformToTransformData(enemy.EnemyTransform),
                ObjectDataConvector.GetSpriteRenderer(enemy.SpriteRendererChange)
                );
            _enemyData.Add(enemyToList);
        }
    }
    public void LoadData()
    {
        if (!_reloadingScene) return;
        _reloadingScene = false;
        for (int i = 0; i < _enemyData.Count; i++)
        {
            ObjectDataConvector.ApplyTransformData(_enemyData[i].Transform,ref _enemy[i].EnemyTransform);
            ObjectDataConvector.ApplySpriteRenderer(_enemyData[i].Sprite,ref _enemy[i].SpriteRendererChange);
        }
    }
    
}
