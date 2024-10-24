using UnityEngine;
using TMPro; // Make sure to include this for TextMeshPro
using System.Collections;

public class LevelBar : MonoBehaviour
{
    public GameObject levelPrefab; // Prefab for level display (could be a button or text)
    public Transform levelContainer; // Parent object to hold level display items
    public int totalLevels = 60; // Total levels available
    private int currentLevel = 1; // Starting level (you can set this dynamically)
    private bool isAnimating = false; // To ensure only one coroutine runs at a time
    public Color multipleOf5Color = Color.green; // Color for multiples of 5
    public Color multipleOf30Color = Color.yellow;
    public GameObject uiGolden;
    public GameObject uiSilver;
    public GameObject uiBronze;
    private void Start()
    {
        InitializeLevelDisplay(); // Initialize the display at start
    }

    public void LevelUp()
    {
        if (currentLevel < totalLevels && !isAnimating)
        {
            currentLevel++;
            StartCoroutine(UpdateLevelDisplayCoroutine());

            // Determine spin type based on the current level
            if (currentLevel % 30 == 0)
            {
                TriggerGoldenSpin();
            }
            else if (currentLevel % 5 == 0)
            {
                TriggerSilverSpin();
            }
            else
            {
                TriggerBronzeSpin();
            }
        }
    }

    private void TriggerGoldenSpin()
    {
        Debug.Log("Golden Spin Triggered!");
        // Show a golden spin modal or animation
        uiBronze.SetActive(false);
        uiGolden.SetActive(true);
    }

    private void TriggerSilverSpin()
    {
        Debug.Log("Silver Spin Triggered!");
        // Show a silver spin modal or animation
        uiBronze.SetActive(false);
        uiSilver.SetActive(true);
    }

    private void TriggerBronzeSpin()
    {
        Debug.Log("Bronze Spin Triggered!");

        // Check if uiBronze is inactive, then activate it
        if (!uiBronze.activeSelf)
        {
            // Disable other UI elements
            uiGolden.SetActive(false);
            uiSilver.SetActive(false);

            // Activate the Bronze UI
            uiBronze.SetActive(true);
        }

        // Logic for Bronze Spin (basic rewards)
    }
    public void RestartLevelBar()
    {
        currentLevel = 1; // Reset the current level
        InitializeLevelDisplay(); // Reinitialize the level display
    }

    public void RestartSpinWheel()
    {
        TriggerBronzeSpin(); // Reinitialize the level display
    }

    private IEnumerator UpdateLevelDisplayCoroutine()
    {
        isAnimating = true; // Lock animation

        // Calculate new target positions for all level items
        Vector2[] targetPositions = new Vector2[totalLevels];
        for (int i = 0; i < totalLevels; i++)
        {
            GameObject levelItem = levelContainer.GetChild(i).gameObject; // Get the corresponding level item
            RectTransform rectTransform = levelItem.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // Calculate the new target position (moving upwards)
                targetPositions[i] = new Vector2(0, rectTransform.anchoredPosition.y + rectTransform.rect.height); // Shift upwards
            }
        }

        // Animate all level items to their new positions
        float duration = 0.5f; // Duration of the animation
        float elapsedTime = 0f;

        // Perform the animation for all items simultaneously
        while (elapsedTime < duration)
        {
            for (int i = 0; i < totalLevels; i++)
            {
                GameObject levelItem = levelContainer.GetChild(i).gameObject; // Get the corresponding level item
                RectTransform rectTransform = levelItem.GetComponent<RectTransform>();

                if (rectTransform != null)
                {
                    // Calculate the new position using Lerp
                    rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPositions[i], elapsedTime / duration);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure final positions are set
        for (int i = 0; i < totalLevels; i++)
        {
            GameObject levelItem = levelContainer.GetChild(i).gameObject; // Get the corresponding level item
            RectTransform rectTransform = levelItem.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = targetPositions[i]; // Set to final position
            }
        }

        isAnimating = false; // Unlock animation for the next level-up
    }
    private void InitializeLevelDisplay()
    {
        // Clear previous level items before initializing new ones
        foreach (Transform child in levelContainer)
        {
            Destroy(child.gameObject); // Destroy all previous level items
        }

        // Now create level items
        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject levelItem = Instantiate(levelPrefab, levelContainer);
            TextMeshProUGUI levelText = levelItem.GetComponent<TextMeshProUGUI>(); // Assuming levelPrefab has a Text component

            if (levelText != null)
            {
                levelText.text = i.ToString(); // Set the level number

                // Set color for multiples of 5 and 30
                if (i % 30 == 0)
                {
                    levelText.color = multipleOf30Color; // Set yellow for multiples of 30
                }
                else if (i % 5 == 0)
                {
                    levelText.color = multipleOf5Color; // Set green for multiples of 5
                }
            }

            // Set the initial position
            RectTransform rectTransform = levelItem.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(0, -((i - 1) * rectTransform.rect.height)); // Initial positioning
            }
        }
    }
}
