using UnityEngine;
using System.Collections.Generic;

public static class RandomPosition
{
    private static List<Vector3> availablePositions = new List<Vector3>();

    public static List<Props> SpawnObjects(List<Props> prefabs, int spawnRadius, int row)
    {
        bool isReverse = Random.Range(0, 2) == 0;

        List<Props> spawnedObjects = new List<Props>();

        GenerateAvailablePositions(spawnRadius, row);

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

                if (availablePositions.Count < prefab.Size)
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

    private static void GenerateAvailablePositions(int spawnRadius, int row)
    {
        availablePositions.Clear();
        spawnRadius /= 2;

        for (int x = -spawnRadius; x <= spawnRadius; x++)
        {
            if (x == 0) continue;
            Vector3 position = new Vector3(x, 0, row);
            availablePositions.Add(position);
        }
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

}
