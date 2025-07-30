using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public int maxHandSize = 4;
    public int maxSwapCardsInHand = 2;

    public List<EquationCard> deck = new List<EquationCard>();
    public List<EquationCard> hand = new List<EquationCard>();
    public List<EquationCard> discardPile = new List<EquationCard>();

    private bool hasDiscardedThisTurn = false;

    // ต้องแน่ใจว่าคุณมี enum CardType และ class EquationCard ในโปรเจกต์ของคุณ
    // เช่น ในไฟล์ CardData.cs หรือในไฟล์อื่นที่เข้าถึงได้

    public void BuildDeck()
    {
        deck.Clear();
        // เพิ่มการ์ดตัวเลข 0-9 อย่างละ 1 ใบ
        for (int i = 0; i < 10; i++)
        {
            deck.Add(new EquationCard { Type = CardType.Number, Value = i });
        }
        // เพิ่มการ์ด EquationSwap 3 ใบ
        for (int i = 0; i < 3; i++)
        {
            deck.Add(new EquationCard { Type = CardType.EquationSwap });
        }
        ShuffleDeck();
        Debug.Log($"Deck built with {deck.Count} cards."); // Debug Log เพิ่มเติม
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rand = Random.Range(i, deck.Count);
            var temp = deck[i];
            deck[i] = deck[rand];
            deck[rand] = temp;
        }
        Debug.Log("Deck shuffled."); // Debug Log เพิ่มเติม
    }

    public void StartNewTurn()
    {
        hasDiscardedThisTurn = false;
        Debug.Log("New turn started. Discard allowance reset."); // Debug Log เพิ่มเติม
    }

    /// <summary>
    /// ทิ้งการ์ดที่ handIndex และจั่วการ์ดใหม่
    /// </summary>
    /// <param name="handIndex">Index ของการ์ดในมือที่ต้องการทิ้ง</param>
    /// <param name="isSpecialEnemy">เป็นศัตรูพิเศษหรือไม่ (มีผลต่อการจั่วการ์ด)</param>
    /// <param name="correctAnswer">คำตอบที่ถูกต้องของสมการปัจจุบัน (มีผลต่อการจั่วการ์ดสำหรับศัตรูพิเศษ)</param>
    /// <returns>true ถ้าทิ้งและจั่วสำเร็จ, false ถ้าไม่สำเร็จ (เช่น ทิ้งไปแล้วในเทิร์นนี้)</returns>
    public bool DiscardAndDraw(int handIndex, bool isSpecialEnemy, int correctAnswer)
    {
        if (hasDiscardedThisTurn)
        {
            Debug.LogWarning("Cannot discard: Already discarded this turn.");
            return false;
        }
        if (handIndex < 0 || handIndex >= hand.Count)
        {
            Debug.LogError($"Invalid hand index for discard: {handIndex}. Hand size: {hand.Count}");
            return false;
        }

        EquationCard cardToDiscard = hand[handIndex];
        hand.RemoveAt(handIndex);
        discardPile.Add(cardToDiscard);
        Debug.Log($"Discarded card: {cardToDiscard.Type} (Value: {cardToDiscard.Value}). Hand size now: {hand.Count}"); // Debug Log เพิ่มเติม

        // จั่วการ์ดใหม่ทันที
        DrawCard(isSpecialEnemy, correctAnswer);

        hasDiscardedThisTurn = true;
        return true;
    }

    /// <summary>
    /// จั่วการ์ด 1 ใบเข้ามือ
    /// </summary>
    /// <param name="isSpecialEnemy">เป็นศัตรูพิเศษหรือไม่ (มีผลต่อการจั่วการ์ด)</param>
    /// <param name="correctAnswer">คำตอบที่ถูกต้องของสมการปัจจุบัน (มีผลต่อการจั่วการ์ดสำหรับศัตรูพิเศษ)</param>
    public void DrawCard(bool isSpecialEnemy, int correctAnswer)
    {
        if (hand.Count >= maxHandSize)
        {
            Debug.LogWarning("Cannot draw card: Hand is full.");
            return;
        }

        // ถ้า Deck หมด ให้สับไพ่จาก discardPile กลับมา
        if (deck.Count == 0)
        {
            if (discardPile.Count > 0)
            {
                deck.AddRange(discardPile);
                discardPile.Clear();
                ShuffleDeck();
                Debug.Log($"Deck ran out. Reshuffling {deck.Count} cards from discard pile."); // Debug Log เพิ่มเติม
            }
            else
            {
                Debug.LogWarning("Cannot draw card: Deck and Discard Pile are both empty.");
                return;
            }
        }

        // กรองการ์ดที่มีสิทธิ์จั่ว:
        // - การ์ดตัวเลขต้องไม่ซ้ำกับตัวเลขที่มีอยู่ในมือแล้ว
        // - การ์ด EquationSwap ต้องไม่เกิน maxSwapCardsInHand
        HashSet<int> handNumbers = new HashSet<int>(hand.Where(c => c.Type == CardType.Number).Select(c => c.Value));
        int swapCount = hand.Count(c => c.Type == CardType.EquationSwap);

        List<EquationCard> candidates = deck.Where(card =>
            (card.Type == CardType.Number && !handNumbers.Contains(card.Value)) ||
            (card.Type == CardType.EquationSwap && swapCount < maxSwapCardsInHand)
        ).ToList();

        if (candidates.Count == 0)
        {
            Debug.LogWarning("No eligible cards to draw from the deck.");
            return;
        }

        EquationCard cardToDraw = null;

        // Logic การจั่วพิเศษสำหรับศัตรูพิเศษ: 60% โอกาสจั่วการ์ดคำตอบ
        if (isSpecialEnemy && Random.value < 0.6f)
        {
            var answerCard = candidates.FirstOrDefault(c => c.Type == CardType.Number && c.Value == correctAnswer);
            if (answerCard != null)
            {
                cardToDraw = answerCard;
                Debug.Log($"Special enemy draw: Found and drawing answer card ({correctAnswer})."); // Debug Log เพิ่มเติม
            }
        }

        // ถ้ายังไม่ได้เลือกการ์ด (หรือไม่มีการ์ดคำตอบ) ให้สุ่มจั่วจากการ์ดที่เหลือ
        if (cardToDraw == null)
        {
            cardToDraw = candidates[Random.Range(0, candidates.Count)];
            Debug.Log($"Normal draw: Drawing {cardToDraw.Type} (Value: {cardToDraw.Value})."); // Debug Log เพิ่มเติม
        }

        deck.Remove(cardToDraw);
        hand.Add(cardToDraw);
        Debug.Log($"Successfully drew a card. Hand size: {hand.Count}"); // Debug Log เพิ่มเติม
    }
}