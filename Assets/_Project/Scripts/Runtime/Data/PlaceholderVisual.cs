using System;
using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>
    /// How an item looks while we only have primitives. Replaced by <see cref="ItemDefinition.Prefab"/> once art exists.
    /// </summary>
    [Serializable]
    public struct PlaceholderVisual
    {
        public PlaceholderShape Shape;
        public Color Color;
        [Tooltip("Size in metres (x = width, y = height, z = depth).")]
        public Vector3 Size;

        public PlaceholderVisual(PlaceholderShape shape, Color color, Vector3 size)
        {
            Shape = shape;
            Color = color;
            Size = size;
        }
    }
}
