using UnityEngine;
using UnityEngine.InputSystem; // <-- ยังคงต้องมีถ้าใช้ Input System

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // ความเร็วในการเคลื่อนที่
    public float rotationSpeed = 720f; // ความเร็วในการหมุนตัวละคร (องศาต่อวินาที)

    // อ้างอิงถึงกล้องที่กำลังติดตามผู้เล่น
    public Transform cameraTransform; // <<< ต้องลาก Main Camera มาใส่ใน Inspector

    // ส่วนของ Input System (ถ้าคุณเลือกใช้)
    private Vector2 movementInput; 

    // ถ้าคุณใช้ Input System Package:
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    // **********************************

    void Update()
    {
        // 1. รับค่า Input จากแกนแนวนอน (Horizontal) และแนวตั้ง (Vertical)
        // ถ้าคุณยังใช้ Input.GetAxis():
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 rawMovementInput = new Vector3(horizontalInput, 0f, verticalInput);

        // ถ้าคุณใช้ Input System:
        // Vector3 rawMovementInput = new Vector3(movementInput.x, 0f, movementInput.y);
        // ***************************************************************

        // ตรวจสอบว่าผู้เล่นมีการกดปุ่มเคลื่อนที่หรือไม่
        if (rawMovementInput.magnitude > 0.1f) // ใช้ magnitude เพื่อตรวจสอบว่ามีการเคลื่อนที่จริงๆ (มากกว่าค่า epsilon)
        {
            // 2. คำนวณทิศทางการเคลื่อนที่โดยอิงจากมุมกล้อง
            // รับทิศทาง "หน้า" ของกล้อง (forward vector)
            Vector3 cameraForward = cameraTransform.forward;
            // รับทิศทาง "ขวา" ของกล้อง (right vector)
            Vector3 cameraRight = cameraTransform.right;

            // ทำให้ vector เป็นระนาบ (ไม่มีผลกับแกน Y)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // Normalize เพื่อให้แน่ใจว่า vector มีขนาด 1
            cameraForward.Normalize();
            cameraRight.Normalize();

            // คำนวณทิศทางการเคลื่อนที่ที่แท้จริง โดยผสม Input กับทิศทางของกล้อง
            // verticalInput จะใช้ทิศหน้าของกล้อง
            // horizontalInput จะใช้ทิศขวาของกล้อง
            Vector3 moveDirection = cameraForward * rawMovementInput.z + cameraRight * rawMovementInput.x;
            moveDirection.Normalize(); // Normalize อีกครั้งเพื่อให้ความเร็วคงที่

            // 3. เคลื่อนย้ายตำแหน่งตัวละคร
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // 4. หมุนตัวละครให้หันหน้าไปในทิศทางที่กำลังเคลื่อนที่
            // ใช้ Quaternion.LookRotation เพื่อสร้างการหมุนที่ทำให้วัตถุหันไปในทิศทางที่กำหนด
            // Quaternion.Slerp เพื่อให้การหมุนนุ่มนวลขึ้น
            if (moveDirection != Vector3.zero) // ตรวจสอบว่ามีทิศทางการเคลื่อนที่จริง ก่อนหมุน
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}