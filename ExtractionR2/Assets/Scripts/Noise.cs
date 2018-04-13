using System.Collections;
using UnityEngine;

public static class Noise {

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale) {

        float[,] noiseMap = new float[mapWidth, mapHeight];

        if (scale <= 0) {   // this will prevent an error if we divide the x and y values by 0 (it won't just be zero now).
            scale = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                float sampleX = x / scale; // this will give us a non-integer value... a float.
                float sampleY = y / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinValue;
            }
        }
        return noiseMap;
    }


}
