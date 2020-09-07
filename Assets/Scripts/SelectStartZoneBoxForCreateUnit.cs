using UnityEngine;

public class SelectStartZoneBoxForCreateUnit : MonoBehaviour
{
    public GameObject unit;
    public Projector proj;
    public GameObject startingPlatform;

    private void OnMouseDown()
    {                        
        if (gameObject.tag == "FirstZonePlatform")
        {
            GameObject[] platform = GameObject.FindGameObjectsWithTag("FirstZonePlatform");

            foreach (var item in platform)
            {
                item.GetComponent<SelectStartZoneBoxForCreateUnit>().proj.enabled = false;
            }

            proj.enabled = true;
        }       
    }    
}
