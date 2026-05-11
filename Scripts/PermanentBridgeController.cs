using UnityEngine;

public class PermanentBridgeController : MonoBehaviour
{
    public PressureSwitch switchTrigger;
    public GameObject bridge;

    private bool activated = false;

    void Update()
    {
        if (!activated && switchTrigger.isPressed)
        {
            activated = true;
            bridge.SetActive(true);
        }
    }
}