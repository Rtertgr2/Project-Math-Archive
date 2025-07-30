using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI typeText;
    public Button discardButton;

    private Action<int> onDiscardClicked;
    private int cardIndex;

    public void Setup(EquationCard cardData, int index, Action<int> discardCallback)
    {
        cardIndex = index;
        onDiscardClicked = discardCallback;
        valueText.text = (cardData.Type == CardType.Number) ? cardData.Value.ToString() : "↔";
        typeText.text = (cardData.Type == CardType.Number) ? "ตัวเลข" : "ย้ายข้าง";

        if (discardButton != null)
        {
            discardButton.onClick.RemoveAllListeners();
            discardButton.onClick.AddListener(() => onDiscardClicked?.Invoke(cardIndex));
        }
    }
}
