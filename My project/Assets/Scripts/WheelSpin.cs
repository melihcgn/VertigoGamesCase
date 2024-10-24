using UnityEngine;
using System.Collections; // Add this line
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
namespace Wheel
{
    public class WheelSpin : MonoBehaviour
    {
        public float spinSpeed = 1000f; // Initial speed of the spin
        public float deceleration = 5f;  // How quickly the wheel slows down
        private bool isSpinning = false; // To track if the wheel is spinning
        private float targetAngle; // The angle to snap to after spinning
        public GameObject slicePrefab; // Reference to your WheelSlice prefab
        public List<WheelSlice> wheelSlices; // List of slices on the wheel
        public ModalManager modalManager;
        private float initialAngle;

        private void Start()
        {
            initialAngle = transform.eulerAngles.z; // Store the initial angle
            targetAngle = initialAngle;            // Initialize the target angle to the current angle
            Debug.Log("initialAngle:initialAngle: " + initialAngle);
            InstantiateWheelSlices();
        }

        void Update()
        {
            // Spin the wheel if it's spinning
            if (isSpinning)
            {
                // Rotate the wheel
                transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

                // Decrease the speed gradually
                spinSpeed -= deceleration * Time.deltaTime;

                // Stop the wheel when the speed is low enough
                if (spinSpeed <= 0)
                {
                    spinSpeed = 0;
                    isSpinning = false;
                    SnapToNearestAngle(); // Snap to the nearest angle after stopping
                }
            }
        }
        private Sprite LoadSprite(string path)
        {
            Sprite sprite = Resources.Load<Sprite>(path);
            if (sprite == null)
            {
                Debug.LogWarning($"Failed to load sprite at path: {path}");
            }
            return sprite;
        }
        private void InstantiateWheelSlices()
        {
            float radius = 120f; // Adjust this radius to fit your wheel size
            int totalSlices = wheelSlices.Count; // Total number of slices

            // Loop to instantiate wheel slices based on your data
            for (int i = 0; i < totalSlices; i++)
            {
                WheelSlice sliceData = wheelSlices[i];
                GameObject slice = Instantiate(slicePrefab, transform); // Instantiate the prefab

                // Calculate the angle for the current slice
                float angle = i * (360f / totalSlices);

                // Position the slice in a circular formation
                float radian = angle * Mathf.Deg2Rad; // Convert angle to radians
                float x = Mathf.Cos(radian) * radius; // Calculate x position
                float y = Mathf.Sin(radian) * radius; // Calculate y position

                slice.transform.localPosition = new Vector3(x, y, 0); // Set local position
                slice.transform.localRotation = Quaternion.Euler(0, 0, -angle); // Set rotation based on angle

                // Get the Image component if it's a UI Image
                // Get the Image component if it's a UI Image
                Image sliceImage = slice.GetComponent<Image>();
                if (sliceImage != null)
                {
                    sliceImage.sprite = sliceData.icon; // Assign the sprite if using UI
                }

                // Set the text from the WheelSlice name
                TextMeshProUGUI textComponent = slice.GetComponentInChildren<TextMeshProUGUI>(); // or TextMeshProUGUI
                if (textComponent != null)
                {
                    textComponent.text = sliceData.text; // Set the text from the WheelSlice
                }
            }
        }
        // Call this function to start spinning the wheel
        public void StartSpinning()
        {
            if (!isSpinning)
            {
                // Randomize the spin speed for variation
                spinSpeed = Random.Range(800f, 1200f);
                isSpinning = true;

                // Calculate the target angle to snap to
                targetAngle = GetRandomTargetAngle();
            }
        }

        // Snap the wheel to the nearest target angle
        // Snap the wheel to the nearest target angle and detect the slice
        private void SnapToNearestAngle()
        {
            float currentAngle = transform.eulerAngles.z; // Get the current angle of the wheel
            float nearestAngle = GetNearestAngle(currentAngle); // Get the nearest target angle
            Debug.Log("currentAngle: " + currentAngle);

            // Adjust the angle by subtracting the initial angle (set during the Start)
            float adjustedAngle = (nearestAngle - initialAngle + 360f) % 360f;

            //for counterclockwise
            // Calculate the slice index based on the adjusted angle
            int sliceNum = Mathf.RoundToInt(adjustedAngle / (360f / wheelSlices.Count));
            int sliceIndex = sliceNum % wheelSlices.Count;
            Debug.Log("initial angle: " + initialAngle);
            // Ensure the index is valid
            if (sliceIndex >= 0 && sliceIndex < wheelSlices.Count)
            {
                // Get the correct slice data
                WheelSlice selectedSlice = wheelSlices[(wheelSlices.Count - sliceIndex +2) % wheelSlices.Count];
                Debug.Log("Landed on slice: " + selectedSlice.name); // Debugging line

                // Show the correct modal based on the selected slice data
                modalManager.ShowModal(selectedSlice); // Pass the correct slice data
            }

            // Smoothly rotate to the nearest angle
            StartCoroutine(RotateToAngle(nearestAngle));
        }


        // Get a random target angle from predefined angles
        private float GetRandomTargetAngle()
        {
            float[] angles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
            return angles[Random.Range(0, angles.Length)];
        }

        // Find the nearest angle in the predefined angles
        private float GetNearestAngle(float angle)
        {
            float[] angles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
            float closest = angles[0];
            float closestDiff = Mathf.Abs(angle - closest);

            foreach (float a in angles)
            {
                float diff = Mathf.Abs(angle - a);
                if (diff < closestDiff)
                {
                    closestDiff = diff;
                    closest = a;
                }
            }

            return closest;
        }

        // Coroutine to smoothly rotate to the target angle
        private IEnumerator RotateToAngle(float target)
        {
            float duration = 0.5f; // Duration for the rotation
            float startAngle = transform.eulerAngles.z;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                float angle = Mathf.Lerp(startAngle, target, elapsedTime / duration);
                transform.eulerAngles = new Vector3(0, 0, angle);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.eulerAngles = new Vector3(0, 0, target); // Ensure it ends exactly on target
        }
    }
}
