using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(HolderItem))]
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Настройки перетаскивания")]
    [SerializeField] private float _dragScale = 1.1f;
    [SerializeField] private float _dragAlpha = 0.8f;

    private HolderItem _holderItem;
    private Canvas _rootCanvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    private Transform _originalParent;
    private int _originalSiblingIndex;
    private Vector3 _originalScale;

    private void Awake()
    {
        _holderItem     = GetComponent<HolderItem>();
        _rectTransform  = GetComponent<RectTransform>();
        _canvasGroup    = GetOrAddComponent<CanvasGroup>();
        _rootCanvas     = GetRootCanvas();
    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = transform.parent;
        _originalSiblingIndex = transform.GetSiblingIndex();
        _originalScale = transform.localScale;
        
        transform.SetParent(_rootCanvas.transform, true);
        transform.SetAsLastSibling();

        transform.localScale = _originalScale * _dragScale;
        _canvasGroup.alpha = _dragAlpha;
        _canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rootCanvas.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        _rectTransform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha          = 1f;
        _canvasGroup.blocksRaycasts = true;
        transform.localScale        = _originalScale;

        Transform targetZone = ManagerInventory.Instance.GetZoneUnderPoint(eventData.position);

        if (targetZone != null)
        {
            ManagerInventory.Instance.MoveItemToZone(_holderItem, targetZone, eventData.position);
        }
        else
        {
            ReturnToOriginal();
        }
    }
    
    private void ReturnToOriginal()
    {
        transform.SetParent(_originalParent, false);
        transform.SetSiblingIndex(_originalSiblingIndex);
    }

    private Canvas GetRootCanvas()
    {
        Canvas[] canvases = GetComponentsInParent<Canvas>();
        foreach (Canvas c in canvases)
        {
            if (c.isRootCanvas) return c;
        }
        return GetComponentInParent<Canvas>();
    }

    private T GetOrAddComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        return component != null ? component : gameObject.AddComponent<T>();
    }
}