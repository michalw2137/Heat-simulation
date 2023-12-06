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

    public Text textField;
    public string variableName;

    void Awake() {
        instace = this;

        slider = GetComponent<Slider>();

        textField = GetComponentInChildren<Text>();
    }

    public void Init() {
        // Set the min, max, and default values of the slider
        slider.minValue = minSliderValue;
        slider.maxValue = maxSliderValue;
        slider.value = defaultSliderValue;

        // Add a listener to respond to changes in the slider value
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    protected void HandleSliderValueChanged(float value) {
        // Do something with the new slider value
        currentValue = slider.value;

        textField.text = $"{variableName}: {currentValue}";
    }
}
