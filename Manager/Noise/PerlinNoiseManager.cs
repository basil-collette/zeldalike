using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinHelper : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    public int octaves = 4;

    public float persistance = 0.5f;

    public float lacunarity = 2f;

    private FastNoiseLite noise;

    void Start()
    {
        this.noise = new FastNoiseLite();

        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        noise.SetFrequency(scale);
        noise.SetFractalOctaves(octaves);
        noise.SetFractalGain(persistance);

        //noise.SetFractalWeightedStrength();
        //permet de pond�rer chaque octave fractale de mani�re non-lin�aire. Cela signifie que les octaves sup�rieures auront plus d'impact sur la valeur de bruit finale. Cette m�thode prend un param�tre compris entre 0 et 1, o� 0 signifie que toutes les octaves ont le m�me poids et 1 signifie que les octaves sup�rieures ont un poids beaucoup plus important.

        //noise.SetFractalPingPongStrength()
        //permet de cr�er des oscillations dans la valeur de bruit finale en r�p�tant les octaves.Les valeurs de bruit sont r�p�t�es en alternance, ce qui donne une apparence de vague. Cette m�thode prend un param�tre compris entre 0 et 1, o� 0 signifie que les octaves ne sont pas r�p�t�es et 1 signifie que les octaves sont compl�tement r�p�t�es.
        
        noise.SetFractalLacunarity(lacunarity);
    }

    float[,] GetNoiseMap(int width, int height)
    {
        float[,] noiseMap = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float sampleX = (float)x / width * scale + offsetX;
                float sampleY = (float)y / height * scale + offsetY;

                float noiseValue = noise.GetNoise(sampleX, sampleY);
                noiseValue = (noiseValue + 1) / 2f; // Normalize to [0, 1]

                noiseMap[x, y] = noiseValue;
            }
        }

        return noiseMap;
    }

    float GetNoiseAt(int xPos, int yPos)
    {
        return this.noise.GetNoise(xPos, yPos);
    }

}
