using System.Collections.Generic;
using UnityEngine;

public class FindNearestEnemy : MonoBehaviour
{
    List<GameObject> enemy = new List<GameObject>();
    private string myTag;
    GameObject closest;
    void Start()
    {
        myTag = gameObject.tag;

        if (myTag.Equals("FirstPlayer"))
        {
            enemy.AddRange(GameObject.FindGameObjectsWithTag("SecondPlayer"));
        }

        else
        {
            enemy.AddRange(GameObject.FindGameObjectsWithTag("FirstPlayer"));
        }
    }

    public GameObject FindEnemy()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject item in enemy)
        {
            if (item != null)
            {
                Vector3 diff = item.transform.position - position;
                float currDistance = diff.sqrMagnitude;

                if (currDistance < distance)
                {
                    distance = currDistance;
                    if (item.tag != "Untagged")
                    {
                        closest = item;
                    }
                }
            }
        }
        return closest;
    }                         
}
