using UnityEngine;

public class Battle : MonoBehaviour
{
    public CardUIManager cardUI; // เชื่อมกับ CardUI ใน Inspector

    public void StartBattle()
    {
        // เริ่มต้นฉากต่อสู้, โหลด Enemy 3D ฯลฯ
        Debug.Log("เริ่มการต่อสู้!");
        cardUI.SetTurn(1); // ตัวอย่างเรียกไปยัง CardUIManager
    }

    public void PlayerAttack()
    {
        // ตรรกะโจมตีในโลก 3D
        Debug.Log("ผู้เล่นโจมตี!");
        // หลังโจมตีเสร็จ อาจเรียกอัปเดตการ์ดหรือแจ้ง UI
        cardUI.ShowAttackAnimation();
    }
}

