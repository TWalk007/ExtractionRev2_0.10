using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGeneratorLague))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        MapGeneratorLague mapGen = (MapGeneratorLague) target; //the object the inspector is editing so let's cast it to the map generator script (class).

        if (DrawDefaultInspector()) {
            if (mapGen.autoUpdate) {
                mapGen.GenerateMap();
            }
        }
        if(GUILayout.Button("Generate")) {
            mapGen.GenerateMap();
        }


        /*  END LAGUE TUTORIAL________________________________________________________________________________________
         *  
         *  The following button calls the Texture Generator to evaluate the Texture2D color map info to create a tile map from it
         */
        //if (GUILayout.Button("Generate Tiles")) {
        //    TextureGenerator.TileMapFromTexture();
        //    Debug.Log("Generating Tile Map");
        //}
    }
}
