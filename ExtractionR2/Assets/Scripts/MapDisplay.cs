using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public Renderer textureRenderer;

    public void DrawNoiseMap(float[,] noiseMap) {
        int width = noiseMap.GetLength(0); // this grabs the first parameter in the noiseMap array parameters which is mapWidth
        int height = noiseMap.GetLength(1); // this grabs the second parameter in the noiseMap array parameters which is mapHeight

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);  //multiply y * width to get the index of the array.
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();

        // now we have to apply it to the texture renderer.
        // we cannot use "texture.Material" as it is instantiated at runtime and we want to see it in the renderer.

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);

    }
}
