using UnityEngine;
using TMPro;
using System.Collections;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    Busy
}

public class TurnManager : MonoBehaviour
{
    public int turnNumber = 1;
    public TurnState currentState = TurnState.PlayerTurn;
    public CombatManager combatManager;
    public TextMeshProUGUI turnText;

    void Start()
    {
        UpdateTurnText();
        BeginPlayerTurn();
    }

    public void BeginPlayerTurn()
    {
        currentState = TurnState.PlayerTurn;
        if (combatManager != null)
            combatManager.PlayerTurnStart();
        UpdateTurnText();
    }

    public void EndPlayerTurn()
    {
        currentState = TurnState.Busy;
        turnNumber++;
        UpdateTurnText();
        StartCoroutine(EnemyTurnRoutine());
    }

    public void OnEndTurnButtonPressed()
    {
        if (currentState == TurnState.PlayerTurn)
            EndPlayerTurn();
    }

    private IEnumerator EnemyTurnRoutine()
    {
        currentState = TurnState.EnemyTurn;
        UpdateTurnText();

        // ตัวอย่าง AI ศัตรู: สุ่มโจทย์ใหม่
        if (combatManager != null && combatManager.currentEquation != null)
            combatManager.currentEquation.GenerateNewEquation();

        yield return new WaitForSeconds(1.0f);
        BeginPlayerTurn();
    }

    void UpdateTurnText()
    {
        if (turnText != null)
            turnText.text = $"TURN {turnNumber} {(currentState == TurnState.PlayerTurn ? "(Player)" : "(Enemy)")}";
    }
}
