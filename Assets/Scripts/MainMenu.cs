using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu : MonoBehaviour {

    public void StartSimulation() => GameManager.GameManagerInstance.LoadGame();

    public void QuitSimulation() => Application.Quit();
}