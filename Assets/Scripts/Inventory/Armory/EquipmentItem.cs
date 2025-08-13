using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class EquipmentItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public EquipmentSO equipment;
    [HideInInspector] public Transform parentAfterDrag;

    public Image icon;

    private CanvasGroup canvasGroup;

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialise(EquipmentSO eq)
    {
        equipment = eq;
        parentAfterDrag = transform.parent;
        if (icon != null) icon.sprite = eq.icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        // if not accepted by any slot, return to original slot
        if (transform.parent == transform.root)
        {
            if (parentAfterDrag != null)
            {
                transform.SetParent(parentAfterDrag, false);
                transform.localPosition = Vector3.zero;
            }
            else Destroy(gameObject);
        }
    }
}
