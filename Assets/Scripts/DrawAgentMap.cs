using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAgentMap : MonoBehaviour {
    [SerializeField] ComputeShader _drawAgentMap;

    void Start() {
        RenderTexture RT = new RenderTexture(2048, 2048, 0);
        RT.enableRandomWrite = true;
        RT.Create();
        _drawAgentMap.SetTexture(0, "surface", RT);  
        _drawAgentMap.Dispatch(0, RT.width / 8, RT.height / 8, 1);
        GetComponent<Renderer>().material.mainTexture = RT;      
    }

    void Update() {
        
    }
}
