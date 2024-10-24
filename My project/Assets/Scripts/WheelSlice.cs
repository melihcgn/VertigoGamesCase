using UnityEngine;

[System.Serializable]
public class WheelSlice
{
    public string name; // Name of the content
    public Sprite icon; // Sprite for the content
    public string text;
    public WheelSlice(string name, Sprite icon, string text)
    {
        this.name = name;
        this.icon = icon;
        this.text = text;
    }
}
