using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFloatText : MonoBehaviour
{
    public GameObject myBase; 
    public void DestroyText()
    {
        Destroy(myBase);
        Destroy(gameObject);
    }
}
