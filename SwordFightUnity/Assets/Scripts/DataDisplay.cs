using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDisplay : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI textElement;
    [SerializeField]
    private FighterAgent agent1;
    [SerializeField]
    private FighterAgent agent2;

    // Update is called once per frame
    void Update()
    {
        textElement.text = "Agent1:\nReward: "
            + agent1.GetCumulativeReward().ToString("0.00") + "\n\nAgent2:\nReward: "
            + agent2.GetCumulativeReward().ToString("0.00");
    }
}
