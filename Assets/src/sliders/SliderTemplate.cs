using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SliderTemplate : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component

    public float minSliderValue = 0f; // Minimum value for the slider
    public float maxSliderValue = 100f; // Maximum value for the slider
    public float defaultSliderValue = 50f; // Default value for the slider

    public float currentValue = 0;

    public static SliderTemplate instace;

    void Awake() {
        instace = this;
    }

    public void Init() {
        // Set the min, max, and default values of the slider
        slider.minValue = minSliderValue;
        slider.maxValue = maxSliderValue;
        slider.value = defaultSliderValue;

        // Add a listener to respond to changes in the slider value
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    void HandleSliderValueChanged(float value) {
        // Do something with the new slider value
        currentValue = slider.value;
    }
}
