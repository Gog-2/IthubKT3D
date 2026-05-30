using UnityEngine;
using TMPro;
using System;
using Cysharp.Threading.Tasks;

public class WeaponShooting : MonoBehaviour
{
    [Header("Настройки оружия")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float weaponRange = 100f;
    [SerializeField] private LayerMask hitLayers = ~0;

    [Header("Скорострельность (Кулдаун)")]
    [Tooltip("Задержка между выстрелами в секундах")]
    [SerializeField] private float fireRate = 0.2f; 
    private bool canShoot = true;

    [Header("Патроны")]
    [SerializeField] private int maxAmmoInMagazine = 3;
    [SerializeField] private int totalAmmo = 9;
    [SerializeField] private float reloadTime = 1.5f;
    
    [Header("Звуки")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptyClickSound;
    
    [Header("Импакт камеры (Отдача)")]
    [Tooltip("Насколько камера уйдет назад по оси Z при выстреле")]
    [SerializeField] private float recoilDistance = 0.15f; 
    [Tooltip("Длительность анимации отдачи (туда-обратно)")]
    [SerializeField] private float kickDuration = 0.15f; 
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Ссылки")]
    [SerializeField] private PlayerInteraction playerInteraction;

    private int currentAmmo;
    private bool isReloading = false;
    private AudioSource audioSource;
    private Camera mainCamera;
    private Transform cameraTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        currentAmmo = maxAmmoInMagazine;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        if (playerInteraction == null)
        {
            playerInteraction = GetComponent<PlayerInteraction>();
        }
        
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && canShoot && !isReloading)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                ApplyCooldown().Forget();
                ApplyCameraKick().Forget();
            }
            else
            {
                PlaySound(emptyClickSound);
                ApplyCooldown().Forget(); 
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmoInMagazine && totalAmmo > 0)
        {
            Reload().Forget();
        }
    }

    private void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();
        PlaySound(shootSound);

        Ray aimRay = playerInteraction != null ? playerInteraction.GetAimRay() : new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(aimRay, out RaycastHit hit, weaponRange, hitLayers))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable == null) damageable = hit.collider.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }
    
    private async UniTaskVoid ApplyCooldown()
    {
        canShoot = false;
        await UniTask.Delay(TimeSpan.FromSeconds(fireRate), cancellationToken: this.GetCancellationTokenOnDestroy());
        canShoot = true;
    }
    
    private async UniTaskVoid ApplyCameraKick()
    {
        Vector3 originalPos = cameraTransform.localPosition;
        Vector3 targetPos = originalPos + new Vector3(0, 0, recoilDistance);
        
        float halfDuration = kickDuration / 2f;
        float elapsed = 0f;
        
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            cameraTransform.localPosition = Vector3.Lerp(originalPos, targetPos, elapsed / halfDuration);
            await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
        }
        
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            cameraTransform.localPosition = Vector3.Lerp(targetPos, originalPos, elapsed / halfDuration);
            await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
        }
        
        cameraTransform.localPosition = originalPos;
    }
    
    private async UniTaskVoid Reload()
    {
        isReloading = true;
        PlaySound(reloadSound);

        await UniTask.Delay(TimeSpan.FromSeconds(reloadTime), cancellationToken: this.GetCancellationTokenOnDestroy());

        int ammoNeeded = maxAmmoInMagazine - currentAmmo;
        if (totalAmmo >= ammoNeeded)
        {
            currentAmmo = maxAmmoInMagazine;
            totalAmmo -= ammoNeeded;
        }
        else
        {
            currentAmmo += totalAmmo;
            totalAmmo = 0;
        }

        UpdateAmmoUI();
        isReloading = false;
    }

    public void AddAmmo(int amount)
    {
        totalAmmo += amount;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null) ammoText.text = $"{currentAmmo} / {totalAmmo}";
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}