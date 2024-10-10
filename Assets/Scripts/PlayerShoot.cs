using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject prefab;
    public GameObject spawnCoord;
    public float destroyTime;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject clone = Instantiate(prefab);
            clone.transform.position = spawnCoord.transform.position;
            clone.transform.rotation = spawnCoord.transform.rotation;

            Destroy(clone, destroyTime);
        }
    }
}
