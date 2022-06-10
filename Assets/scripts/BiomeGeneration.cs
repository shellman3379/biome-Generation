using UnityEngine;
using Unity.Collections;
using Random = UnityEngine.Random;

public class BiomeGeneration : MonoBehaviour
{
    private int height;
    private int width;

    public MeshRenderer rend;

    public uint scale = 100;

    public uint biomes = 5;

    float[] xout;
    float[] yout;

    public void Start()
    {
        xout = new float[biomes + 1];
        yout = new float[biomes + 1];

        width = (int)scale;
        height = (int)scale;

        if (biomes > scale * scale)
            return;

        for (int i = 0; i < biomes; i++)
        {
            for (int j = 0; j < i; j++)
            {
                bool t = true;
                while (t)
                {
                    xout[i] = (int)Random.Range(0, scale);
                    yout[i] = (int)Random.Range(0, scale);
                    if ((xout[i] != xout[j]) && (yout[i] != yout[j]))
                    {
                        t = false;
                    }
                }
            }
        }

        rend.material.mainTexture = generateTexture();
    }
    
    Texture2D generateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixelColorIndex = new Color[biomes];
        Vector2[] pixelLocationIndex = new Vector2[biomes];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int i = 0; i < biomes; i++)
                {
                    if(xout[i] == x)
                    {
                        if(yout[i] == y)
                        {
                            pixelColorIndex[i] = CalculateColor();
                            pixelLocationIndex[i] = new Vector2(x, y);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < biomes; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float[] biomeDistanceIndex = new float[biomes];

                    if(pixelLocationIndex.Equals(new Vector2(x,y)) == false)
                    {
                        for (int j = 0; j < biomes; j++)
                        {
                            biomeDistanceIndex[j] = Vector2.Distance(pixelLocationIndex[j], new Vector2(x, y));
                        }
                        
                        int MinDistance = 1;
                        float buffer = scale * scale;

                        for (int k = 0; k < biomes; k++)
                        {
                            if(biomeDistanceIndex[k] < buffer)
                            {
                                buffer = biomeDistanceIndex[k];
                                MinDistance = k;
                            }
                        }
                        

                        texture.SetPixel(x, y, pixelColorIndex[MinDistance]);
                    }
                    else
                    {
                        texture.SetPixel(x, y, pixelColorIndex[i]);
                    }
                }
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor()
    {
        Color color = new Color(Random.value, Random.value, Random.value);
        return color;
    }
}
