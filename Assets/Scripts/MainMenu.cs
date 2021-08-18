using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu : MonoBehaviour {

    public void StartSimulation () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitSimulation () {
        Application.Quit();
    }
}
