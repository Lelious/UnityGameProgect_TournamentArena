using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameTimer : MonoBehaviour
{
    public GameObject timer;
    public int _timer;
    void Start()
    {
        _timer = 200;
    }
    IEnumerator TimerExpire()
    {
        _timer -= 1;
        GetComponent<Text>().text = _timer.ToString();
        yield return new WaitForSeconds(1.0f);
    }
    private void Update()
    {
        if (_timer > 0)
        {
            StartCoroutine(TimerExpire());
        }

        else
        {
            var firstPlayerGroup = GameObject.FindGameObjectsWithTag("FirstPlayer");
            var secondPlayerGroup = GameObject.FindGameObjectsWithTag("SecondPlayer");

            foreach (var item in firstPlayerGroup)
            {
                item.GetComponent<FindNearestEnemy>().enabled = true;
            }

            foreach (var item in secondPlayerGroup)
            {
                item.GetComponent<FindNearestEnemy>().enabled = true;
            }
            StopCoroutine(TimerExpire());
        }
    }
}
