using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexController : MonoBehaviour {

    [SerializeField] List<GameObject> _doors = new List<GameObject>();

    /*

    234/235/236 zusammen als 234
    306/308/324/325/327/328/329/345/346/348 zusammen als 306
    304 Mensa
    700/720 zusammen als 700


    Mensagebäude noch einfügen und Mittagspause reinbringen !ACHTUNG: Teilweise Vorlesungen den ganzen Tag -> Ausnahmen?

    */

    void Start () {

        GameObject doors_parent = GameObject.FindGameObjectWithTag("ComplexController");

        foreach (Transform door in doors_parent.transform) {
            _doors.Add(door.gameObject);
        }

        Debug.Log(_doors[1].tag);
    }
}
