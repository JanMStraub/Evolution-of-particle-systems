using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneIntegration : MonoBehaviour {

 // Runs before a scene gets loaded
    #if UNITY_EDITOR 
    public static int otherScene = -2;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitLoadingScene() {
        Debug.Log("InitLoadingScene()");
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0) return;

        Debug.Log("Loading _preload scene");
        otherScene = sceneIndex;
        // _preload scene is the first in scene build list
        SceneManager.LoadScene(0); 
    }
    #endif

 // You can choose to add any "Service" component to the Main prefab.
 // Examples are: Input, Saving, Sound, Config, Asset Bundles, Advertisements
}