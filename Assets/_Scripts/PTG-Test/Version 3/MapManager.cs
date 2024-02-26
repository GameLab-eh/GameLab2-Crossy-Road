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

        for (int i = 0; i < 10; i++)
        {
            GameObject row = Instantiate(currentTheme.SafeArea.Prefab, transform);
            row.transform.localScale = new(rowWidth, 1f, 1f);
            row.transform.position = new(0, 0, i * -1);

            List<Props> props = new();

            int numberOfObjjects = 0;
            int mask = 0;

            switch (i)
            {
                case 0:
                    mask = gameRowWidth;
                    break;
                case 1:
                    mask = gameRowWidth - 2;
                    break;
                case 2:
                    mask = gameRowWidth - 4;
                    break;
                case 3:
                    mask = gameRowWidth - 6;
                    break;
                case 4:
                    mask = gameRowWidth - 8;
                    break;
                default:
                    numberOfObjjects = rowWidth;
                    break;
            }
            props = ObjectGenerate(currentTheme.SafeArea, rowWidth, numberOfObjjects, mask, (-1 * i) + 1, true);

            Template template = row.AddComponent<Template>();
            if (i != 0) template.IsFull(currentTheme.SafeArea.IsFull);


            template.PropList(props);

            props.ForEach(son => son.transform.SetParent(template.transform));

            rowlist.Add(row);
        }
        RowIDIncrement();

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

        if (rowCount % layout.ChunkLength == 0)
            chunkCount++;

        if (layout.chunkDelay == 0 || chunkCount == 0 || themeCount + 3 > layout.Theme.Count)
            return;

        themeCount = Mathf.FloorToInt(Mathf.Min(chunkCount / layout.ChunkDelay, layout.Theme.Count - 1));

        currentTheme = nextTheme;
        nextTheme = layout.Theme[themeCount];
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

        List<Props> props = new();

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
        int radius = rowWidth;
        int numberOfObjects = 0;
        int mask = 0;

        switch (terrain.name.ToLower())
        {
            case "lake":
                radius = gameRowWidth;
                break;
            case "grass":
                radius = gameRowWidth;
                mask = gameRowWidth;
                break;
            case "railroad":
                numberOfObjects = 1;
                mask = rowWidth - 6;
                break;
            default:
                break;
        }

        if (numberOfObjects == 0)
        {
            int min = Mathf.CeilToInt((radius - 2) * terrain.Density(chunkCount));
            numberOfObjects = Mathf.Clamp((int)(min * layout.ObstacleDensity), 0, radius - 2);
        }

        return ObjectGenerate(terrain, radius, numberOfObjects, mask);
    }

    List<Props> ObjectGenerate(DefinitionTerrain terrain, int radius, int numberOfObjects = 0, int mask = 0, int z = 0, bool isFull = false)
    {
        List<Props> list = new();

        list.AddRange(ListProps(numberOfObjects, terrain));

        List<Props> objects = numberOfObjects > 1 ? RandomPosition.SpawnObjects(list, radius, 0, rowCount - 1 + z, isFull) : new();

        if (mask != 0)
        {
            if (numberOfObjects != 1)
            {
                list.AddRange(ListProps(rowWidth - gameRowWidth, terrain));

                list.RemoveAll(obj => obj.name == "Coin");
            }
            objects.AddRange(RandomPosition.SpawnObjects(list, rowWidth, mask, rowCount - 1 + z, isFull));
        }

        list.Clear();

        return objects;
    }

    List<Props> ListProps(int numberOfObjects, DefinitionTerrain terrain)
    {
        List<Props> list = new();

        if (terrain.name == "Railroad")
        {
            list.Add(terrain.Props[0]);
            list.Add(terrain.Props[1]);

            return list;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            Props prop = Object(terrain);
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