using UnityEngine;

public class SimpleEquation
{
    public string equationString;
    public int answer;

    private int leftConstant;
    private int rightConstant;
    private bool isXFirst;
    private bool isPlus;

    public void GenerateNewEquation()
    {
        isXFirst = Random.value > 0.5f;
        isPlus = Random.value > 0.5f;
        answer = Random.Range(0, 10);

        int constant = Random.Range(1, 11);

        if (isXFirst)
        {
            leftConstant = constant;
            rightConstant = isPlus ? (answer + constant) : (answer - constant);
            equationString = isPlus ? $"x + {leftConstant} = {rightConstant}" : $"x - {leftConstant} = {rightConstant}";
        }
        else
        {
            leftConstant = constant;
            rightConstant = isPlus ? (constant + answer) : (constant - answer);
            equationString = isPlus ? $"{leftConstant} + x = {rightConstant}" : $"{leftConstant} - x = {rightConstant}";
        }
    }

    public void ApplyEquationSwap()
    {
        rightConstant -= leftConstant;
        leftConstant = 0;
        equationString = $"x = {rightConstant}";
    }

    public bool ValidateAnswer(int playerAnswer)
    {
        if (leftConstant == 0)
            return playerAnswer == rightConstant;
        return playerAnswer == answer;
    }
}
