using TMPro;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public CombatManager combatManager;

    void Update()
    {
        if (combatManager.currentEnemy != null)
        {
            nameText.text = combatManager.currentEnemy.enemyName;
            hpText.text = $"HP: {combatManager.currentEnemy.hp} / {combatManager.currentEnemy.maxHp}";
        }
    }
}
