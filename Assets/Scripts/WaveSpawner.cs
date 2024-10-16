using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject prefab;
    public GameObject player;
    public float startTime;
    public float endTime;
    public float spawnRate;
    // Start is called before the first frame update
    void Start()
    {
        WaveManager.instance.waves.Add(this);
        InvokeRepeating(nameof(Spawn), startTime, spawnRate);
        Invoke(nameof(EndSpawn), endTime);
    }
    void Spawn()
    {
        var enemy = Instantiate(prefab, transform.position, transform.rotation);
        var enemyScript = enemy.GetComponent<EnemyFSM>();
        enemyScript.playerObject = player;
    }

    // Update is called once per frame
    void EndSpawn()
    {
        WaveManager.instance.waves.Remove(this);
        CancelInvoke();
    }
}
