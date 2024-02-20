using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

/*
public class GetShaderResult : MonoBehaviour
{
    [SerializeField] Material materialWithShaderGraph;
    [SerializeField] int _Seed = 1111;
    [SerializeField] int pixWidth = 50;
    [SerializeField] int pixHeight = 50;
    [SerializeField] float _Water = 0.1f;
    [SerializeField] float _Stage = 0.3f;

    private Texture2D noiseTex;
    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        Generate();
    }

    public void Generate()
    {
        noiseTex = new Texture2D(pixWidth, pixHeight);
        rend.material.mainTexture = noiseTex;

        materialWithShaderGraph.SetFloat("_Seed", _Seed);

        RenderTexture renderTexture = RenderTexture.GetTemporary(pixWidth, pixHeight, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(null, renderTexture, materialWithShaderGraph);

        Texture2D texture2D = new Texture2D(pixWidth, pixHeight, TextureFormat.ARGB32, false);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, pixWidth, pixHeight), 0, 0);
        texture2D.Apply();

        RenderTexture.ReleaseTemporary(renderTexture);

        Color[] pixels = texture2D.GetPixels();

        int mapSize = (int)Math.Sqrt(pixels.Length);
        float[][] map = new float[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            map[i] = pixels.Skip(0).Take(mapSize).Select(x => x.r).ToArray();
        }

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = GetColor(pixels[i].r);
        }

        noiseTex.SetPixels(pixels);
        noiseTex.Apply();
    }

    public void Reseed()
    {
        _Seed = Random.Range(-99999, 99999);
        Generate();
    }

    Color GetColor(float height)
    {
        if (height < _Water) return new Color(0, 0, 1);
        if (height < _Stage) return new Color(0, 1, 0);
        return new Color(0.25f, 0.2f, 0.1f);

        //return new Color(0, 0, 0);
    }

}

[CustomEditor(typeof(GetShaderResult))]
public class VotreScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GetShaderResult script = (GetShaderResult)target;

        if (DrawDefaultInspector())
        {
            script.Generate();
        }

        if (GUILayout.Button("Reseed"))
        {
            script.Reseed();
        }

        if (GUILayout.Button("Generate"))
        {
            script.Generate();
        }
    }
}
*/