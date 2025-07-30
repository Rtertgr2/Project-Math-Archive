using UnityEngine;
using UnityEngine.UI;

public class BrainPowerBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxBrainPower(int maxPower)
    {
        slider.maxValue = maxPower;
        slider.value = maxPower;
    }

    public void SetBrainPower(int power)
    {
        slider.value = power;
    }
}
