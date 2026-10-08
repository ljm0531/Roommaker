using System.Collections.Generic;
using UnityEngine;

public class FurnitureUIManager : MonoBehaviour
{
    [Header("--- Furniture Database ---")]
    [SerializeField] private List<FurnitureData> furnitureList = new List<FurnitureData>();

    [Header("--- UI References ---")]
    [SerializeField] private Transform slotContainer;      // ScrollView의 Content Transform
    [SerializeField] private GameObject slotPrefab;        // FurnitureDragSlot 스크립트가 붙은 UI 프리팹

    [Header("--- Placement Settings ---")]
    [SerializeField] private LayerMask floorLayer;         // 3D 바닥 레이어 (Floor/Ground)
    
    private Camera mainCamera;
    private GameObject currentPreviewObject;
    private FurnitureData currentSelectedData;

    void Start()
    {
        mainCamera = Camera.main;
        PopulateFurnitureUI();
    }

    // 1. 하단 UI 슬롯 생성
    private void PopulateFurnitureUI()
    {
        if (slotContainer == null || slotPrefab == null) return;

        foreach (var data in furnitureList)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            FurnitureDragSlot slotScript = slotObj.GetComponent<FurnitureDragSlot>();
            if (slotScript != null)
            {
                slotScript.Init(data, this);
            }
        }
    }

    // 2. UI 드래그 시작: 임시 프리팹 생성 (미리보기)
    public void StartPlacementDrag(FurnitureData data)
    {
        currentSelectedData = data;
        if (data.prefab != null)
        {
            currentPreviewObject = Instantiate(data.prefab);
            // 필요 시 미리보기용 머티리얼 변경이나 콜라이더 비활성화 가능
        }
    }

    // 3. UI 드래그 중: 마우스 위치에 맞춰 3D 바닥 위에 미리보기 배치
    public void UpdatePlacementDrag(Vector2 screenPosition)
    {
        if (currentPreviewObject == null) return;

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, floorLayer))
        {
            currentPreviewObject.transform.position = hit.point;
            currentPreviewObject.SetActive(true);
        }
        else
        {
            // 바닥 레이어에 안 닿은 경우 숨김
            currentPreviewObject.SetActive(false);
        }
    }

    // 4. 드래그 종료: 바닥 위면 배치 완료, 아니면 취소/삭제
    public void EndPlacementDrag(Vector2 screenPosition)
    {
        if (currentPreviewObject == null) return;

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, floorLayer))
        {
            // 최종 위치에 고정
            currentPreviewObject.transform.position = hit.point;
            currentPreviewObject = null; // 배치 성공
        }
        else
        {
            // 허용되지 않은 공간에 놓으면 파괴
            Destroy(currentPreviewObject);
            currentPreviewObject = null;
        }
    }
}