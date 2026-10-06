namespace SortingGame.Data
{
    /// <summary>
    /// GDD 8.1. Common and Rare items go on shelves (Rare glows blue and pays a bit more);
    /// Mascot (Chubby) is the only collectible and goes to the Collection Book.
    /// </summary>
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

    /// <summary>GDD 10.1 tools in the tool bar. The Magnifier was dropped after playtesting (2026-10-06).</summary>
    public enum ToolType
    {
        Hand,
        Broom,
        Magnet
    }
}
