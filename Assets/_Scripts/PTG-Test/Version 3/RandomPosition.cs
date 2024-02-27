using UnityEngine;
using System.Collections.Generic;

public static class RandomPosition
{
    private static List<Vector3> availablePositions = new List<Vector3>();

    private static bool isReverse = false;

    private static bool directionBoat;
    private static int nextRow = 1;

    public static List<Props> SpawnObjects(List<Props> prefabs, int spawnRadius, int mask, int row, bool isFull = false)
    {
        isReverse = Random.Range(0, 2) == 0;

        bool first = true;

        float speed = Mathf.Clamp(Random.Range(3, 10) * (row / 100), 2, 10);

        List<Props> spawnedObjects = new List<Props>();

        GenerateAvailablePositions(spawnRadius, mask, row, isFull);

        bool isFirst = true;

        int count = prefabs.Count;

        if (prefabs[0].name == "Train") count = 1;

        for (int prefabIndex = 0; prefabIndex < count; prefabIndex++)
        {
            Props prefab = prefabs[prefabIndex];

            if (prefab.name == "WaterLilyLeaf" && isFirst)
            {
                Vector3 position = new(0, 0, row);
                Props newObject = Object.Instantiate(prefab, position, Quaternion.identity);
                spawnedObjects.Add(newObject);
                isFirst = false;
            }
            else
            {
                if (availablePositions.Count < prefab.Size && prefab.name != "Train") break;


                int randomIndex = Random.Range(0, availablePositions.Count);
                Vector3 randomPosition = availablePositions[randomIndex];

                if (!CheckPositionSize(randomPosition, prefab.Size) && prefab.name != "Train") continue;


                if (prefab.name == "Train") randomPosition = availablePositions[isReverse ? 0 : ^1];

                Props newObject = Object.Instantiate(prefab, randomPosition, Quaternion.identity);

                if (newObject is DynamicProps props)
                {
                    if (prefab.name.Length >= 4 && prefab.name[..4] == "Boat" && first)
                    {
                        if (row == nextRow)
                        {
                            directionBoat = !directionBoat;
                            isReverse = directionBoat;
                        }

                        nextRow = row + 1;
                        first = false;
                    }

                    if (isReverse) props.Reverse();

                    if (prefab.name != "Train") props.Speed = speed;
                }

                if (prefab.name == "Train")
                {
                    float delay = Random.Range(0f, 3f);
                    ((DynamicProps)newObject).StartDelay(delay);

                    spawnedObjects.Add(Object.Instantiate(prefabs[1], new Vector3(-5, 0, row), Quaternion.identity));
                    spawnedObjects.Add(Object.Instantiate(prefabs[1], new Vector3(5, 0, row), Quaternion.identity));
                }

                spawnedObjects.Add(newObject);

                RemoveOccupiedPositions(randomPosition, prefab.Size);

                //if (newObject.GetComponent<AudioSource>() != null)
                //    GameManager.Instance.AudioManager.AddManualAudioSource(newObject.GetComponent<AudioSource>());

                //debug
                //if (prefab.name.Contains("Three3"))
                //{
                //    string x = "";
                //    foreach (Vector3 vector in availablePositions)
                //    {
                //        x += vector.x + ", ";
                //    }
                //    Debug.Log($"{prefabIndex} | {randomPosition.x} | {availablePositions.Count} | ({x})");
                //}
            }
        }

        return spawnedObjects;
    }

    private static void GenerateAvailablePositions(int spawnRadius, int mask, int row, bool isFull)
    {
        availablePositions.Clear();
        spawnRadius /= 2;

        for (int x = -spawnRadius; x <= spawnRadius; x++)
        {
            if (x == 0 && !isFull) continue;
            Vector3 position = new Vector3(x, 0, row);
            availablePositions.Add(position);
        }

        if (mask != 0) availablePositions.RemoveRange(spawnRadius - mask / 2, mask - 1);
    }

    private static void RemoveOccupiedPositions(Vector3 position, int objectSize)
    {
        if (!CheckPositionSize(position, objectSize)) return;

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
        bool check = objectSize > 1;
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
