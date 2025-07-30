// File: Assets/Scripts/Camera/CombatCameraManager.cs
using UnityEngine;
using System.Collections; // สำหรับ Coroutine
using System.Collections.Generic; // สำหรับ List

public class CombatCameraManager : MonoBehaviour
{
    public Transform defaultCombatViewPoint; // ตำแหน่ง/การหมุนสำหรับมุมมองหลักของสนามรบ (Isometric)
    public float panSpeed = 3f; // ความเร็วในการแพนกล้อง
    public float zoomSpeed = 3f; // ความเร็วในการซูมกล้อง
    public float zoomDistance = 5f; // ระยะซูมเมื่อโฟกัสยูนิต

    // อ้างอิงถึงกล้องแบบ FixedBehindPlayer
    public CombatCamera_FixedBehindPlayer fixedBehindPlayerCamera;

    private Transform currentTarget; // ยูนิตที่กล้องกำลังโฟกัสอยู่

    void Start()
    {
        // ตรวจสอบว่า fixedBehindPlayerCamera ถูกลากมาใส่ใน Inspector แล้ว
        if (fixedBehindPlayerCamera == null)
        {
            fixedBehindPlayerCamera = FindAnyObjectByType<CombatCamera_FixedBehindPlayer>();
            if (fixedBehindPlayerCamera == null)
            {
                Debug.LogError("CombatCameraManager: CombatCamera_FixedBehindPlayer not found in scene or not assigned!");
            }
        }
    }

    // ฟังก์ชันสำหรับตั้งค่ามุมมองหลักของสนามรบ (Isometric/Top-down)
    public void SetDefaultCombatView()
    {
        if (defaultCombatViewPoint == null)
        {
            Debug.LogError("Default Combat View Point is not assigned!");
            return;
        }
        StopAllCoroutines(); // หยุด Coroutine ก่อนหน้า
        StartCoroutine(MoveCameraToTarget(defaultCombatViewPoint.position, defaultCombatViewPoint.rotation));

        // เมื่อกลับมาที่มุมมอง Isometric นี้ ให้ปิดกล้องแบบ FixedBehindPlayer (ถ้าเปิดอยู่)
        if (fixedBehindPlayerCamera != null && fixedBehindPlayerCamera.enabled)
        {
            fixedBehindPlayerCamera.enabled = false;
        }
    }

    // ฟังก์ชันสำหรับโฟกัสกล้องไปที่ยูนิตที่ระบุ (ใช้สำหรับ Cinematic Shot หรือ Focus)
    public void FocusOnUnit(Transform unitTransform)
    {
        if (unitTransform == null)
        {
            Debug.LogWarning("Unit Transform is null for camera focus!");
            return;
        }
        currentTarget = unitTransform;

        // คำนวณตำแหน่งกล้องใหม่ (ด้านหลัง/บนของยูนิต) สำหรับการโฟกัสชั่วคราว
        Vector3 targetPosition = unitTransform.position + Vector3.up * 3f + unitTransform.forward * -zoomDistance;
        Quaternion targetRotation = Quaternion.LookRotation(unitTransform.position - targetPosition);

        StopAllCoroutines(); // หยุด Coroutine ก่อนหน้า
        StartCoroutine(MoveCameraToTarget(targetPosition, targetRotation));

        // เมื่อโฟกัสยูนิต อาจจะปิด FixedBehindPlayerCamera ชั่วคราว (ถ้าเปิดอยู่)
        if (fixedBehindPlayerCamera != null && fixedBehindPlayerCamera.enabled)
        {
            fixedBehindPlayerCamera.enabled = false;
        }
    }

    // Coroutine สำหรับการเคลื่อนที่/หมุนกล้องอย่างนุ่มนวล
    IEnumerator MoveCameraToTarget(Vector3 position, Quaternion rotation)
    {
        while (Vector3.Distance(transform.position, position) > 0.1f || Quaternion.Angle(transform.rotation, rotation) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, position, panSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, zoomSpeed * Time.deltaTime);
            yield return null; // รอ 1 เฟรม
        }
        transform.position = position; // ตั้งค่าตำแหน่งสุดท้ายให้ตรงเป๊ะ
        transform.rotation = rotation; // ตั้งค่าการหมุนสุดท้ายให้ตรงเป๊ะ
    }

    // ฟังก์ชันสำหรับเปิดใช้งานกล้องแบบ FixedBehindPlayer
    public void EnableFixedBehindPlayerCamera()
    {
        if (fixedBehindPlayerCamera != null)
        {
            StopAllCoroutines(); // หยุด Coroutine ของ CombatCameraManager ที่อาจจะกำลังเคลื่อนกล้องอยู่
            fixedBehindPlayerCamera.enabled = true; // เปิดใช้งาน Script กล้องแบบ FixedBehindPlayer
        }
        Debug.Log("Fixed Behind Player Camera Enabled.");
    }
}