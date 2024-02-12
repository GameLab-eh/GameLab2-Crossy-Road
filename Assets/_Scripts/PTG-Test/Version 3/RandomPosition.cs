using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

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
                int numberOfObjects = prefabs.Count;

                for (int i = 0; i < numberOfObjects; i++)
                {
                    if (availablePositions.Count < prefab.Size)
                    {
                        Debug.LogError("Not enough available positions.");
                        break;
                    }

                    int randomIndex = Random.Range(0, availablePositions.Count);
                    Vector3 randomPosition = availablePositions[randomIndex];

                    Props newObject = Object.Instantiate(prefab, randomPosition, Quaternion.identity);

                    if (newObject is DynamicProps && isReverse) ((DynamicProps)newObject).Reverse();

                    spawnedObjects.Add(newObject);

                    RemoveOccupiedPositions(randomPosition, prefab.Size);
                }
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

                // Sposta la posizione all'estremità opposta
                position = GetOppositeEndPosition(indexToRemove);
            }
            else
            {
                // Sposta la posizione normalmente
                position = new Vector3(position.x - 1, 0, 0);
            }
        }
    }

    private static Vector3 GetOppositeEndPosition(int currentIndex)
    {
        int lastIndex = availablePositions.Count - 1;

        // Calcola l'indice all'estremità opposta garantendo che rimanga all'interno dei limiti della lista
        int oppositeIndex = lastIndex - currentIndex;
        if (oppositeIndex < 0)
        {
            oppositeIndex += availablePositions.Count;
        }

        return availablePositions[oppositeIndex];
    }


}
