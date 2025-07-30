using UnityEngine;
using TMPro;

public class CardUIManager : MonoBehaviour
{
    public Battle battleLogic;            // เชื่อมกับ Battle
    public TextMeshProUGUI turnText;

    public void SetTurn(int turnNumber)
    {
        if (turnText != null)
            turnText.text = $"Turn: {turnNumber}";
    }

    public void ShowAttackAnimation()
    {
        // ตัวอย่าง: แสดง effect ทำให้ผู้เล่นรู้ว่ามีการโจมตี
        Debug.Log("เล่นเอฟเฟกต์แอนิเมชันโจมตีที่ UI");
    }

    public void OnCardSelected(int cardIndex)
    {
        if (battleLogic != null)
            battleLogic.PlayerAttack(); // ส่งสัญญาณให้ Battle ดำเนิน Logic
    }
}
