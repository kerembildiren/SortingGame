using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>
    /// GDD 8.1. A box, bag or crate that tips over and scatters its contents when tapped.
    /// Contents are assigned by the section generator so that shelves always fill exactly.
    /// </summary>
    [CreateAssetMenu(menuName = "Sorting Game/Container", fileName = "Container_")]
    public class ContainerDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayNameKey;
        public int Capacity = 10;

        public GameObject Prefab;
        public PlaceholderVisual Placeholder = new(PlaceholderShape.Cube, new Color(0.72f, 0.55f, 0.36f), new Vector3(0.6f, 0.45f, 0.6f));
    }
}
