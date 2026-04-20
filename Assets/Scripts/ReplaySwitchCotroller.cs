using UnityEngine;

public class RelaySwitchController : MonoBehaviour
{
    public PressureSwitch switchA;
    public PressureSwitch switchB;

    public GameObject bridge;

    public float delay = 0.2f;
    private float timer;

    void Update()
    {
        if (switchA.isPressed || switchB.isPressed)
        {
            timer = delay;
            bridge.SetActive(true);
        }
        else
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                bridge.SetActive(false);
            }
        }
    }
}