using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Wheel
{
    public class ModalManager : MonoBehaviour
    {
        public GameObject rewardModal; // Panel for claiming rewards
        public GameObject bombModal;   // Panel for landing on a bomb
        public GameObject endPanelModal;   // Panel for background

        public TextMeshProUGUI modalMessage; // Text for the modal message
        public Button claimRewardButton; // Button to claim reward
        public Button claimRewardAndExitButton; // Button to claim reward

        public Button restartButton; // Button to restart after bomb
        public LevelBar levelBar; // Reference to the LevelBar script

        private void Start()
        {
            // Ensure modals are disabled at the start


            // Set up button listeners
            claimRewardButton.onClick.AddListener(OnClaimReward);
            claimRewardAndExitButton.onClick.AddListener(OnClaimRewardAndExit);
            restartButton.onClick.AddListener(OnRestart);
            
        }

        // Method to show the correct modal based on the slice
        public void ShowModal(WheelSlice sliceData)
        {
            endPanelModal.SetActive(true);
            if (sliceData.name == "Bomb")
            {
                // Show bomb modal
                Image deathIcon = bombModal.transform.Find("death_image").GetComponent<Image>(); // Adjust the name "RewardIcon" to match your UI element
                TextMeshProUGUI deathText = bombModal.transform.Find("death_text").GetComponent<TextMeshProUGUI>(); // Adjust the name "RewardText" to match your UI element
                if (deathIcon != null && deathText != null)
                {
                    deathIcon.sprite = sliceData.icon; // Set the icon
                    deathText.text = "OH NO, A BOMB EXPLODED RIGHT IN YOUR HANDS!";   // Set the name
                }
                bombModal.SetActive(true);
            }
            else
            {
                Image rewardIcon = rewardModal.transform.Find("reward_image").GetComponent<Image>(); // Adjust the name "RewardIcon" to match your UI element
                TextMeshProUGUI rewardText = rewardModal.transform.Find("reward_text").GetComponent<TextMeshProUGUI>(); // Adjust the name "RewardText" to match your UI element
                if (rewardIcon != null && rewardText != null)
                {
                    rewardIcon.sprite = sliceData.icon; // Set the icon
                    rewardIcon.preserveAspect = true;
                    rewardText.text = "You win "+sliceData.text + " " + sliceData.name + "!";   // Set the name
                }
                Debug.Log("modalMessage");
                // Show reward modal
                rewardModal.SetActive(true);
            }
        }

        private void OnClaimReward()
        {
            // Hide the reward modal
            levelBar.LevelUp();
            rewardModal.SetActive(false);
            endPanelModal.SetActive(false);
            Debug.Log("Reward claimed. You can add your logic here.");
            // You can trigger level up or other actions here
        }

        private void OnClaimRewardAndExit()
        {
            // Hide the reward modal
            levelBar.RestartLevelBar();
            rewardModal.SetActive(false);
            endPanelModal.SetActive(false);
        }

        private void OnRestart()
        {
            // Hide the bomb modal and restart the game
            levelBar.RestartLevelBar();
            bombModal.SetActive(false);
            endPanelModal.SetActive(false);
            Debug.Log("Restarting game.");
        }
    }
}
