using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    [Header("Brush Properties")] [SerializeField] [Range(1, 500)]
    private float brushSize;

    [SerializeField] [Range(0, 1)] private float brushStrength;

    [Space(10)] private Camera cam;
    public Shader drawShader;
    private Material snowMaterial;
    private Material drawMaterial;
    private RenderTexture splatMap;

    private RaycastHit hit;

    // Start is called before the first frame update
    void Awake()
    {
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector("_Color", Color.red);
        cam = Camera.main;
        snowMaterial = GetComponent<MeshRenderer>().material;
        splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat", splatMap);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            {
                drawMaterial.SetFloat("_Size", brushSize);
                drawMaterial.SetFloat("_Strength", brushStrength);

                drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                RenderTexture temp =
                    RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), splatMap, ScaleMode.ScaleToFit, false, 1);
    }
}