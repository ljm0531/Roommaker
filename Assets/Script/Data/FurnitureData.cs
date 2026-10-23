using UnityEngine;

// 프로젝트 창에서 우클릭으로 데이터를 쉽게 생성할 수 있도록 메뉴 추가
[CreateAssetMenu(fileName = "NewFurnitureData", menuName = "Furniture/Furniture Data")]
public class FurnitureData : ScriptableObject
{
    public string furnitureName;   // 가구 이름
    public GameObject prefab;      // 배치할 가구 프리팹
    public Sprite thumbnail;       // UI 슬롯에 들어갈 썸네일 이미지
}