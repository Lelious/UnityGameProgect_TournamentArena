using UnityEngine;

public class ArrowFly : MonoBehaviour
{
    public float speed = 20f;
    public GameObject target;
    public GameObject targetHitBox;
    public GameObject shooter;
    public int damage;
    public int critChance;
    public int critMultiplier;

    void Start()
    {
        targetHitBox = target.GetComponent<Stats>().hitBox;
    }

    void Update()
    {
        transform.LookAt(targetHitBox.transform);
        transform.position = Vector3.MoveTowards(transform.position, targetHitBox.transform.position, Time.deltaTime * speed);

        if (shooter.tag != "Untagged" && target.tag != "Untagged")
        {

            if (Vector3.Distance(transform.position, targetHitBox.transform.position) < 0.2)
            {
                int crit = Random.Range(0, 100);

                if (crit <= critChance)
                {
                    damage *= critMultiplier;
                    target.SendMessage("RecieveCriticalDamage", damage);
                }

                else
                {
                    target.SendMessage("RecieveDamage", damage);
                }

                Destroy(gameObject);
            }                
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
