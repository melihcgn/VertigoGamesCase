using UnityEngine;
using UnityEngine.UI;

public class DynamicIconAdjuster : MonoBehaviour
{
    public Image iconImage; // The Image component for the icon

    void Start()
    {
        // Optionally initialize with a default icon
        if (iconImage.sprite != null)
        {
            AdjustIconSize(iconImage.sprite);
        }
    }

    public void SetIcon(Sprite newIcon)
    {
        // Set the icon sprite
        iconImage.sprite = newIcon;

        // Adjust the size dynamically based on the new icon
        AdjustIconSize(newIcon);
    }

    private void AdjustIconSize(Sprite iconSprite)
    {
        // Ensure the Image has an AspectRatioFitter component
        AspectRatioFitter aspectFitter = iconImage.GetComponent<AspectRatioFitter>();
        
        if (aspectFitter == null)
        {
            aspectFitter = iconImage.gameObject.AddComponent<AspectRatioFitter>();
        }

        // Set the aspect mode to Fit In Parent
        aspectFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;

        // Calculate the aspect ratio from the icon's sprite
        if (iconSprite != null)
        {
            float aspectRatio = (float)iconSprite.rect.width / (float)iconSprite.rect.height;
            aspectFitter.aspectRatio = aspectRatio; // Set the dynamic aspect ratio
        }
    }
}
