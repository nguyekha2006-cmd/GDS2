using UnityEngine;

public class LadderController : MonoBehaviour
{
    public LadderSwitch switchTrigger;
    public GameObject ladder;

    void Update()
    {
        if (switchTrigger.isPressed)
        {
            ladder.SetActive(true);
        }
        else
        {
            ladder.SetActive(false);
        }
    }
}