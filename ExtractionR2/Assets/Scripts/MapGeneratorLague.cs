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

    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;
    public bool createTileMap;

    public TerrainType[] regions;

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

        int tileTexWidth = mapWidth * textureSizeDPI;
        int tileTexHeight = mapHeight * textureSizeDPI;
        int[,][] terrainTextures = new int[tileTexWidth, tileTexHeight][]; // Creates a 2D Array x rows and columns for storing the color information.
 
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

            /*
             * The below commented code correctly stores the Texture2D's color information into an array.
             * _____________________________________________________________________________________________
                Color[] texMap = new Color[textureSizeDPI ^ 2];
                texMap = regions[1].terrainTex.GetPixels();
                Debug.Log(texMap[5]);
             * __________________________________________________________________________________________*/


            // ----NEW LOGIC----
            // For each region, we will add the tile's Texture2D color[] data to the terrainTextures[,] array.  This final array of colors will form our color map.

            for (int i = 0; i < regions.Length; i++) {
                if (regions[i].terrainTex == null) {
                    Debug.Log(regions[i].name + " is missing a terrain texture!");
                } else if (regions[i].terrainTex != null) { // If the region has a texture available, continue.
                    for (int y = 0; y < tileTexHeight; y++) {
                        for (int x = 0; x < tileTexWidth; x++) { // We are now looping through each tile [x,y] of our terrainTextures map.
                            Color[] texMap = new Color[textureSizeDPI ^ 2];
                            texMap = regions[i].terrainTex.GetPixels();

                            terrainTextures[x, y][] = texMap;
                        }
                    }
                }
            }


            //BAD LOGIC BELOW!!!!!!!!!!
            //for (int y = 0; y < mapHeight; y++) { // we don't " * texureSizeDPI " here because we are indexing based upon the noise map.  We add that further down to accommodate the larger size.
            //    for (int x = 0; x < mapWidth; x++) {
            //        // Below means "current pixel within the noise map.  This is relevant because we are using this pixel as our whole tile).  one tile per pixel.
            //        float currentLocation = terrainTextures[x, y];

            //        for (int i = 0; i < regions.Length; i++) {
            //            if (regions[i].terrainTex == null) {
            //                Debug.Log(regions[i].name + " is missing a terrain texture!");
            //            } else if (regions[i].terrainTex != null) {
            //                /* This is where I need to place the texMap array into the correct index of the terrainTextures array.
            //                I need to make sure if it's the first slot of the terrainTextures array it goes to terrainTextures[0], otherwise
            //                it should just be an "array.add".  The problem with this is that it will add arrays into the slots of where
            //                any missing arrays should go.So...it would be better to insert the texMap array into the correct index instead
            //                of just doing an array.add.Not sure yet how to do this.
            //                */

            //                Color[] texMap = new Color[textureSizeDPI^2];
            //                texMap = regions[i].terrainTex.GetPixels();
            //                //Debug.Log(texMap[10]);  // [10] is the color stored in the index 10 of the Color array.  To show all of them you would need to loop through.

            //                //terrainTextures[y * mapWidth + x] = tileTemp[];

            //                for (int j  = 0; j < terrainTextures.Length; j++) {
            //                    currentHeight = 

            //                }
            //                break;
            //            }
            //        }
            //    }
            //}
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

    //private Color[] TileToPixels (Texture2D tileTex){
    //    // **"Color" is a float between 0 and 1.  "Color32" is an integer 0 to 255.
    //    Color[] texMap = tileTex.GetPixels();
    //    int pixelCount = texMap.Length;
    //    float r = 0, g = 0, b = 0, a = 0;
    //    Color c = new Color(0, 0, 0, 0);

    //    for (int z = 0; z <= pixelCount; z++) {
    //        c = texMap[z];
    //        r += c.r;
    //        g += c.g;
    //        b += c.b;
    //        a += c.a;
    //    }        
    //    return texMap;
    //}


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

