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
        //permet de pondérer chaque octave fractale de manière non-linéaire. Cela signifie que les octaves supérieures auront plus d'impact sur la valeur de bruit finale. Cette méthode prend un paramètre compris entre 0 et 1, où 0 signifie que toutes les octaves ont le même poids et 1 signifie que les octaves supérieures ont un poids beaucoup plus important.

        //noise.SetFractalPingPongStrength()
        //permet de créer des oscillations dans la valeur de bruit finale en répétant les octaves.Les valeurs de bruit sont répétées en alternance, ce qui donne une apparence de vague. Cette méthode prend un paramètre compris entre 0 et 1, où 0 signifie que les octaves ne sont pas répétées et 1 signifie que les octaves sont complètement répétées.
        
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
