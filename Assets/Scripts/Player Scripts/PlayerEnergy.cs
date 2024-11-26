using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public float energy = 100;
    public float energyMAX = 100;

    public GameObject text;
    public Text placeText;

    void Update()
    {
        placeText.text = "Energy: " + Mathf.RoundToInt(energy) + "%";
    }

    public void EnergyDrainSprint()
    {
        if (energy > 0)
        {
            energy -= 0.4f;
        } else
        {
            EnergyMINOUTcheck();
        }
    }

    public void EnergyDrainDash()
    {
        if (energy > 0)
        {
            energy -= 50;
            if (energy < 0) {
                EnergyMINOUTcheck();
            }
        }
    }

    public void EnergyRecover()
    {
        if (energy < energyMAX)
        {
            energy += 0.4f;
        } else
        {
            EnergyMAXOUTcheck();
        }
    }

    public void EnergyRecoverEMPTY()
    {
        if (energy < energyMAX){
            energy += 0.32f;
        } else
        {
            EnergyMAXOUTcheck();
        }
    }

    private void EnergyMAXOUTcheck()
    {
        if (energy > energyMAX)
        {
            energy = energyMAX;
        }
    }

    private void EnergyMINOUTcheck()
    {
        if (energy < 0)
        {
            energy = 0;
        }
    }

    
}
