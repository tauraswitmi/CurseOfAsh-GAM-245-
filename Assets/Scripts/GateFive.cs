using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateFive : MonoBehaviour
{

    public int tally = 1;

    // Update is called once per frame
    void Update()
    {
        if (tally == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
