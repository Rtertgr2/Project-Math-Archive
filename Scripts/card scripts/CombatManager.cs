using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public CardManager cardManager;
    public SimpleEquation currentEquation;
    public int brainPower = 100;
    public Enemy currentEnemy;
    public EnemyUI enemyUI;


    public bool isSpecialEnemy = false; // << เพิ่มบรรทัดนี้ให้แน่ใจว่ามีตัวแปรนี้

    [SerializeField] private HandPanelUI handPanelUI;

    void Start()
    {
        StartNewCombat();
    }

    public void StartNewCombat()
    {
        currentEquation = new SimpleEquation();
        currentEquation.GenerateNewEquation();

        currentEnemy = new Enemy("Slime", 5, false);

        cardManager.BuildDeck();
        for (int i = 0; i < cardManager.maxHandSize; i++)
        {
            cardManager.DrawCard(isSpecialEnemy, currentEquation.answer);
        }
        // *** เพิ่มบรรทัดนี้ ***
        if (handPanelUI != null)
            handPanelUI.UpdateDisplay();
    }

    // ฟังก์ชันนี้จะถูกเรียกโดย TurnManager
    public void PlayerTurnStart()
    {
        cardManager.StartNewTurn();
        cardManager.DrawCard(isSpecialEnemy, currentEquation.answer);
        if (handPanelUI != null)
            handPanelUI.UpdateDisplay();
    }

    public void OnPlayerDiscardAndDraw(int handIndex)
    {
        bool success = cardManager.DiscardAndDraw(handIndex, isSpecialEnemy, currentEquation.answer);
        if (success && handPanelUI != null)
        {
            handPanelUI.UpdateDisplay(); 
        }
    }

    public void OnDrawCardButtonPressed()
    {
        cardManager.DrawCard(isSpecialEnemy, currentEquation.answer);
        if (handPanelUI != null)
            handPanelUI.UpdateDisplay();
    }

    // ...ส่วนอื่นๆที่อาจมี เช่น ใช้การ์ด, ตรวจสอบคำตอบ ...
    public void DealDamageToEnemy(int amount)
    {
        currentEnemy.hp -= amount;
        if (currentEnemy.hp <= 0)
        {
            Debug.Log($"{currentEnemy.enemyName} ถูกกำจัดแล้ว!");
            SpawnNextEnemy();
        }
        // แก้ error ด้วยการมีฟังก์ชันนี้!
        UpdateEnemyUI();
    }

    public void SpawnNextEnemy()
    {
        // (ยกตัวอย่าง) สุ่มหรือดึงศัตรูใหม่จาก Array/List
        string[] names = { "Slime", "Bat", "Goblin", "Boss" };
        int[] hps = { 5, 3, 8, 20 };
        bool[] specials = { false, false, false, true };

        int idx = Random.Range(0, names.Length); // สุ่มศัตรูใหม่
        currentEnemy = new Enemy(names[idx], hps[idx], specials[idx]);
        UpdateEnemyUI();
    }

    // เพิ่มฟังก์ชันนี้เข้าไปใน CombatManager.cs (ที่ไหนก็ได้ในคลาส)
    public void UpdateEnemyUI()
    {
        // ตัวอย่างสำหรับอัปเดต UI ข้อมูลศัตรู:
        // - หากคุณมี EnemyUI.cs หรือฟิลด์ TextMeshProUGUI สำหรับชื่อและ HP ศัตรู
        // - แนะนำให้เชื่อม EnemyUI จาก Inspector หรือเรียก method ใน EnemyUI class

        // ตัวอย่างพื้นฐาน (หากคุณมี EnemyUI เป็นตัวแปรใน CombatManager)
        if (enemyUI != null && currentEnemy != null)
        {
            enemyUI.nameText.text = currentEnemy.enemyName;
            enemyUI.hpText.text = $"HP: {currentEnemy.hp} / {currentEnemy.maxHp}";
        }

        // ... หรือหากใช้นอกคลาส ให้แก้เป็น public static หรือตามโครงสร้างของคุณ
    }

}
