using System.Collections.Generic;

public class Pool
{
    ObjectPooler<Terrain> terrain;
    List<ObjectPooler<Props>> propsList = new();

    public Pool(ObjectPooler<Terrain> pooler)
    {
        terrain = pooler;
    }

    public void SetTerrain(ObjectPooler<Terrain> pooler) => terrain = pooler;
    public void Add(ObjectPooler<Props> pooler) => propsList.Add(pooler);
    public ObjectPooler<Terrain> Terrain { get { return terrain; } }
    public List<ObjectPooler<Props>> PropsList { get { return propsList; } }
    public ObjectPooler<Terrain> ObjectPooler { get; }
    public ObjectPooler<Props> Props(int index) { return propsList[index]; }

    public void DestroyPoolers()
    {
        terrain?.DestroyPool();

        foreach (var propsPooler in propsList)
        {
            propsPooler?.DestroyPool();
        }
    }
}
