using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainConverter))]
public class TerrainConverterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TerrainConverter terrainConverter = (TerrainConverter)target;
        if (GUILayout.Button("Convert Terrain"))
        {
            terrainConverter.ConvertTerrain();
        }
    }
}
