using System.Collections.Generic;
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
        if (this != null)
        {
            foreach (var prop in propList)
            {
                Destroy(prop.gameObject);
            }
            propList.Clear();
        }
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
        int lineSize = GameManager.Instance.MapManager.GameRowWidth;

        int boundsSize = (int)transform.localScale.x - lineSize;

        Vector3 centerPosition = transform.position;

        Gizmos.color = Color.cyan;

        for (int x = 0; x < lineSize; x++)
        {
            if (x == lineSize / 2) if (!isFull) continue;

            Vector3 cubePosition = centerPosition + new Vector3(x - (lineSize - 1) * 0.5f, 0.5f, 0);
            Gizmos.DrawWireCube(cubePosition, Vector3.one);
        }

        Gizmos.color = Color.magenta;

        for (int x = 0; x < boundsSize; x++)
        {
            float offset = (x < boundsSize / 2) ? -0.5f : 0.5f;
            Vector3 cubePosition = centerPosition + new Vector3(x + offset * lineSize - (boundsSize - 1) * 0.5f, 0.5f, 0);
            Gizmos.DrawWireCube(cubePosition, Vector3.one);
        }
    }
    private void DrawSquares()
    {
        Gizmos.color = Color.cyan;

        float squareSize = 1.0f;

        Vector3 leftSquarePosition = transform.position - 0.5f * transform.localScale.x * transform.right;
        Gizmos.DrawWireCube(leftSquarePosition + Vector3.up * 0.5f, new(0, squareSize, squareSize));

        Vector3 rightSquarePosition = transform.position + 0.5f * transform.localScale.x * transform.right;
        Gizmos.DrawWireCube(rightSquarePosition + Vector3.up * 0.5f, new(0, squareSize, squareSize));

        Gizmos.color = Color.magenta;

        int lineSize = GameManager.Instance.MapManager.GameRowWidth;

        float halfLineSize = lineSize / 2;
        float zPosition = transform.position.z;

        Vector3 startPoint1 = new Vector3(-halfLineSize - 0.5f, 0f, zPosition - 0.5f);
        Vector3 endPoint1 = new Vector3(-halfLineSize - 0.5f, 0f, zPosition + 0.5f);
        Gizmos.DrawLine(startPoint1, endPoint1);

        Vector3 startPoint2 = new Vector3(halfLineSize + 0.5f, 0f, zPosition - 0.5f);
        Vector3 endPoint2 = new Vector3(halfLineSize + 0.5f, 0f, zPosition + 0.5f);
        Gizmos.DrawLine(startPoint2, endPoint2);
    }

    #endregion
#endif
}
