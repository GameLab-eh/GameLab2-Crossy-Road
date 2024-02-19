using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        StartInizializer();
    }

    private void StartInizializer()
    {
        playerPosition = (int)player.position.z;

        layout = GameManager.Instance.CurrentLayout;
        currentTheme = layout.Theme[themeCount];
        if (layout.Theme.Count > 1) nextTheme = layout.Theme[themeCount + 1];

        GameObject row = Instantiate(currentTheme.SafeArea, transform);
        row.transform.localScale = new(rowWidth, 1f, 1f);
        row.transform.position = new(0, 0, rowCount);
        RowIDIncrement();
        rowlist.Add(row);

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

        if (terrain.Prefab.name == "River" || terrain.Prefab.name == "Lake")
        {
            GameObject waterfalls = Instantiate(currentTheme.Waterfall, transform);
            waterfalls.transform.position = new(-(int)(gameRowWidth / 2), 0, rowCount - 1);
            waterfalls.transform.parent = row.transform;
            waterfalls.transform.eulerAngles = new(0, 180, 0);

            waterfalls = Instantiate(currentTheme.Waterfall, transform);
            waterfalls.transform.position = new((int)(gameRowWidth / 2), 0, rowCount - 1);
            waterfalls.transform.parent = row.transform;
        }

        List<Props> props = new List<Props>();

        props = ObjectGenerate(terrain);

        template.PropList(props);

        props.ForEach(son => son.transform.SetParent(template.transform));

        rowlist.Add(row);
    }

    Props Object(DefinitionTerrain terrain)
    {
        Props prop = terrain.Props[0];
        float random = Random.Range(0f, 100f);
        float tmp = 0;

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

    List<Props> ObjectGenerate(DefinitionTerrain terrain)
    {
        List<Props> list = new();

        int radius = rowWidth;
        int numberOfObjects = 0;
        int mask = 0;

        bool needsToBeChecked = false;

        switch (terrain.name.ToLower())
        {
            case "river":
                needsToBeChecked = true;
                break;
            case "lake":
                radius = gameRowWidth;
                break;
            case "road":
                needsToBeChecked = true;
                break;
            case "grass":
                radius = gameRowWidth;
                mask = gameRowWidth;
                break;
            default:
                numberOfObjects = 1;
                mask = rowWidth - 6;
                break;
        }

        if (numberOfObjects != 1)
        {
            int min = Mathf.CeilToInt((radius - 2) * terrain.Density(chunkCount));
            numberOfObjects = Mathf.Clamp((int)(min * layout.ObstacleDensity), 0, radius - 2);
        }

        list.AddRange(ListProps(numberOfObjects, terrain, needsToBeChecked));

        List<Props> objects = numberOfObjects > 1 ? RandomPosition.SpawnObjects(list, radius, 0, rowCount - 1) : new();

        if (mask != 0)
        {
            if (numberOfObjects != 1) list.AddRange(ListProps(rowWidth - gameRowWidth, terrain, needsToBeChecked));
            objects.AddRange(RandomPosition.SpawnObjects(list, rowWidth, mask, rowCount - 1));
        }

        list.Clear();

        return objects;
    }

    List<Props> ListProps(int numberOfObjects, DefinitionTerrain terrain, bool needsToBeChecked)
    {
        List<Props> list = new List<Props>();

        if (terrain.name == "Railroad")
        {
            list.Add(terrain.Props[0]);
            list.Add(terrain.Props[1]);

            return list;
        }

        float speed = 0;
        for (int i = 0; i < numberOfObjects; i++)
        {
            Props prop = Object(terrain);
            if (needsToBeChecked)
            {
                if (i == 0) speed = ((DynamicProps)prop).Speed;
                while (((DynamicProps)prop).Speed != speed)
                {
                    prop = Object(terrain);
                }
            }
            list.Add(prop);
        }

        return list;
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
    public int GameRowWidth => gameRowWidth;
}