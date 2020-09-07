using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HumanKnightController : MonoBehaviour
{
    public GameObject Arrow;
    public NavMeshAgent myAgent;
    public GameObject myTarget;
    public Animator myAnim;
    public GameObject point;
    public MeshCollider mesh;
    public GameObject displayFloatTextObjectDamage;
    public GameObject displayFloatTextObjectCriticalDamage;

    private int _minDamage;
    private int _maxDamage;
    private int _attackPower;
    private float _attackSpeed;
    private float _armor;
    private int _maxHealth;
    private int _maxMana;
    private int _health;
    private int _mana;
    private float _runSpeed;
    private int _criticalChance;
    private int _criticalMultiplier;
    private int _healthRegeneration;
    private int _manaRegeneration;
    private int _evansion;
    private int _arrowReflectChance;
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
        _evansion = GetComponent<Stats>().evansion;
        _arrowReflectChance = GetComponent<Stats>().arrowReflectChance;
        _maxHealth = _health;
        _maxMana = _mana;
    }

    IEnumerator AttackDetect()
    {        
        if (myTarget.tag != "Untagged" && myTarget != null)
        {
            transform.LookAt(myTarget.transform);
            StopCoroutine(FindTarget());

            if (Vector3.Distance(myTarget.transform.position, transform.position) < 3f)
            {
                myAnim.SetBool("run", false);
                myAnim.SetBool("attack", true);
            } 

            if (Vector3.Distance(myTarget.transform.position, transform.position) >= 1.5f)
            {
                GetComponent<Stats>().runSpeed = _runSpeed;
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
        _evansion = GetComponent<Stats>().evansion;
        _arrowReflectChance = GetComponent<Stats>().arrowReflectChance;
    }

    public void AttackSwordEvent()
    {
        int critChance = Random.Range(0, 100);
        int sendDamage = Random.Range(_attackPower - 10, _attackPower + 10);

        if (critChance <= _criticalChance)
        {
            sendDamage *= _criticalMultiplier;
            myTarget.SendMessage("RecieveCriticalDamage", sendDamage);
        }

        else
        {
            myTarget.SendMessage("RecieveDamage", sendDamage);
        }
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

    public void RecieveDamage(int damage)
    {
        int evansionChance = Random.Range(1, 100);
        int arrowReflectionChance = Random.Range(1, 100);

        if (arrowReflectionChance <= _arrowReflectChance)
        {
            GameObject arr = Instantiate(Arrow, point.transform.position, Quaternion.identity) as GameObject;
            arr.GetComponent<ArrowFly>().target = myTarget;
            arr.GetComponent<ArrowFly>().damage = damage;
            arr.GetComponent<ArrowFly>().critChance = 0;
            arr.GetComponent<ArrowFly>().critMultiplier = 1;
            arr.GetComponent<ArrowFly>().shooter = gameObject;
        }

        if (evansionChance <= _evansion)
        {
            GameObject misstext = Instantiate(displayFloatTextObjectDamage, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            misstext.GetComponent<FloatingText>().floatingTextMessage = "Dodge";
        }

        else
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
    }

    public void RecieveCriticalDamage(int damage)
    {
        int evansionChance = Random.Range(0, 100);

        if (evansionChance <= _evansion)
        {
            GameObject misstext = Instantiate(displayFloatTextObjectDamage, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            misstext.GetComponent<FloatingText>().floatingTextMessage = "Dodge";
        }

        else
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
}
