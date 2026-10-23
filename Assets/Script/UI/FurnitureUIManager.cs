using System.Collections.Generic;
using UnityEngine;

public class FurnitureUIManager : MonoBehaviour
{
    [Header("데이터 및 프레임 설정")]
    [SerializeField] private List<FurnitureData> furnitureList; // 등록할 가구 데이터 리스트
    [SerializeField] private GameObject uiItemPrefab;            // UI_FurnitureItem 프리팹
    [SerializeField] private Transform contentParent;            // UI ScrollView의 Content 트랜스폼

    private void Start()
    {
        GenerateUIList();
    }

    // 데이터 리스트를 바탕으로 UI 슬롯들을 생성
    private void GenerateUIList()
    {
        // 기존 생성된 항목 청소
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 가구 데이터 수만큼 UI 슬롯 생성
        foreach (FurnitureData data in furnitureList)
        {
            GameObject itemObj = Instantiate(uiItemPrefab, contentParent);
            FurnitureUIItem itemScript = itemObj.GetComponent<FurnitureUIItem>();
            
            if (itemScript != null)
            {
                itemScript.Setup(data);
            }
        }
    }
}