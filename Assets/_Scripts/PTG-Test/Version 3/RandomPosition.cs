using UnityEngine;
using System.Collections.Generic;

public static class RandomPosition
{
    private static List<Vector3> availablePositions = new List<Vector3>();

    public static List<Props> SpawnObjects(List<Props> prefabs, int spawnRadius, int mask, int row)
    {
        bool isReverse = Random.Range(0, 2) == 0;

        List<Props> spawnedObjects = new List<Props>();

        GenerateAvailablePositions(spawnRadius, mask, row);

        bool isFirst = true;

        for (int prefabIndex = 0; prefabIndex < prefabs.Count; prefabIndex++)
        {
            Props prefab = prefabs[prefabIndex];

            if (prefab.name == "WaterLilyLeaf" && isFirst)
            {
                Vector3 position = new Vector3(0, 0, row);
                Props newObject = Object.Instantiate(prefab, position, Quaternion.identity);
                spawnedObjects.Add(newObject);
                isFirst = false;
            }
            else
            {

                if (availablePositions.Count < prefab.Size) // check
                {
                    Debug.LogWarning("Not enough available positions.");
                    break;
                }

                int randomIndex = Random.Range(0, availablePositions.Count);
                Vector3 randomPosition = availablePositions[randomIndex];

                Props newObject = Object.Instantiate(prefab, randomPosition, Quaternion.identity);

                if (newObject is DynamicProps && isReverse) ((DynamicProps)newObject).Reverse();
                if (newObject.name == "Train(Clone)") ((DynamicProps)newObject).StartDelay(Random.Range(0f, 3f));

                spawnedObjects.Add(newObject);

                RemoveOccupiedPositions(randomPosition, prefab.Size);

            }
        }

        return spawnedObjects;
    }

    private static void GenerateAvailablePositions(int spawnRadius, int mask, int row)
    {
        availablePositions.Clear();
        spawnRadius /= 2;

        for (int x = -spawnRadius; x <= spawnRadius; x++)
        {
            if (x == 0) continue;
            Vector3 position = new Vector3(x, 0, row);
            availablePositions.Add(position);
        }

        if (mask != 0) availablePositions.RemoveRange(spawnRadius - mask / 2, mask - 1);
    }

    private static void RemoveOccupiedPositions(Vector3 centerPosition, int objectSize)
    {
        Vector3 position = centerPosition;

        for (int i = 0; i < objectSize; i++)
        {
            if (availablePositions.Contains(position))
            {
                int indexToRemove = availablePositions.FindIndex(pos => pos == position);
                availablePositions.RemoveAt(indexToRemove);

                position = GetOppositeEndPosition(indexToRemove);
            }
            else position = new Vector3(position.x - 1, 0, 0);
        }
    }

    private static Vector3 GetOppositeEndPosition(int currentIndex) // needs to be revised
    {
        if (availablePositions.Count == 0) return Vector3.zero;

        int lastIndex = availablePositions.Count - 1;

        int oppositeIndex = (lastIndex - currentIndex + availablePositions.Count) % availablePositions.Count;

        return availablePositions[oppositeIndex];
    }

    //private static void RemoveOccupiedPositions(Vector3 centerPosition, int objectSize)
    //{
    //    Vector3 position = centerPosition;

    //    if (availablePositions.Contains(position) && availablePositions.Contains(new Vector3(position.x + objectSize - 1, 0f, position.z)))
    //    {
    //        int indexLower = availablePositions.FindIndex(pos => pos == position);

    //        availablePositions.RemoveRange(indexLower, objectSize);
    //    }
    //    else
    //    {
    //        int[] array = new int[objectSize];
    //        int tmp = (int)position.x;

    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            Vector3 vectortmp = new(tmp + i, 0, position.z);
    //            if (availablePositions.Contains(vectortmp))
    //            {
    //                array[i] = availablePositions.FindIndex(pos => pos == vectortmp);
    //            }
    //            else
    //            {
    //                tmp -= availablePositions.Count;
    //                i--;
    //            }
    //        }

    //        System.Array.Sort(array);

    //        Debug.Log("--------------------------");

    //        for (int i=0; i<array.Length; i++)
    //        {
    //            Debug.Log(array[i]);
    //            //availablePositions.RemoveAt(array[i]);
    //        }
    //    }
    //}

    /* for external density
    for (int x = 0; x < boundsSize; x++)
        {
            float offset = (x < boundsSize / 2) ? -0.5f : 0.5f;
            Vector3 cubePosition = centerPosition + new Vector3(x + offset * lineSize - (boundsSize - 1) * 0.5f, 0.5f, 0);
            Gizmos.DrawWireCube(cubePosition, Vector3.one);
        }
    */

}
