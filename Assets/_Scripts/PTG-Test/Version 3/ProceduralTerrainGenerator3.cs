using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProceduralTerrainGenerator3 : MonoBehaviour
{
    [SerializeField] Transform _player;

    [Min(5)] public int _gameTerrainWidth;

    [SerializeField] DefinitionLayout _definitionLayout;


    #region Gizmo

    public Vector3 dimensioni = new Vector3(1f, 1f, 1f);

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3[] vertici = GetCubeVertices();

        DrawEdges(vertici);
    }

    Vector3[] GetCubeVertices()
    {
        Vector3[] vertici = new Vector3[8];
        Vector3 offset = dimensioni * 0.5f;

        for (int i = 0; i < 8; i++)
        {
            float x = (i & 1) == 0 ? -offset.x : offset.x;
            float y = (i & 2) == 0 ? -offset.y : offset.y;
            float z = (i & 4) == 0 ? -offset.z : offset.z;

            vertici[i] = transform.TransformPoint(new Vector3(x, y, z));
        }

        return vertici;
    }

    void DrawEdges(Vector3[] vertici)
    {
        Gizmos.DrawLine(vertici[0], vertici[1]);
        Gizmos.DrawLine(vertici[2], vertici[3]);
        Gizmos.DrawLine(vertici[4], vertici[5]);
        Gizmos.DrawLine(vertici[6], vertici[7]);
        Gizmos.DrawLine(vertici[0], vertici[4]);
        Gizmos.DrawLine(vertici[1], vertici[5]);
        Gizmos.DrawLine(vertici[2], vertici[6]);
        Gizmos.DrawLine(vertici[3], vertici[7]);
        Gizmos.DrawLine(vertici[0], vertici[2]);
        Gizmos.DrawLine(vertici[1], vertici[3]);
        Gizmos.DrawLine(vertici[4], vertici[6]);
        Gizmos.DrawLine(vertici[5], vertici[7]);
    }

    #endregion
}

[CustomEditor(typeof(ProceduralTerrainGenerator3))]
public class ProceduralTerrainGenerator3Editor : Editor
{
    public override void OnInspectorGUI()
    {
        ProceduralTerrainGenerator3 ptg = (ProceduralTerrainGenerator3)target;

        EditorGUI.BeginChangeCheck();

        int width = EditorGUILayout.IntField("Chunk Width (Odd only)", ptg._gameTerrainWidth);

        if (EditorGUI.EndChangeCheck())
        {
            ptg._gameTerrainWidth = ((width % 2 == 0) ? width + 1 : width);
            EditorUtility.SetDirty(ptg);
        }

        DrawDefaultInspector();
    }
}