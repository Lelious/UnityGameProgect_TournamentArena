using UnityEngine;
using UnityEngine.UI;

public class HealthBarCalculating : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;
    public float fillhp;
    public float fillmp;
    private int maxHp;
    private int maxMp;
    private int realHealth;
    private int realMana;
    void Start()
    {
        fillhp = 1;
        fillmp = 1;
        maxHp = gameObject.GetComponent<Stats>().health;
        maxMp = gameObject.GetComponent<Stats>().mana;
    }

    void Update()
    {
        realHealth = gameObject.GetComponent<Stats>().health;
        realMana = gameObject.GetComponent<Stats>().mana;

        if (realHealth > 0)
        {
            fillhp = (float)realHealth / maxHp;
            hpBar.fillAmount = fillhp;
        }
        
        if (realHealth > 1)
        {
            fillhp = 1;
        }

        if (realHealth <= 0)
        {
            fillhp = 0;
        }

        if (realMana > 0)
        {
            fillmp = (float)realMana / maxMp;
            mpBar.fillAmount = fillmp;
        }

        if (realMana <= 0)
        {
            fillmp = 0;
        }

        if (realMana > 1)
        {
            fillmp = 1;
        }

    }   
}
