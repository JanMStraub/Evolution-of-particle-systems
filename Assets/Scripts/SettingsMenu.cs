using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour {

    public TMP_Text timeText;


    public void SetTimeSpeed(float timeSpeed) {
        SimulationSettings.timeSpeed = timeSpeed;
        timeText.text = timeSpeed.ToString("F1");
    }


    public void SetCoronaSimulation() {
        SimulationSettings.coronaSimulation = true;
        Debug.Log(SimulationSettings.coronaSimulation);
    }
}