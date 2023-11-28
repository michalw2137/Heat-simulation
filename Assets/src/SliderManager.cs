using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class VariableSettings
{
    public float min;
    public float max;
    public float defaultValue;
}

[System.Serializable]
public class JsonSettings
{
    public VariableSettings objectLenght;
    public VariableSettings objectWidth;
    public VariableSettings objectDepth;
    public VariableSettings objectThermalDiffusivity;
    public VariableSettings objectStartingTemp;
    public VariableSettings airStartingTemp;
    public VariableSettings sourceX;
    public VariableSettings sourceY;
    public VariableSettings sourceZ;
    public VariableSettings sourceLenght;
    public VariableSettings sourceWidth;
    public VariableSettings sourceDepth;
    public VariableSettings sourceStartingTemp;
}

public class SliderManager : MonoBehaviour
{
    public GameObject sliderPrefab; // Reference to the slider prefab in the scene
    public Text TextVariableName;
    public Slider variableSlider;
    public Text TextVariableValue;

    public float verticalOffset = 30f; // Adjust this value based on your layout needs

    public float yPos;
    public string jsonFilePath = "config.json"; // Update the path to your JSON file

    public JsonSettings jsonSettings;

    public static SliderManager instance; // you can reference values from json via SliderManager.instance.jsonSettings.ObjectLength.defaultValue etc

    public static JsonSettings GetJsonSettings() { // or by SliderManager.GetJsonSettings().ObjectLength.defaultValue
        return instance.jsonSettings;
    }
    private void Start()
    {
        instance = this;

        // Read JSON data from file
        string json = File.ReadAllText(jsonFilePath);

        // Deserialize JSON into the JsonSettings object
        jsonSettings = JsonUtility.FromJson<JsonSettings>(json);

        // Initialize sliders for each variable
        CreateSlider("objectLenght", jsonSettings.objectLenght);
        CreateSlider("objectWidth", jsonSettings.objectWidth);
        CreateSlider("objectDepth", jsonSettings.objectDepth);
        CreateSlider("objectThermalDiffusivity", jsonSettings.objectThermalDiffusivity);
        CreateSlider("objectStartingTemp", jsonSettings.objectStartingTemp);
        CreateSlider("airStartingTemp", jsonSettings.airStartingTemp);
        CreateSlider("sourceX", jsonSettings.sourceX);
        CreateSlider("sourceY", jsonSettings.sourceY);
        CreateSlider("sourceZ", jsonSettings.sourceZ);
        CreateSlider("sourceLenght", jsonSettings.sourceLenght);
        CreateSlider("sourceWidth", jsonSettings.sourceWidth);
        CreateSlider("sourceDepth", jsonSettings.sourceDepth);
        CreateSlider("sourceStartingTemp", jsonSettings.sourceStartingTemp);
    }

    private void CreateSlider(string variableName, VariableSettings variableSettings)
    {
        GameObject sliderObject = Instantiate(sliderPrefab, this.transform);

        sliderObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);
        yPos -= verticalOffset;

        Slider slider = sliderObject.GetComponent<Slider>();

        Text nameText = sliderObject.transform.Find("TextVariableName").GetComponent<Text>();
        Text valueText = sliderObject.transform.Find("TextVariableValue").GetComponent<Text>();

        nameText.text = variableName;
        slider.minValue = variableSettings.min;
        slider.maxValue = variableSettings.max;
        slider.value = variableSettings.defaultValue;
        valueText.text = slider.value.ToString("F3");

        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(slider, valueText); });
    }

    private void OnSliderValueChanged(Slider slider, Text valueText)
    {
        valueText.text = slider.value.ToString("F3");
    }
}
