using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour {

    #if UNITY_EDITOR 
    public void Awake() {
        DontDestroyOnLoad(gameObject);
        Debug.Log("DDOL " + gameObject.name);

        if (LoadingSceneIntegration.otherScene > 0) {
            Debug.Log("Returning again to the scene: " + LoadingSceneIntegration.otherScene);
            SceneManager.LoadScene(LoadingSceneIntegration.otherScene);
        }
    }
    #endif
}
