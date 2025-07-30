// File: Assets/Scripts/Managers/GameStateManager.cs (หรือโฟลเดอร์ที่คุณต้องการ)
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // อ้างอิงถึง Script กล้องที่แนบอยู่บน Main Camera
    public ExplorationCamera explorationCamera;
    public CombatCamera_FixedBehindPlayer combatCameraFixed; // เปลี่ยนชื่อตัวแปรให้ชัดเจน

    // สถานะปัจจุบันของเกม
    public enum GameState
    {
        Exploration,
        Combat
    }
    public GameState currentGameState = GameState.Exploration;

    void Start()
    {
        // ค้นหา Component กล้องโดยใช้ FindAnyObjectByType (แนะนำ)
        if (explorationCamera == null)
        {
            explorationCamera = FindAnyObjectByType<ExplorationCamera>();
        }
        if (combatCameraFixed == null) // ใช้ combatCameraFixed
        {
            combatCameraFixed = FindAnyObjectByType<CombatCamera_FixedBehindPlayer>();
        }

        // กำหนดสถานะเริ่มต้น
        SetGameState(GameState.Exploration);
    }

    // ฟังก์ชันสำหรับเปลี่ยนสถานะของเกม
    public void SetGameState(GameState newState)
    {
        if (currentGameState == newState) return; // ถ้าสถานะเหมือนเดิม ไม่ต้องทำอะไร

        currentGameState = newState;
        Debug.Log("Game State Changed to: " + newState);

        switch (currentGameState)
        {
            case GameState.Exploration:
                EnableExplorationCamera();
                break;
            case GameState.Combat:
                EnableCombatCamera();
                break;
        }
    }

    private void EnableExplorationCamera()
    {
        if (explorationCamera != null)
        {
            explorationCamera.enabled = true; // เปิดใช้งาน Script กล้องสำรวจ
        }
        if (combatCameraFixed != null) // ใช้ combatCameraFixed
        {
            combatCameraFixed.enabled = false; // ปิดใช้งาน Script กล้องต่อสู้
        }
        // ตั้งค่า Cursor สำหรับ Exploration (อาจจะไม่ล็อกและมองเห็น)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void EnableCombatCamera()
    {
        if (explorationCamera != null)
        {
            explorationCamera.enabled = false; // ปิดใช้งาน Script กล้องสำรวจ
        }
        if (combatCameraFixed != null) // ใช้ combatCameraFixed
        {
            combatCameraFixed.enabled = true; // เปิดใช้งาน Script กล้องต่อสู้
            // เมื่อเข้าสู่โหมดต่อสู้ ให้รีเซ็ตการหมุนของกล้องต่อสู้ให้เหมาะสม (ถ้ามีฟังก์ชัน)
            // (CombatCamera_FixedBehindPlayer ไม่มีฟังก์ชัน Reset เพราะมันตาม Target อัตโนมัติ)
        }
        // ตั้งค่า Cursor ให้ถูกล็อกและซ่อน สำหรับ Combat
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // สำหรับทดสอบ: คุณสามารถเพิ่มปุ่มใน UI หรือใช้ Key เพื่อสลับโหมดได้
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // กด P เพื่อสลับโหมด (ตัวอย่าง)
        {
            if (currentGameState == GameState.Exploration)
            {
                SetGameState(GameState.Combat);
            }
            else
            {
                SetGameState(GameState.Exploration);
            }
        }
    }
}