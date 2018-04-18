using System.Collections;
using UnityEngine;

public class MapGeneratorLague : MonoBehaviour {

    public enum DrawMode { NoiseMap, ColorMap }
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public int textureSizeDPI = 64;
    public float noiseScale;

    [Range(1, 25)]
    public int octaves;

    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;
    public bool createTileMap;  // If this is checked it places the Texture2D that is in the "Terrain Tex" slot of each region.

    public TerrainType[] regions;
    public Color[] SingleTileArray;


    [System.Serializable]
    public struct TerrainType {
        public string name;
        public float height;
        public Color color;
        public Texture2D terrainTex;
    }

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];
        Texture2D[] terrainTextures = new Texture2D[mapWidth * mapHeight * textureSizeDPI];

        if (!createTileMap) {
            for (int y = 0; y < mapHeight; y++) {
                for (int x = 0; x < mapWidth; x++) {
                    float currentHeight = noiseMap[x, y];

                    for (int i = 0; i < regions.Length; i++) {
                        if (currentHeight <= regions[i].height) {
                            colorMap[y * mapWidth + x] = regions[i].color;
                            break;
                        }
                    }
                }
            }
        } else if (createTileMap) {
            // this is where we'll place the Texture2D that is in the "Terrain Tex" slot of the inspector for each region.
            // First, we need to store each of the Texture2D pixel info into an array.

            //TESTING ONLY///////////////////////////////////////////
            TileToPixels(regions[1].terrainTex);


            for (int y = 0; y < mapHeight; y++) {
                for (int x = 0; x < mapWidth; x++) {
                    float currentHeight = noiseMap[x, y];

                    for (int i = 0; i < regions.Length; i++) {
                        if (currentHeight <= regions[i].height) {
                            colorMap[y * mapWidth + x] = regions[i].color;
                            break;
                        }
                    }
                }
            }
        }
        

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        } else if (drawMode == DrawMode.ColorMap) {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
        
    }

    // TileToPixels () is a method that will use a specified Texture2D from the regions [] and use it to populate a color array.
    // We will use this data later to populate a single tile on the larger texture map (our tile map).
    private Color[] TileToPixels (Texture2D tileTex) {
        Color[] singleTilePixels = new Color[textureSizeDPI^2];

        for (int y = 0; y < textureSizeDPI; y++) {
            for (int x = 0; x < textureSizeDPI; x++) {

                float currentHeight = tileTex.GetPixel(x,y).grayscale;

                singleTilePixels[y * textureSizeDPI + x] = tileTex.GetPixel(x,y);

                Debug.Log(singleTilePixels);
                break;
                
            }
        }


        return singleTilePixels;
    }



    private void OnValidate() { //called automatically whenever one of the scripts variables change in the inspector.
        if (mapWidth < 1) {
            mapWidth = 1;
        }
        if (mapHeight < 1) {
            mapHeight = 1;
        }
        if (lacunarity < 1) {
            lacunarity = 1;
        }
    }
}

