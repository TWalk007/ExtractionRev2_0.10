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

            Color[] texMap = new Color[textureSizeDPI ^ 2];
            texMap = regions[1].terrainTex.GetPixels();
            Debug.Log(texMap[5]);

            //  The values have correctly been store in the color array!!!  Woot!!
            //  Now I just need apply this color[] data (texMap) to the correct spot in the colorMap above.
            //  Or more precisely create my own Color Map so I don't overwrite the noise color map as it's still useful.

            for (int y = 0; y < mapHeight; y++) { // we don't " * texureSizeDPI " here because we are indexing based upon the noise map.  We add that further down to accommodate the larger size.
                for (int x = 0; x < mapWidth; x++) {
                    for (int i = 0; i < regions.Length; i++) {
                        // Below means "current pixel within the noise map.  This is relevant because we are using this pixel as our whole tile).  one tile per pixel.
                        float currentHeight = noiseMap[x, y];

                        // Then for each tile, we will add the terrain's texture data to the new terrainTextures array (our Texture2D).
                        if (regions[i].terrainTex == null) {
                            Debug.Log(regions[i].name + " is missing a terrain texture!");
                        } else if (regions[i].terrainTex != null) {

                        /* This is where I need to place the texMap array into the correct index of the terrainTextures array.
                        I need to make sure if it's the first slot of the terrainTextures array it goes to terrainTextures[0], otherwise
                        it should just be an "array.add".  The problem with this is that it will add arrays into the slots of where
                        any missing arrays should go.So...it would be better to insert the texMap array into the correct index instead
                        of just doing an array.add.Not sure yet how to do this.
                        */
                            //terrainTextures[y * textureSizeDPI + x] = texMap; ;
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

