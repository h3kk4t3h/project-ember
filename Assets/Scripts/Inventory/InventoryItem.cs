using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public ItemSO item;
    [HideInInspector] public Transform parentAfterDrag;

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI countText;

    public int count = 1;

    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // Count setter 
    public void InitialiseItem(ItemSO newItem, int amount = 1)
    {
        item = newItem;
        count = Mathf.Max(1, amount);
        if (icon != null && item != null) icon.sprite = item.icon;
        UpdateCountText();
    }

    public void UpdateCountText()
    {
        if (countText == null) return;

        if (item != null && item.stackable)
        {
            countText.gameObject.SetActive(true);
            countText.text = count > 1 ? count.ToString() : "";
        }
        else
        {
            countText.gameObject.SetActive(false);
        }
    }

    // merge logic 
    public int AddAmount(int amount)
    {
        if (item == null || !item.stackable) return amount;
        int space = item.maxStack - count;
        int toAdd = Mathf.Min(space, amount);
        count += toAdd;
        UpdateCountText();
        return amount - toAdd;
    }

    // reduce logic if item is used 
    public bool ReduceAmount(int amount)
    {
        count -= amount;
        UpdateCountText();
        if (count <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvasGroup != null) canvasGroup.blocksRaycasts = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null) canvasGroup.blocksRaycasts = true;
        if (parentAfterDrag != null)
        {
            transform.SetParent(parentAfterDrag, false);
            transform.localPosition = Vector3.zero;
        }
    }

    void OnValidate()
    {
        if (countText != null) UpdateCountText();
    }
}
