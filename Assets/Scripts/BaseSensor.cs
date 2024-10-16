using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSensor : MonoBehaviour
{
    private Life life;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        life = other.GetComponent<Life>();
        InvokeRepeating(nameof(Heal), 1, 1);
    }

    private void OnTriggerExit(Collider other)
    {
        CancelInvoke();
    }

    private void Heal()
    {
        life.currentHp += 10;
    }
}
