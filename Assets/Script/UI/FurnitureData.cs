using UnityEngine;

[System.Serializable]
public class FurnitureData
{
    public string furnitureName;    // 가구 이름
    public Sprite icon;             // 하단 UI에 표시할 아이콘 이미지
    public GameObject prefab;       // 3D 공간에 생성될 가구 프리팹
}