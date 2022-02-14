using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowNoise : MonoBehaviour
{
    [SerializeField] private Shader snowFallShader;
    private Material snowFallMaterial;

    private MeshRenderer meshRenderer;

    [Range(0.001f, 0.1f)] [SerializeField] private float flakeAmount;

    [Range(0f, 1f)] [SerializeField] private float flakeOpacity;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        snowFallMaterial = new Material(snowFallShader);
    }

    // Update is called once per frame
    void Update()
    {
        snowFallMaterial.SetFloat("_FlakeAmount", flakeAmount);
        snowFallMaterial.SetFloat("_FlakeOpacity", flakeOpacity);
        RenderTexture snow = (RenderTexture) meshRenderer.material.GetTexture("_Splat");
        RenderTexture temp = RenderTexture.GetTemporary(snow.width, snow.height,
            0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(snow, temp, snowFallMaterial);
        Graphics.Blit(temp, snow);
        meshRenderer.material.SetTexture("_Splat", snow);
        RenderTexture.ReleaseTemporary(temp);
    }
}