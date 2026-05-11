using UnityEngine;

public class DualSwitchBridge_New : MonoBehaviour
{
    public PressureSwitch switchLeft;
    public PressureSwitch switchRight;

    public GameObject bridge;

    void Update()
    {
        if (switchLeft.isPressed || switchRight.isPressed)
        {
            bridge.SetActive(true);
        }
        else
        {
            bridge.SetActive(false);
        }
    }
}