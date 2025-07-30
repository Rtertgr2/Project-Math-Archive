using System.Collections.Generic;
using UnityEngine;

public class HandPanelUI : MonoBehaviour
{
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private GameObject cardPrefab;

    private List<GameObject> cardObjects = new List<GameObject>();

    public void UpdateDisplay()
    {
        // ลบการ์ดเก่าทุกใบ
        foreach (var cardObj in cardObjects)
        {
            Destroy(cardObj);
        }
        cardObjects.Clear();

        if (combatManager == null || combatManager.cardManager == null)
            return;

        List<EquationCard> currentHand = combatManager.cardManager.hand;
        for (int i = 0; i < currentHand.Count; i++)
        {
            AddCardToUI(currentHand[i], i);
        }
    }

    // เพิ่มการ์ดใบเดียวเข้า UI
    public void AddCardToUI(EquationCard cardData, int index)
    {
        GameObject cardObject = Instantiate(cardPrefab, transform);
        cardObject.name = $"Card_{index}";

        CardUI cardUI = cardObject.GetComponent<CardUI>();
        if (cardUI != null)
        {
            cardUI.Setup(cardData, index, OnDiscardCard);
        }
        cardObjects.Add(cardObject);
    }

    // ลบการ์ดใบเดียวออกจาก UI ด้วย index
    public void RemoveCardFromUI(int index)
    {
        if (index < 0 || index >= cardObjects.Count) return;
        Destroy(cardObjects[index]);
        cardObjects.RemoveAt(index);
    }

    // Callback ที่จะถูกเรียกเมื่อปุ่มทิ้งการ์ดถูกกด
    private void OnDiscardCard(int handIndex)
    {
        combatManager.OnPlayerDiscardAndDraw(handIndex);
    }
}
