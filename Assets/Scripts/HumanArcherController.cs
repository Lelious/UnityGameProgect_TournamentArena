using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HumanArcherController : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject point;
    public NavMeshAgent myAgent;
    public GameObject myTarget;
    public Animator myAnim;
    public MeshCollider mesh;
    public GameObject displayFloatTextObjectDamage;
    public GameObject displayFloatTextObjectCriticalDamage;

    private int _minDamage;
    private int _maxDamage;
    private int _attackPower;
    private float _attackSpeed;
    private int _armor;
    private int _health;
    private int _mana;
    private int _maxHealth;
    private int _maxMana;
    private float _runSpeed;
    private int _criticalChance;
    private int _criticalMultiplier;
    private int _healthRegeneration;
    private int _manaRegeneration;
    public bool dead;
    public bool isAttacking;

    void Start()
    {
        InvokeRepeating("HealthAndManaRegeneration", 0, 1);
        InvokeRepeating("ActualStatsUpdate", 0, 1);
        _healthRegeneration = GetComponent<Stats>().healthRegeneration;
        _manaRegeneration = GetComponent<Stats>().manaRegeneration;
        _attackPower = GetComponent<Stats>().attackPower;
        _attackSpeed = GetComponent<Stats>().attackSpeed;
        _criticalChance = GetComponent<Stats>().criticalChance;
        _criticalMultiplier = GetComponent<Stats>().criticalMultiplier;
        _runSpeed = GetComponent<Stats>().runSpeed;
        _health = GetComponent<Stats>().health;
        _armor = GetComponent<Stats>().armor;
        _mana = GetComponent<Stats>().mana;
        _maxHealth = _health;
        _maxMana = _mana;
    }

    IEnumerator AttackDetect()
    {
        myAnim.SetFloat("attack_speed", _attackSpeed);

        if (myTarget.tag != "Untagged" && myTarget != null)
        {
            transform.LookAt(myTarget.transform);
            StopCoroutine(FindTarget());

            if (Vector3.Distance(myTarget.transform.position, transform.position) < 10f)
            {
                myAnim.SetBool("run", false);
                myAnim.SetBool("attack", true);
            }

            if (Vector3.Distance(myTarget.transform.position, transform.position) >= 10f)
            {
                myAnim.SetBool("attack", false);
                myAnim.SetBool("run", true);
            }
        }

        else
        {
            myAnim.SetBool("attack", false);
            myAnim.SetBool("run", false);
            StartCoroutine(FindTarget());
        }
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator FindTarget()
    {
        myTarget = GetComponent<FindNearestEnemy>().FindEnemy();
        yield return new WaitForSeconds(0.3f);
    }

    void Update()
    {
        StartCoroutine(FindTarget());
        StartCoroutine(AttackDetect());
    }

    private void HealthAndManaRegeneration()
    {
        if (GetComponent<Stats>().health < _maxHealth)
        {
            GetComponent<Stats>().health += _healthRegeneration;
        }

        if (GetComponent<Stats>().mana < _maxMana)
        {
            GetComponent<Stats>().mana += _manaRegeneration;
        }
    }

    private void ActualStatsUpdate()
    {
        myAnim.SetFloat("attack_speed", _attackSpeed);
        myAnim.SetFloat("run_speed", _runSpeed);
        _healthRegeneration = GetComponent<Stats>().healthRegeneration;
        _manaRegeneration = GetComponent<Stats>().manaRegeneration;
        _attackPower = GetComponent<Stats>().attackPower;
        _attackSpeed = GetComponent<Stats>().attackSpeed;
        _criticalChance = GetComponent<Stats>().criticalChance;
        _criticalMultiplier = GetComponent<Stats>().criticalMultiplier;
        _runSpeed = GetComponent<Stats>().runSpeed;
        _health = GetComponent<Stats>().health;
        _armor = GetComponent<Stats>().armor;
        _mana = GetComponent<Stats>().mana;
    }

    public void BowShoot()
    {
        if (_mana > 0)
        {
            _mana -= 5;
        }

        gameObject.GetComponent<Stats>().mana = _mana;
        
        int sendDamage = Random.Range(_attackPower - 10, _attackPower + 10);

        GameObject arr = Instantiate(Arrow, point.transform.position, Quaternion.identity) as GameObject;
        arr.GetComponent<ArrowFly>().target = myTarget;
        arr.GetComponent<ArrowFly>().damage = sendDamage;
        arr.GetComponent<ArrowFly>().critChance = _criticalChance;
        arr.GetComponent<ArrowFly>().critMultiplier = _criticalMultiplier;
        arr.GetComponent<ArrowFly>().shooter = gameObject;
    }

    public void RecieveDamage(int damage)
    {
        float reductedDmg = _armor / 100;
        float reductedDamageFloat = damage * reductedDmg;
        damage -= (int)reductedDamageFloat;
        _health -= damage;
        gameObject.GetComponent<Stats>().health = _health;
        GameObject text = Instantiate(displayFloatTextObjectDamage, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        text.GetComponent<FloatingText>().floatingTextMessage = damage.ToString();

        if (_health <= 0)
        {
            CancelInvoke("HealthAndManaRegeneration");
            gameObject.GetComponent<HealthBarCalculating>().hpBar.fillAmount = 0;
            GetComponent<HealthBarCalculating>().hpBar.gameObject.SetActive(false);
            gameObject.tag = "Untagged";
            myAnim.SetTrigger("death");
            myAgent.isStopped = true;
            StopAllCoroutines();
            Destroy(gameObject, 2f);
        }
    }

    public void RecieveCriticalDamage(int damage)
    {
        float reductedDmg = _armor / 100;
        float reductedDamageFloat = damage * reductedDmg;
        damage -= (int)reductedDamageFloat;
        _health -= damage;
        gameObject.GetComponent<Stats>().health = _health;
        GameObject text = Instantiate(displayFloatTextObjectCriticalDamage, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        text.GetComponent<FloatingText>().floatingTextMessage = $"Critical!\n{damage.ToString()}";

        if (_health <= 0)
        {
            CancelInvoke("HealthAndManaRegeneration");           
            gameObject.GetComponent<HealthBarCalculating>().hpBar.fillAmount = 0;
            GetComponent<HealthBarCalculating>().hpBar.gameObject.SetActive(false);
            gameObject.tag = "Untagged";
            myAnim.SetTrigger("death");
            myAgent.isStopped = true;
            StopAllCoroutines();
            Destroy(gameObject, 2f);
        }
    }
}
