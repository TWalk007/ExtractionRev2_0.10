﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator {

    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height) {
        Texture2D texture = new Texture2D(width , height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap) {
        int width = heightMap.GetLength(0); // this grabs the first parameter in the noiseMap array parameters which is mapWidth
        int height = heightMap.GetLength(1); // this grabs the second parameter in the noiseMap array parameters which is mapHeight

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);  //multiply y * width to get the index of the array.
            }
        }
        return TextureFromColorMap(colorMap, width, height);
    }

    /*  END LAGUE TUTORIAL______________________________________________________________________________________________________________
    *
    *   The following script will evaluate the Texture2D information that was generated above and apply the correct tiles for each quad.
    */
    public static Texture2D TileMapFromTexture(Color [,] textureColorMap, Texture2D tile) {

        Debug.Log("Creating tile map from texture information...");

        





        return null;
    }

}
