using UnityEngine;

public class WASDCameraController : MonoBehaviour
{
    [Header("이동 속도 설정")]
    [SerializeField] private float moveSpeed = 8f;        // 기본 이동 속도
    [SerializeField] private float boostMultiplier = 2f;  // Shift 키를 누를 때 가속 배율

    [Header("회전 속도 설정")]
    [SerializeField] private float lookSensitivity = 2f;  // 마우스 회전 감도

    private float yaw = 0f;   // 좌우 회전 각도 (Y축)
    private float pitch = 0f; // 상하 회전 각도 (X축)

    private void Start()
    {
        // 시작 시 카메라의 현재 회전값을 읽어와 초기화
        Vector3 currentAngles = transform.eulerAngles;
        yaw = currentAngles.y;
        pitch = currentAngles.x;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    // 1. WASD 및 Q/E 키를 이용한 위치 이동
    private void HandleMovement()
    {
        // Left Shift 누르고 있으면 이동 속도 증가
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= boostMultiplier;
        }

        // WASD 키 입력 처리 (A/D: -1~1, W/S: -1~1)
        float moveX = Input.GetAxis("Horizontal"); // A, D 키
        float moveZ = Input.GetAxis("Vertical");   // W, S 키

        // 높이 조절 키 입력 (E: 위로, Q: 아래로)
        float moveY = 0f;
        if (Input.GetKey(KeyCode.E)) moveY = 1f;
        if (Input.GetKey(KeyCode.Q)) moveY = -1f;

        // 카메라가 바라보는 방향 기준으로 이동 방향 계산
        Vector3 moveDirection = (transform.right * moveX) + (transform.up * moveY) + (transform.forward * moveZ);

        // 카메라 위치 갱신 (프레임 독립성 유지)
        transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }

    // 2. 마우스 우클릭 드래그를 이용한 시점 회전
    private void HandleRotation()
    {
        // 마우스 우클릭(1번 버튼) 상태일 때만 시점 회전 실행
        if (Input.GetMouseButton(1))
        {
            // 마우스 이동량 계산
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

            yaw += mouseX;
            pitch -= mouseY; // Y축 이동에 따라 X축 회전을 반대로 적용해야 정방향 회전

            // 상하 회전 각도가 너무 돌아가지 않도록 제한 (-80도 ~ 80도)
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            // 카메라 회전 적용
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}