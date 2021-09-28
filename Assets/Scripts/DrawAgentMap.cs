using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class DrawAgentMap : MonoBehaviour {

    private const int _updateKernel = 0;
    private const int _colourKernel = 2;

    [SerializeField] ComputeShader _drawAgentMap;
    [SerializeField] protected RenderTexture _trailMap;

    public const FilterMode defaultFilterMode = FilterMode.Bilinear;
	public const GraphicsFormat defaultGraphicsFormat = GraphicsFormat.R16G16B16A16_SFloat; //GraphicsFormat.R8G8B8A8_UNorm;
    public int width = 1920;
	public int height = 1080;
	public int numAgents = 100;

    void Start() {
        /*
        RenderTexture RT = new RenderTexture(2048, 2048, 0);
        RT.enableRandomWrite = true;
        RT.Create();
        _drawAgentMap.SetBuffer(0, "agents", agentBuffer);
        _drawAgentMap.SetTexture(0, "surface", RT);  
        _drawAgentMap.Dispatch(0, RT.width / 8, RT.height / 8, 1);
        GetComponent<Renderer>().material.mainTexture = RT;      
        */

        // Create render texture
        CreateRenderTexture(ref _trailMap, width, height);

        // Assign texture
        _drawAgentMap.SetTexture(_updateKernel, "surface", _trailMap);
    }

    void Update() {
        
    }

    public static void CreateRenderTexture(ref RenderTexture texture, int width, int height) {
			if (texture == null 
            || !texture.IsCreated() 
            || texture.width != width 
            || texture.height != height 
            || texture.graphicsFormat != defaultGraphicsFormat) {

				if (texture != null) {
					texture.Release();
				}
                
				texture = new RenderTexture(width, height, 0);
				texture.graphicsFormat = defaultGraphicsFormat;
				texture.enableRandomWrite = true;

				texture.autoGenerateMips = false;
				texture.Create();
			}
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = defaultFilterMode;
		}
}
