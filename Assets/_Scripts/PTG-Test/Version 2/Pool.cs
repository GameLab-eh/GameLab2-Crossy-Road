using System.Collections.Generic;

/// <summary>
/// Class that manages a pool of objects, including terrain and props.
/// </summary>
public class Pool
{
    // Pool for terrains
    ObjectPooler<Terrain> terrain;

    // List of poolers for props objects
    List<ObjectPooler<Props>> propsList = new();

    /// <summary>
    /// Constructor for the Pool class.
    /// </summary>
    /// <param name="pooler">Pooler for terrains</param>
    public Pool(ObjectPooler<Terrain> pooler)
    {
        terrain = pooler;
    }

    /// <summary>
    /// Sets the terrain pooler.
    /// </summary>
    /// <param name="pooler">New pooler for terrains</param>
    public void SetTerrain(ObjectPooler<Terrain> pooler) => terrain = pooler;

    /// <summary>
    /// Adds a props objects pooler to the list.
    /// </summary>
    /// <param name="pooler">New pooler for props objects</param>
    public void Add(ObjectPooler<Props> pooler) => propsList.Add(pooler);

    /// <summary>
    /// Gets the terrain pooler.
    /// </summary>
    public ObjectPooler<Terrain> Terrain { get { return terrain; } }

    /// <summary>
    /// Gets the list of props objects poolers.
    /// </summary>
    public List<ObjectPooler<Props>> PropsList { get { return propsList; } }

    /// <summary>
    /// Gets the props objects pooler based on the index.
    /// </summary>
    /// <param name="index">Index in the props objects list</param>
    /// <returns>Props objects pooler</returns>
    public ObjectPooler<Props> Props(int index) { return propsList[index]; }

    /// <summary>
    /// Destroys all poolers, including those for terrains and props objects.
    /// </summary>
    public void DestroyPoolers()
    {
        terrain?.DestroyPool();

        foreach (var propsPooler in propsList)
        {
            propsPooler?.DestroyPool();
        }
    }
}
