using System.Collections;
using UnityEngine;

public static class Noise {

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {

        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed); // PRNG = Pseudo Random Number Generator
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }



        if (scale <= 0) {   // this will prevent an error if we divide the x and y values by 0 (it won't just be zero now).
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x; // this will give us a non-integer value... a float.
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y; // multiply by frequency so the higher the frequency the height values will change more rapidly.

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // we multiply by 2 and subract 1 so it puts us in the range -1 to 1.
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance; // this is in the range 0-1 so that decreases each octave
                    frequency *= lacunarity; // so the frequency increases each octaves since lacunarity is always greater than 1.
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]); //normalized the noise map to set the value between 0 and 1.
            }
        }
        return noiseMap;
    }

}
