using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAgendaController : MonoBehaviour {

    private GameObject _GameManager;

    void Start () {
        _GameManager = GameObject.FindGameObjectWithTag("GameController");

        Debug.Log(_GameManager.LectureList[0]);
    }

}
