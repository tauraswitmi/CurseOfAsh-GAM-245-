using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    move,
    chase,
    evade,
    evadeRun,
    attack,
    tagged,
    back,
    dead
}

public class EnemyScript : MonoBehaviour
{
    public EnemyState currentState;
    public string enemyName;
    public int healthDamage;
    public int corruptionDamage;
    public float speed;
    public bool hasCoroutineActive;

    public int room;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
