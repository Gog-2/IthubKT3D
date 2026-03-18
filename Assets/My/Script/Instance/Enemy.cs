using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField]private Transform teleporterMin,teleporterMax;
    public Transform EnemyTransform;
    [SerializeField]private AudioClip _audioClip;

    public SpriteRenderer SpriteRendererChange;

    private void Awake()
    {
        GameManager.instance.Subscribe(this);
    }

    private void Start()
    {
        GameManager.instance.WakeOnLoad();
    }

    //Я этот Сanvas в the world ШАТАЛ ЧТО КНОПКА НЕ РАБОТАЕТ Я через Scale делал для стабильность но он не работает.😭😭😭
    public void TakeDamage()
    {
        EnemyTransform.position = new Vector3(
            Random.Range(teleporterMin.position.x,teleporterMax.position.x), 
            Random.Range(teleporterMin.position.y,teleporterMax.position.y), 
            Random.Range(teleporterMin.position.z,teleporterMax.position.z)
            );
        SpriteRendererChange.color = new  Color(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0,1f),1);
        GameManager.instance.AudioPlayOneShot(_audioClip,AudioType.SFX);
    }
}
