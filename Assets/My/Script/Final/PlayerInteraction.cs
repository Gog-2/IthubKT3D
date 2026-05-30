using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("UI Настройки")]
    [Tooltip("Корневой объект UI подсказки (панель или сам текст). Будет скрываться/показываться.")]
    [SerializeField] private GameObject interactionPromptUI;
    
    [Tooltip("Текстовый компонент для вывода названия объекта и клавиши.")]
    [SerializeField] private TextMeshProUGUI promptText;

    [Header("Настройки рейкаста")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactionLayers = ~0;
    
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Ray aimRay = GetAimRay();
        
        if (Physics.Raycast(aimRay, out RaycastHit hit, interactionRange, interactionLayers))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable == null)
            {
                interactable = hit.collider.GetComponentInParent<IInteractable>();
            }
            
            if (interactable != null)
            {
                ShowPrompt(interactable.GetInteractionPrompt());
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
                return; 
            }
        }
        
        HidePrompt();
    }
    
    public Ray GetAimRay()
    {
        return new Ray(mainCamera.transform.position, mainCamera.transform.forward);
    }

    private void ShowPrompt(string prompt)
    {
        if (interactionPromptUI != null && !interactionPromptUI.activeSelf)
        {
            promptText.gameObject.SetActive(true);
            interactionPromptUI.SetActive(true);
        }
        
        if (promptText != null && promptText.text != prompt)
        {
            promptText.text = prompt;
        }
    }

    private void HidePrompt()
    {
        if (interactionPromptUI != null && interactionPromptUI.activeSelf)
        {
            promptText.gameObject.SetActive(false);
            interactionPromptUI.SetActive(false);
        }
    }
}