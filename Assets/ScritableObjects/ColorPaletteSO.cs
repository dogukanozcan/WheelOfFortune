using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette", menuName = "ScriptableObjects/ColorPalette", order = 1)]
public class ColorPaletteSO : ScriptableObject
{
    public List<Color> colors;
    private int index = 0;
    public Color GetNextColor()
    {
        return colors[index++ % colors.Count];
    }
}
