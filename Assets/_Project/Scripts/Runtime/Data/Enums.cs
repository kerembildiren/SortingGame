namespace SortingGame.Data
{
    /// <summary>GDD 8.1. Common items go on shelves; Rare and Mascot go to the Collection Book.</summary>
    public enum ItemRarity
    {
        Common,
        Rare,
        Mascot
    }

    /// <summary>Primitive shapes used until real models exist. Each category gets a recognisable shape.</summary>
    public enum PlaceholderShape
    {
        Book,      // flat slab (comics)
        Cube,      // boxy toys, tool boxes
        Sphere,
        Capsule,   // figures, robots
        Cylinder,  // cans, jars
        Rod        // long thin tools
    }
}
