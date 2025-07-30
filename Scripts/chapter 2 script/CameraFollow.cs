using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // ผู้เล่นที่เราต้องการให้กล้องติดตาม
    public Vector3 offset = new Vector3(0f, 3f, -5f); // ระยะห่างจากตัวละคร (X, Y, Z)
                                                      // Y: ความสูงจากตัวละคร
                                                      // Z: ระยะห่างด้านหลังตัวละคร (ค่าลบ = ด้านหลัง)
    public float smoothSpeed = 0.125f; // ความเร็วในการเคลื่อนที่ของกล้อง (ยิ่งค่าน้อย ยิ่งหน่วงมาก)

    // ไม่ต้องใช้ตัวแปรเหล่านี้แล้วเมื่อล็อกมุมกล้อง
    // public float mouseSensitivity = 100f;
    // private float currentYaw = 0f;
    // private float currentPitch = 0f;
    // public float minPitch = -30f;
    // public float maxPitch = 60f;

    // เราจะใช้ Quaternion หรือ Vector3 นี้เพื่อกำหนดการหมุนเริ่มต้นที่ "ตายตัว"
    public Quaternion fixedRotation = Quaternion.Euler(30f, 0f, 0f); // ตัวอย่าง: เอียง 30 องศา, ไม่หมุนซ้ายขวา

    void Start()
    {
        // ยกเลิกการล็อกเคอร์เซอร์เมาส์และซ่อนเคอร์เซอร์ หากคุณเคยใส่ไว้
        // เพราะเราจะไม่ใช้เมาส์ในการหมุนกล้องแล้ว
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // กำหนดการหมุนเริ่มต้นของกล้องให้เป็น fixedRotation ทันที
        transform.rotation = fixedRotation;

        // ไม่ต้องกำหนด currentYaw จาก target.eulerAngles.y แล้ว
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("ThirdPersonCamera: Target (Player) is not assigned!");
            return;
        }

        // <<< ส่วนนี้คือส่วนที่คุณต้องคอมเมนต์หรือลบออกไป เพราะมันคือการรับ Input เมาส์และหมุนกล้อง
        // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // currentYaw += mouseX;
        // currentPitch -= mouseY;
        // currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        // Quaternion desiredRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        // transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed);
        // ***********************************************************************************

        // ส่วนที่เหลือคือการคำนวณตำแหน่งกล้องที่ติดตามผู้เล่น
        Vector3 desiredPosition = target.position;

        // นำ offset มาแปลงตามการหมุนของกล้อง (ซึ่งตอนนี้เป็น fixedRotation)
        // แล้วบวกเข้ากับตำแหน่ง Target
        desiredPosition += fixedRotation * offset; // ใช้ fixedRotation แทน transform.rotation

        // กำหนดตำแหน่งของกล้องอย่างนุ่มนวล
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // หรือถ้าคุณต้องการให้กล้องมองไปยังผู้เล่นเสมอ ไม่ว่าจะตั้ง fixedRotation อย่างไร
        // คุณสามารถใช้ LookAt ได้ แต่จะต้องระวังว่ามันจะ "แทนที่" การหมุนที่ fixedRotation ตั้งไว้
        // หากต้องการมุมมอง Top-down ที่กล้อง "เอียงลง" คุณควรใช้ fixedRotation เท่านั้น
        // แต่ถ้าต้องการให้กล้อง "มองตรงไปที่ผู้เล่นเสมอ" ในระนาบแนวนอน
        // transform.LookAt(target.position + Vector3.up * 1f); // มองที่จุดเหนือตัวละคร
    }
}