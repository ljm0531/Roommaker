using UnityEngine;
using UnityEngine.EventSystems;

public class FurniturePlacementManager : MonoBehaviour
{
    public static FurniturePlacementManager Instance { get; private set; }

    [Header("레이어 설정")]
    [SerializeField] private LayerMask groundLayer;      // 바닥 레이어 (이동 계산용)
    [SerializeField] private LayerMask furnitureLayer;   // 가구 레이어 (선택 클릭용)

    [Header("회전 및 생성 옵션")]
    [SerializeField] private float rotationSpeed = 500f; // 마우스 휠 회전 속도
    [SerializeField] private float spawnDistance = 4f;   // 카메라 기준 생성 거리

    private GameObject currentSelectedObject;
    private FurnitureObject currentFurniture;
    private bool isDragging = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // UI를 클릭하고 있는 중이라면 씬 내부 클릭 동작 차단
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        HandleMouseSelection();
        HandleDragAndRotation();
    }

    // 1. 하단 UI 클릭 시 화면 중앙 시점에 가구 배치
    public void SpawnFurniture(FurnitureData data)
    {
        if (data == null || data.prefab == null) return;

        DeselectCurrent();

        // 화면 중앙 기준 레이캐스트 계산
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 spawnPosition;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            spawnPosition = hit.point;
        }
        else
        {
            // 바닥이 감지되지 않으면 카메라 앞쪽 평면에 배치
            spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
            spawnPosition.y = 0f;
        }

        // 가구 생성 및 즉시 선택 상태 전환
        GameObject newObj = Instantiate(data.prefab, spawnPosition, Quaternion.identity);
        SelectObject(newObj);
    }

    // 2. 마우스 클릭 감지 (가구 선택)
    private void HandleMouseSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, furnitureLayer))
            {
                SelectObject(hit.collider.gameObject);
                isDragging = true; // 클릭과 동시에 드래그 시작
            }
            else
            {
                // 빈 공간(바닥) 클릭 시 선택 해제
                DeselectCurrent();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    // 3. 마우스 드래그 이동 및 누른 상태에서 휠 회전
    private void HandleDragAndRotation()
    {
        if (currentSelectedObject == null) return;

        // (1) 마우스 드래그 이동
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                currentSelectedObject.transform.position = hit.point;
            }

            // (2) 드래그(꾹 누르고 있는) 상태에서 마우스 휠을 돌릴 때 Y축 회전
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                currentSelectedObject.transform.Rotate(Vector3.up, scroll * rotationSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    // 가구 선택 처리
    private void SelectObject(GameObject target)
    {
        DeselectCurrent();

        currentSelectedObject = target;
        currentFurniture = target.GetComponent<FurnitureObject>();

        if (currentFurniture != null)
        {
            currentFurniture.SetSelected(true);
        }
    }

    // 선택 해제 처리
    private void DeselectCurrent()
    {
        if (currentFurniture != null)
        {
            currentFurniture.SetSelected(false);
        }

        currentSelectedObject = null;
        currentFurniture = null;
    }
}