using UnityEngine;

public class FurnitureObject : MonoBehaviour
{
    [Header("마테리얼 설정")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material redMaterial;          // 충돌 시 표시할 빨간색 마테리얼
    [SerializeField] private Material greenOutlineMaterial;  // 선택 시 표시할 초록색 마테리얼

    private Material originalMaterial;                      // 가구의 원래 마테리얼
    private int overlapCount = 0;                           // 현재 겹쳐진 타 오브젝트 수
    private bool isSelected = false;                        // 현재 선택 여부

    private void Awake()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (meshRenderer != null)
            originalMaterial = meshRenderer.material;
    }

    // 다른 오브젝트나 맵과 겹쳤을 때 (Trigger 감지)
    private void OnTriggerEnter(Collider other)
    {
        // 바닥(Ground) 레이어가 아닌 다른 오브젝트와 충돌한 경우
        if (!other.CompareTag("Ground"))
        {
            overlapCount++;
            UpdateVisualState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Ground"))
        {
            overlapCount--;
            if (overlapCount < 0) overlapCount = 0;
            UpdateVisualState();
        }
    }

    // 가구 선택 상태 설정
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        UpdateVisualState();
    }

    // 가구의 현재 시각적 상태(충돌/선택/정상) 업데이트
    private void UpdateVisualState()
    {
        if (meshRenderer == null) return;

        // 1순위: 다른 물체와 겹쳤을 때 빨간색으로 표시
        if (overlapCount > 0)
        {
            meshRenderer.material = redMaterial;
        }
        // 2순위: 선택되어 있을 때 초록색 표시
        else if (isSelected)
        {
            meshRenderer.material = greenOutlineMaterial;
        }
        // 3순위: 정상 상태 시 원래 마테리얼 복원
        else
        {
            meshRenderer.material = originalMaterial;
        }
    }
}