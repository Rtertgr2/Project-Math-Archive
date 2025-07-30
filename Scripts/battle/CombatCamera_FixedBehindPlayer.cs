// File: Assets/Scripts/Camera/CombatCamera_FixedBehindPlayer.cs (หรือโฟลเดอร์ที่คุณต้องการ)
using UnityEngine;

public class CombatCamera_FixedBehindPlayer : MonoBehaviour
{
    public Transform target; // ผู้เล่นที่เราต้องการให้กล้องติดตาม

    // ตำแหน่ง Offset ของกล้องจากผู้เล่น (สำคัญมากในการปรับมุม)
    public Vector3 offset = new Vector3(0f, 3.5f, -6f); // ค่าเริ่มต้นที่แนะนำสำหรับ "Floor 19 Solo"
    // X: ซ้าย/ขวา (0f คือตรงกลาง)
    // Y: ความสูงจากพื้นของตัวละคร (ปรับให้กล้องอยู่สูงกว่าตัวละคร)
    // Z: ระยะห่างด้านหลังตัวละคร (ค่าลบยิ่งมากยิ่งห่าง)

    public float smoothSpeed = 0.125f; // ความเร็วในการเคลื่อนที่ของกล้อง (ยิ่งค่าน้อย ยิ่งหน่วง)

    // Optional: สำหรับการจัดการการชนของกล้อง (เพิ่มเข้าไปได้ถ้าต้องการ)
    public LayerMask collisionLayer; // Layer ที่กล้องจะชน (เช่น Default, Environment)
    public float collisionRadius = 0.5f; // รัศมีของ Spherecast สำหรับการชน
    public float minDistance = 1.5f; // ระยะห่างขั้นต่ำที่กล้องจะอยู่จาก Target เมื่อชน

    void Start()
    {
        // ในโหมดต่อสู้ มักจะล็อกและซ่อนเคอร์เซอร์เพื่อให้ผู้เล่นมีสมาธิกับการต่อสู้
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate() // LateUpdate เหมาะสำหรับการอัปเดตกล้องหลังจากตัวละครเคลื่อนที่แล้ว
    {
        if (target == null)
        {
            Debug.LogWarning("CombatCamera_FixedBehindPlayer: Target (Player) is not assigned!");
            return;
        }

        // คำนวณตำแหน่งที่ต้องการของกล้อง
        // กล้องจะอยู่ด้านหลังและตามการหมุนของ Target
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // --- Optional: การจัดการการชนของกล้อง (Camera Collision) ---
        RaycastHit hit;
        Vector3 targetToCamera = desiredPosition - target.position;
        float currentCalculatedDistance = targetToCamera.magnitude;

        // ตรวจสอบการชนจากผู้เล่นไปยังตำแหน่งกล้องที่คำนวณได้
        if (Physics.SphereCast(target.position, collisionRadius, targetToCamera.normalized, out hit, currentCalculatedDistance, collisionLayer))
        {
            // ถ้าชน ให้ปรับระยะห่างของกล้องให้มาอยู่ตรงจุดที่ชน + ระยะเผื่อ
            float adjustedDistance = Mathf.Max(minDistance, hit.distance - collisionRadius * 0.5f);
            desiredPosition = target.position + (targetToCamera.normalized * adjustedDistance);
        }
        // --- จบ Optional ---

        // เคลื่อนที่กล้องไปยังตำแหน่งที่ต้องการอย่างนุ่มนวล
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // หมุนกล้องให้มองไปที่ผู้เล่นเสมอ (หรือจุดที่สูงกว่าผู้เล่นเล็กน้อย)
        // ปรับค่า 1.5f เพื่อเล็งไปที่ส่วนต่างๆ ของตัวละคร (เช่น หัว, หน้าอก)
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}