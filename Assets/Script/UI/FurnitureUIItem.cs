using UnityEngine;
using UnityEngine.UI;

public class FurnitureUIItem : MonoBehaviour
{
    [SerializeField] private Image thumbnailImage; // 썸네일을 표시할 UI Image
    private FurnitureData furnitureData;

    // UI 요소에 데이터 바인딩
    public void Setup(FurnitureData data)
    {
        furnitureData = data;
        if (thumbnailImage != null && data.thumbnail != null)
        {
            thumbnailImage.sprite = data.thumbnail;
        }
    }

    // UI 항목 클릭 시 실행 (Button 컴포넌트의 OnClick에 연결)
    public void OnClickItem()
    {
        if (furnitureData != null)
        {
            // 배치 매니저에게 선택된 가구 생성을 요청
            FurniturePlacementManager.Instance.SpawnFurniture(furnitureData);
        }
    }
}