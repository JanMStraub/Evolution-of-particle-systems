using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCSSkript : MonoBehaviour {
    public int SphereAmount = 17;
    public ComputeShader Shader;
    ComputeBuffer resultBuffer;
    int kernel;
    uint threadGroupSize;
    Vector3[] output;

    void Start()
    {
        //program we're executing
        kernel = Shader.FindKernel("Spheres");
        Shader.GetKernelThreadGroupSizes(kernel, out threadGroupSize, out _, out _);

        //buffer on the gpu in the ram
        resultBuffer = new ComputeBuffer(SphereAmount, sizeof(float) * 3);
        output = new Vector3[SphereAmount];
    }

    void Update()
    {
        Shader.SetBuffer(kernel, "Result", resultBuffer);
        int threadGroups = (int) ((SphereAmount + (threadGroupSize - 1)) / threadGroupSize);
        Shader.Dispatch(kernel, threadGroups, 1, 1);
        resultBuffer.GetData(output);
    }

    void OnDestroy()
    {
        resultBuffer.Dispose();
    }
}
