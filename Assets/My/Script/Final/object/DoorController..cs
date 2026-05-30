using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Настройки ключа")]
    [SerializeField, Tooltip("ID карты, необходимой для открытия")] 
    private string requiredCardId = "card_01";
    
    [SerializeField, Tooltip("Если выключено — дверь открывается без ключа")]
    private bool requiresKey = true;

    [Header("Настройки двери")]
    [SerializeField] private float openDuration = 3f;
    [SerializeField] private Color transparentColor = new Color(1f, 1f, 1f, 0.2f);

    [Header("Звуки")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip lockedSound;

    private MeshRenderer meshRenderer;
    private Collider doorCollider;
    private Color originalColor;
    private bool isOpen = false;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        doorCollider = GetComponent<Collider>();
        originalColor = meshRenderer.material.color;
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
    }

    public string GetInteractionPrompt()
    {
        if (isOpen) return "Дверь уже открыта";
        if (!requiresKey) return "Открыть дверь";

        bool hasCard = InventoryManager.Instance != null 
                       && InventoryManager.Instance.HasCard(requiredCardId);
        return hasCard ? "Открыть дверь" : $"🔒 Нужна карта: {requiredCardId}";
    }

    public void Interact()
    {
        if (isOpen) return;
        
        if (requiresKey)
        {
            bool hasCard = InventoryManager.Instance != null 
                           && InventoryManager.Instance.HasCard(requiredCardId);
            if (!hasCard)
            {
                PlaySound(lockedSound);
                Debug.Log("[Дверь] Нет нужной карты!");
                return;
            }
        }

        PlaySound(openSound);
        OpenDoorRoutineAsync().Forget();
    }

    private async UniTaskVoid OpenDoorRoutineAsync()
    {
        isOpen = true;
        
        Material mat = meshRenderer.material;
        originalColor = mat.color;
        
        // Включаем прозрачность
        SetMaterialTransparency(mat, true);
        mat.color = transparentColor;
        
        if (doorCollider != null) doorCollider.enabled = false;
        
        await UniTask.Delay(
            System.TimeSpan.FromSeconds(openDuration),
            cancellationToken: destroyCancellationToken
        );
        
        // Восстанавливаем оригинальный вид
        mat.color = originalColor;
        SetMaterialTransparency(mat, false);
        
        if (doorCollider != null) doorCollider.enabled = true;
        isOpen = false;
    }

    private void SetMaterialTransparency(Material mat, bool transparent)
    {
        if (transparent)
        {
            // Включаем режим прозрачности для стандартного шейдера
            mat.SetFloat("_Mode", 2); // Transparent
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
        else
        {
            // Возвращаем в непрозрачный режим
            mat.SetFloat("_Mode", 0); // Opaque
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 2000;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }
}