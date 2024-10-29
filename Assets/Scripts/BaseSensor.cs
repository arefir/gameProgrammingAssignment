using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BaseSensor : MonoBehaviour
{
    public VisualEffect burstEffect;
    private Life life;

    private void Awake()
    {
        burstEffect.Stop(); ///
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        life = other.GetComponent<Life>();
        InvokeRepeating(nameof(Heal), 0, 1);
        burstEffect.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        CancelInvoke();
        burstEffect.Stop();
    }

    private void Heal()
    {
        life.currentHp += 10;
    }
}
