using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu : MonoBehaviour {

    private void Awake() {
        SimulationSettings.timeSpeed = 0.8f;
    }


    public void StartSimulation() => GameManager.GameManagerInstance.LoadGame();


    public void QuitSimulation() => Application.Quit();
}