using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentLoader : MonoBehaviour {

    private static AgentLoader _AgentInstance;


    public static AgentLoader AgentInstance {
        get {return _AgentInstance;}
    }


    void Awake () {
        if (_AgentInstance != null && _AgentInstance != this) {
            Destroy(this.gameObject);
            return;
        }

        _AgentInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
