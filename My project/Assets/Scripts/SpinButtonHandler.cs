using UnityEngine;
using UnityEngine.UI; // For accessing the UI components

namespace Wheel
{
    public class SpinButtonHandler : MonoBehaviour
    {
        public Button spinButton;  // Reference to your button "ui_generic_spin"
        private WheelSpin wheelSpin; // Reference to the WheelSpin script

        void Start()
        {
            // Get the Button component if not manually set
            if (spinButton == null)
            {
                spinButton = GameObject.Find("ui_spin_generic_button").GetComponent<Button>();
            }

            // Get the WheelSpin script component from the same GameObject or find it in the scene
            wheelSpin = GameObject.FindObjectOfType<WheelSpin>(); // Assumes there's one instance of WheelSpin in the scene

            // Attach a listener to the button click event
            spinButton.onClick.AddListener(() => wheelSpin.StartSpinning());  // Call StartSpinning when the button is clicked
        }
    }
}
