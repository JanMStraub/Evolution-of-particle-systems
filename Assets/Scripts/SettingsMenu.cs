using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour {

    public TMP_Text timeText;


    public void SetTimeSpeed(float timeSpeed) {
        SimulationSettings.timeSpeed = timeSpeed;
        timeText.text = timeSpeed.ToString("F1");
    }


    // Not implemented yet
    public void SetCoronaSimulation() {
        SimulationSettings.coronaSimulation = true;
        Debug.Log(SimulationSettings.coronaSimulation);
    }


    public void SetAvoidance(bool agentAvoidance) {
        SimulationSettings.agentAvoidance = agentAvoidance;
        Debug.Log(SimulationSettings.agentAvoidance);
    }
}