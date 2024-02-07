using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Template : MonoBehaviour
{
    List<Props> propList = new();

    bool isFull;
    bool isMove;

    public void PropList(List<Props> value) => propList = value;
    public void IsFull(bool value) => isFull = value;
    public void IsMove(bool value) => isMove = value;

    public void OnDestroy()
    {
        foreach(var prop in propList)
        {
            Destroy(prop.gameObject);
        }
        propList.Clear();
    }

#if UNITY_EDITOR
    #region Gizmo

    private void OnDrawGizmos()
    {
        if (isMove) DrawSquares();
        else DrawCubes();
    }

    private void DrawCubes()
    {
        Gizmos.color = Color.cyan;

        int lineSize = (int)transform.localScale.x;

        Vector3 centerPosition = transform.position;

        for (int x = 0; x < lineSize; x++)
        {
            if (x == lineSize / 2) if (!isFull) continue;

            Vector3 cubePosition = centerPosition + new Vector3(x - (lineSize - 1) * 0.5f, 0.5f, 0);
            Gizmos.DrawWireCube(cubePosition, Vector3.one);
        }
    }
    private void DrawSquares()
    {
        Gizmos.color = Color.cyan;

        float squareSize = 1.0f;

        Vector3 leftSquarePosition = transform.position - transform.right * transform.localScale.x * 0.5f;
        Gizmos.DrawWireCube(leftSquarePosition + Vector3.up * 0.5f, new(0, squareSize, squareSize));

        Vector3 rightSquarePosition = transform.position + transform.right * transform.localScale.x * 0.5f;
        Gizmos.DrawWireCube(rightSquarePosition + Vector3.up * 0.5f, new(0, squareSize, squareSize));
    }

    #endregion
#endif
}
