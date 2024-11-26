using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateThree : MonoBehaviour
{

    public int tally = 2;


    // Update is called once per frame
    void Update()
    {
        if (tally == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
