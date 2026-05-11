using UnityEngine;

public class DualSwitchLadder : MonoBehaviour
{
    public PressureSwitch switchLeft;
    public PressureSwitch switchRight;

    public GameObject ladder;

    void Update()
    {
        if (switchLeft.isPressed || switchRight.isPressed)
        {
            ladder.SetActive(true);
        }
        else
        {
            ladder.SetActive(false);
        }
    }
}