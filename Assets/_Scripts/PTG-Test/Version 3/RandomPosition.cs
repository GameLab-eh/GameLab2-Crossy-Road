using UnityEngine;
using System.Collections.Generic;

public static class RandomPosition
{
    private static List<Vector3> availablePositions = new List<Vector3>();

    private static bool isReverse = false;

    public static List<Props> SpawnObjects(List<Props> prefabs, int spawnRadius, int mask, int row)
    {
        isReverse = Random.Range(0, 2) == 0;

        List<Props> spawnedObjects = new List<Props>();

        GenerateAvailablePositions(spawnRadius, mask, row);

        bool isFirst = true;

        int count = prefabs.Count;

        if (prefabs[0].name == "Train") count = 1;

        for (int prefabIndex = 0; prefabIndex < count; prefabIndex++)
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
                if (availablePositions.Count < prefab.Size && prefab.name != "Train") // first check
                {
                    Debug.LogWarning("Not enough available positions.");
                    break;
                }

                int randomIndex = Random.Range(0, availablePositions.Count);
                Vector3 randomPosition = availablePositions[randomIndex];

                if (!CheckPositionSize(randomPosition, prefab.Size) && prefab.name != "Train")// second check
                {
                    Debug.LogWarning("Not enough available positions.");
                    continue;
                }

                if (prefab.name == "Train") randomPosition = availablePositions[isReverse ? 0 : ^1];

                Props newObject = Object.Instantiate(prefab, randomPosition, Quaternion.identity);

                if (newObject is DynamicProps props && isReverse) props.Reverse();
                if (newObject.name == "Train(Clone)")
                {

                    float delay = Random.Range(0f, 3f);

                    ((DynamicProps)newObject).StartDelay(delay);

                    spawnedObjects.Add(Object.Instantiate(prefabs[1], new Vector3(-5, 0, row), Quaternion.identity));

                    spawnedObjects.Add(Object.Instantiate(prefabs[1], new Vector3(5, 0, row), Quaternion.identity));
                }

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

    private static void RemoveOccupiedPositions(Vector3 position, int objectSize)
    {
        if (!CheckPositionSize(position, objectSize))
        {
            return;
        }

        int index = availablePositions.FindIndex(pos => pos == position);

        index -= isReverse ? 0 : objectSize - 1;

        if (index < 0)
        {
            index = availablePositions.Count + index;

            int tmp = availablePositions.Count - index;
            availablePositions.RemoveRange(index, tmp);

            objectSize -= tmp;
            index = 0;
        }
        if (availablePositions.Count - (index + objectSize) < 0)
        {
            int tmp = availablePositions.Count - index;
            availablePositions.RemoveRange(index, tmp);

            objectSize -= tmp;
            index = 0;
        }

        availablePositions.RemoveRange(index, objectSize);
    }

    private static bool CheckPositionSize(Vector3 position, int objectSize)
    {
        bool check = true;
        float old = position.x;
        for (int i = 0; i < objectSize; i++)
        {
            if (!availablePositions.Contains(position))
            {
                return false;
            }

            int index = availablePositions.FindIndex(pos => pos == position);

            if (index == 0 && check)
            {
                check = !check;
                position = availablePositions[^1];
            }
            else if (index >= availablePositions.Count - 1 && check)
            {
                check = !check;
                position = availablePositions[0];
            }
            else
            {
                check = !check;
                position.x += isReverse ? 1 : -1;
            }

            if ((int)Mathf.Abs(position.x - old) == 1)
            {
                old = position.x;
            }
            else return false;
        }

        return true;
    }
}
