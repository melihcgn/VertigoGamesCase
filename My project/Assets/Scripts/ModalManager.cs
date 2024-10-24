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

        private TextMeshProUGUI modalMessage; // Text for the modal message
        public Button claimRewardButton; // Button to claim reward
        public Button claimRewardAndExitButton; // Button to claim reward and exit
        public Button claimRewardAndLeaveButton; // Button to claim reward and leave

        public Button restartButton; // Button to restart after bomb
        public LevelBar levelBar; // Reference to the LevelBar script

        // This method will automatically set the button references in the editor
        private void OnValidate()
        {
            // If the references are not set, try to find them in the hierarchy
            if (claimRewardButton == null)
                claimRewardButton = transform.Find("RewardModal/ClaimRewardButton")?.GetComponent<Button>();

            if (claimRewardAndExitButton == null)
                claimRewardAndExitButton = transform.Find("RewardModal/ClaimRewardAndExitButton")?.GetComponent<Button>();

            if (claimRewardAndLeaveButton == null)
                claimRewardAndLeaveButton = GameObject.Find("Canvas/ClaimRewardAndLeaveButton")?.GetComponent<Button>();

            if (restartButton == null)
                restartButton = transform.Find("BombModal/RestartButton")?.GetComponent<Button>();

            if (rewardModal == null)
                rewardModal = transform.Find("RewardModal")?.gameObject;

            if (bombModal == null)
                bombModal = transform.Find("BombModal")?.gameObject;

            if (endPanelModal == null)
                endPanelModal = transform.Find("EndPanelModal")?.gameObject;


            if (levelBar == null)
                levelBar = FindObjectOfType<LevelBar>(); // Optionally find the LevelBar if not manually set

            // Log a message if any of the references are missing after validation
            if (claimRewardButton == null || claimRewardAndExitButton == null || restartButton == null || rewardModal == null || bombModal == null || endPanelModal == null || levelBar == null)
            {
                Debug.LogWarning("Some references were not set automatically. Please verify the object structure.");
            }
        }

        private void Start()
        {
            // Ensure modals are disabled at the start
            // Set up button listeners
            claimRewardButton.onClick.AddListener(OnClaimReward);
            claimRewardAndExitButton.onClick.AddListener(OnClaimRewardAndExit);
            claimRewardAndLeaveButton.onClick.AddListener(OnClaimRewardAndExit);
            restartButton.onClick.AddListener(OnRestart);
        }

        // Method to show the correct modal based on the slice
        public void ShowModal(WheelSlice sliceData)
        {
            endPanelModal.SetActive(true);

            if (sliceData.name == "Bomb")
            {
                Image deathIcon = bombModal.transform.Find("death_image").GetComponent<Image>();
                TextMeshProUGUI deathText = bombModal.transform.Find("death_text").GetComponent<TextMeshProUGUI>();
                if (deathIcon != null && deathText != null)
                {
                    deathIcon.sprite = sliceData.icon;
                    deathText.text = "OH NO, A BOMB EXPLODED RIGHT IN YOUR HANDS!";
                }
                bombModal.SetActive(true);
            }
            else
            {
                Image rewardIcon = rewardModal.transform.Find("reward_image").GetComponent<Image>();
                TextMeshProUGUI rewardText = rewardModal.transform.Find("reward_text").GetComponent<TextMeshProUGUI>();
                if (rewardIcon != null && rewardText != null)
                {
                    rewardIcon.sprite = sliceData.icon;
                    rewardIcon.preserveAspect = true;
                    rewardText.text = "You win " + sliceData.text + " " + sliceData.name + "!";
                }
                rewardModal.SetActive(true);
            }
        }

        private void OnClaimReward()
        {
            levelBar.LevelUp();
            rewardModal.SetActive(false);
            endPanelModal.SetActive(false);
        }

        private void OnClaimRewardAndExit()
        {
            levelBar.RestartLevelBar();
            rewardModal.SetActive(false);
            endPanelModal.SetActive(false);
        }

        private void OnRestart()
        {
            levelBar.RestartSpinWheel();
            levelBar.RestartLevelBar();
            bombModal.SetActive(false);
            endPanelModal.SetActive(false);
        }
    }
}
