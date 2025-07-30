// File: Assets/Scripts/Stats/CharacterStats.cs (หรือโฟลเดอร์ที่คุณเลือก)
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public string characterName = "Default Character";
    public float attack = 10f;

    public float maxBrainpower = 100f; // ค่าสูงสุดของ Brainpower
    public float currentBrainpower;    // ค่าปัจจุบันของ Brainpower

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        currentBrainpower = maxBrainpower; // กำหนดค่าเริ่มต้น Brainpower เต็ม
        Debug.Log($"{characterName} initialized with {currentHealth}/{maxHealth} HP and {currentBrainpower}/{maxBrainpower} Brainpower.");
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{characterName} took {damage} damage. Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    private void Die()
    {
        Debug.Log($"{characterName} has died!");
        gameObject.SetActive(false);
    }
}