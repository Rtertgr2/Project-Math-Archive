using UnityEngine;
using UnityEngine.UI;

// บรรทัดนี้จะบังคับให้ GameObject ที่มีสคริปต์นี้ ต้องมี Component Button อยู่ด้วยเสมอ
[RequireComponent(typeof(Button))]
public class DiscardCardButton : MonoBehaviour
{
    public CombatManager combatManager;

    [HideInInspector]
    public int handIndex;

    private Button discardButton;

    void Awake()
    {
        // ดึง Component Button ของตัวเองมาใช้งาน
        discardButton = GetComponent<Button>();

        // เพิ่มการตรวจสอบเพื่อความปลอดภัย
        if (discardButton != null)
        {
            // เพิ่มคำสั่งที่จะทำงานเมื่อปุ่มนี้ถูกกด
            discardButton.onClick.AddListener(OnClickDiscard);
        }
        else
        {
            // แจ้งเตือนถ้ายังหา Button ไม่เจอ
            Debug.LogError("หา Component 'Button' ไม่พบบน GameObject นี้!", this.gameObject);
        }
    }

    public void SetHandIndex(int idx)
    {
        handIndex = idx;
    }

    private void OnClickDiscard()
    {
        if (combatManager != null)
        {
            // เรียกใช้ฟังก์ชันทิ้งการ์ดใน CombatManager
            combatManager.OnPlayerDiscardAndDraw(handIndex);
        }
    }
}
