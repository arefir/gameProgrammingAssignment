using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    { 
        EnemyManager.instance.enemies.Add(this);  
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        EnemyManager.instance.enemies.Remove(this);
    }
}
