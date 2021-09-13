using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour {

	public ComputeShader shader;

	void Start () {
		kernel = Shader.FindKernel("CSMain");
	}

	void Update (){

	}

}
