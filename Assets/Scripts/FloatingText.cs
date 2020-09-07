using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public string floatingTextMessage;
    public TextMesh myTextMesh;

    private void Start()
    {
        myTextMesh.text = floatingTextMessage;
        myTextMesh.transform.eulerAngles = Camera.main.transform.eulerAngles;
    }
}
