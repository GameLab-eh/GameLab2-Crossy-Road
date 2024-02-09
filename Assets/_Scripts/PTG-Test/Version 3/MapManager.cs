using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

[RequireComponent(typeof(MapManager))]
public class MapManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField, Min(5)] int rowWidth;
    [SerializeField, Min(5)] int gameRowWidth;

    DefinitionLayout layout;
    DefinitionTheme currentTheme;
    DefinitionTheme nextTheme;

    private int rowCount = 0;
    private int chunkCount = 0;
    private int themeCount = 0;

    int playerPosition = 0;
    int oldPlayerPosition = 5;

    List<GameObject> rowlist = new();

    private void Start()
    {
        playerPosition = (int)player.position.z;

        layout = GameManager.Instance.CurrentLayout;
        currentTheme = layout.Theme[themeCount];
        if (layout.Theme.Count > 1) nextTheme = layout.Theme[themeCount + 1];

        for (int i = 0; i < layout.ChunkLength; i++)
        {
            RowGenerate();
        }
    }

    private void Update()
    {
        playerPosition = (int)player.position.z;
        if (playerPosition > oldPlayerPosition)
        {
            oldPlayerPosition = playerPosition;
            RowGenerate();
            GameObject row = rowlist[0];
            rowlist.RemoveAt(0);
            Destroy(row);
        }
    }

    void RowIDIncrement()
    {
        rowCount++;
        if (rowCount / layout.ChunkLength > chunkCount) chunkCount++;
        if (layout.chunkDelay != 0)
        {
            if (chunkCount / layout.ChunkDelay > themeCount)
            {
                themeCount++;
                currentTheme = nextTheme;
                if (layout.Theme.Count > themeCount) nextTheme = layout.Theme[themeCount + 1];
            }
        }
    }

    void RowGenerate()
    {
        DefinitionTerrain terrain = null;

        float random = Random.Range(0f, 100f);
        float tmp = 0;

        for (int j = 0; j < currentTheme.Terrain.Count; j++)
        {
            tmp += NormalizedFrequency(currentTheme.Terrain, j, chunkCount);
            if (random <= tmp)
            {
                terrain = currentTheme.Terrain[j];
                break;
            }
        }

        GameObject row = Instantiate(terrain.Prefab, transform);
        row.transform.localScale = new(rowWidth, 1f, 1f);
        row.transform.position = new(0, 0, rowCount);
        Template template = row.AddComponent<Template>();
        template.IsFull(terrain.IsFull);
        template.IsMove(terrain.IsMove);
        RowIDIncrement();

        template.PropList(ObjectGenerate(terrain));

        rowlist.Add(row);
    }

    List<Props> ObjectGenerate(DefinitionTerrain terrain)
    {
        List<Props> props = new();

        bool isReverse = Random.Range(0, 2) == 0;

        bool[] isOccupied = new bool[rowWidth];

        Props prop = Object(terrain);

        bool check = terrain.IsMove;

        int maxRange = Mathf.CeilToInt(terrain.Frequency(chunkCount) * ((rowWidth - 2) / prop.Size) * layout.ObstacleDensity);

        int range = Random.Range(((rowWidth /4) / prop.Size), maxRange);

        for (int i = 0; i < range; i++)
        {
            if (!check) prop = Object(terrain);

            props.Add(Instantiate(prop, transform));

            if (check && isReverse) ((DynamicProps)props[(^1)]).Reverse();

            if (!terrain.IsFull) isOccupied[rowWidth / 2] = true;

            int tp;
            do
            {
                tp = Mathf.CeilToInt(Random.Range(0, rowWidth));
            } while (isOccupied[tp] || isOccupied[tp + (prop.Size - 1) >= isOccupied.Length ? (prop.Size - 2) : tp + (prop.Size - 1)]);
            for (int j = tp; j < tp + prop.Size - 1; j++)
            {
                isOccupied[j + (prop.Size - 1) >= isOccupied.Length ? (prop.Size - 2) : j + (prop.Size - 1)] = true;
            }

            props[(^1)].transform.position = new((tp - (rowWidth / 2)), 0, (rowCount - 1));

            if (prop.IsAlone) break;
        }

        return props;
    }

    Props Object(DefinitionTerrain terrain)
    {
        Props prop = null;
        float random = Random.Range(0f, 100f);
        float tmp = 0;

        prop = terrain.Props[0];

        for (int j = 0; j < terrain.Props.Count; j++)
        {
            tmp += NormalizedFrequency(terrain.Props, j, chunkCount);
            if (random <= tmp)
            {
                prop = terrain.Props[j];
                break;
            }
        }
        return prop;
    }

    float NormalizedFrequency(List<DefinitionTerrain> objectList, int index, int chunkCounter)
    {
        float total = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            total += objectList[i].Frequency(chunkCounter);
        }

        return total > 0 ? (objectList[index].Frequency(chunkCounter) * 100) / total : 0;
    }

    float NormalizedFrequency(List<Props> objectList, int index, int chunkCounter)
    {
        float total = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            total += objectList[i].Frequency(chunkCounter);
        }

        return total > 0 ? (objectList[index].Frequency(chunkCounter) * 100) / total : 0;
    }

    public DefinitionTheme CurrentTheme => currentTheme;

    public DefinitionLayout Layout => layout;

    public int RowWidth => rowWidth;
    public int RowHeight => gameRowWidth;
}