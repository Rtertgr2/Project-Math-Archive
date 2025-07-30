// File: Assets/Scripts/Camera/ExplorationCamera.cs (หรือโฟลเดอร์ที่คุณต้องการ)
using UnityEngine;

public class ExplorationCamera : MonoBehaviour
{
    public Transform target; // ผู้เล่น
    public Vector3 offset = new Vector3(0f, 10f, -10f); // มุมมอง Top-down / Isometric (ปรับได้)
    public float smoothSpeed = 5f; // ความเร็วในการเคลื่อนที่ของกล้อง (ปรับเป็นค่าที่สมเหตุสมผล)

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("ExplorationCamera: Target (Player) is not assigned!");
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // ให้กล้องมองไปที่ผู้เล่น
        transform.LookAt(target.position);
    }
}