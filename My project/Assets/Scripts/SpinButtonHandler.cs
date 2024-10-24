using UnityEngine;
using UnityEngine.UI;

namespace Wheel
{
    public class SpinButtonHandler : MonoBehaviour
    {
        public Button spinButton;  // Reference to your button "ui_generic_spin"
        private WheelSpin wheelSpin; // Reference to the WheelSpin script

        // Automatically set references when modified in the editor
        private void OnValidate()
        {
            // Automatically assign spinButton if not manually set
            if (spinButton == null)
            {
                spinButton = transform.Find("ui_spin_generic_button")?.GetComponent<Button>();

                // Log a message if the button couldn't be found
                if (spinButton == null)
                {
                    Debug.LogWarning("Spin button reference not set and 'ui_spin_generic_button' not found in the hierarchy.");
                }
            }

            // Automatically find the WheelSpin script in the scene if it's not set
            if (wheelSpin == null)
            {
                wheelSpin = FindObjectOfType<WheelSpin>();

                // Log a message if WheelSpin couldn't be found
                if (wheelSpin == null)
                {
                    Debug.LogWarning("WheelSpin script not found in the scene.");
                }
            }
        }

        void Start()
        {
            // Ensure the button and WheelSpin reference are set before adding the listener
            if (spinButton == null || wheelSpin == null)
            {
                Debug.LogError("SpinButtonHandler is missing a reference to spinButton or WheelSpin.");
                return;
            }

            // Attach a listener to the button click event
            spinButton.onClick.AddListener(() => wheelSpin.StartSpinning());  // Call StartSpinning when the button is clicked
        }
    }
}
