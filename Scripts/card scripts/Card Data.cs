public enum CardType
{
    Number,
    EquationSwap
}

[System.Serializable]
public class EquationCard
{
    public CardType Type;
    public int Value;
}
