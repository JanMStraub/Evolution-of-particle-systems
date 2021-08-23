using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu : MonoBehaviour {

    public void StartSimulation () {

        GameManager.Instance.LoadGame();
    }

    public void QuitSimulation () {
        
        Application.Quit();
    }
}
