using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public float maxHp;
    public Image hpBar;
    public float currentHp;
    // Start is called before the first frame update
    void Awake()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        hpBar.fillAmount = (currentHp/maxHp);

        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
    }
}
