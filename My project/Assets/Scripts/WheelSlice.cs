using UnityEngine;

[System.Serializable]
public class WheelSlice
{
    public string name; // Name of the content
    public Sprite icon; // Sprite for the content

    public WheelSlice(string name, Sprite icon)
    {
        this.name = name;
        this.icon = icon;
    }
}
