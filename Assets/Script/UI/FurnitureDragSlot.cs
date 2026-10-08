using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// IBeginDragHandler, IDragHandler, IEndDragHandler 인터페이스 구현
public class FurnitureDragSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image iconImage;
    private FurnitureData furnitureData;
    private FurnitureUIManager uiManager;

    // 슬롯 초기화
    public void Init(FurnitureData data, FurnitureUIManager manager)
    {
        furnitureData = data;
        uiManager = manager;
        if (iconImage != null && data.icon != null)
        {
            iconImage.sprite = data.icon;
        }
    }

    // 드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (furnitureData == null || furnitureData.prefab == null) return;
        uiManager.StartPlacementDrag(furnitureData);
    }

    // 드래그 중 매 프레임 호출
    public void OnDrag(PointerEventData eventData)
    {
        uiManager.UpdatePlacementDrag(eventData.position);
    }

    // 드래그 종료 시 호출 (손을 뗐을 때)
    public void OnEndDrag(PointerEventData eventData)
    {
        uiManager.EndPlacementDrag(eventData.position);
    }
}