using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCorruption : MonoBehaviour
{
    public float corruption = 0;

    public GameObject text;
    public Text placeText;

    void Update()
    {
        placeText.text = "Corruption: " + Mathf.RoundToInt(corruption) + "%";
    }
    
    public void CorruptionFill(float increase)
    {
        if (corruption < 100)
        {
            corruption += increase;

            if (corruption > 100) { corruption = 100; }
        }
    }

    public void CorruptionDrain()
    {
        if (corruption > 0)
        {
            corruption -= 0.06f;
        } else if (corruption < 0)
        {
            corruption = 0;
        }
    }

}
