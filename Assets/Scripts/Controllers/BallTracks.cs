using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTracks : MonoBehaviour
{
    [SerializeField] private Shader drawShader;
    [SerializeField] private GameObject terrain;
    private Transform ball;

    [Header("Brush Properties")] [SerializeField] [Range(1, 500)]
    private float brushSize;

    [SerializeField] [Range(0, 1)] private float brushStrength;


    private Material snowMaterial;
    private Material drawMaterial;
    private RenderTexture splatMap;

    private RaycastHit groundHit;
    private int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        ball = transform;
        layerMask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector("_Color", Color.red);
        snowMaterial = terrain.GetComponent<MeshRenderer>().material;
        snowMaterial.SetTexture("_Splat", splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(ball.position, -Vector3.up,
            out groundHit, 1f, layerMask))
        {
            drawMaterial.SetFloat("_Size", brushSize);
            drawMaterial.SetFloat("_Strength", brushStrength);

            drawMaterial.SetVector("_Coordinate",
                new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
            RenderTexture temp =
                RenderTexture.GetTemporary(splatMap.width, splatMap.height,
                    0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(splatMap, temp);
            Graphics.Blit(temp, splatMap, drawMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }
    }
}