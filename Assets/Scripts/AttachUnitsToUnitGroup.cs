using System.Collections.Generic;
using UnityEngine;

public class AttachUnitsToUnitGroup : MonoBehaviour
{
    List<GameObject> unitsList = new List<GameObject>();

    public void AttachUnitsToGroup()
    {
        unitsList.AddRange(GameObject.FindGameObjectsWithTag("FirstPlayer"));
        unitsList.AddRange(GameObject.FindGameObjectsWithTag("SecondPlayer"));
    }
}
