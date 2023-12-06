public class SliderSourceScale : SliderTemplate
{

    public static SliderSourceScale instance;
    void Awake() {
        instance = this;
    }

    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().sourceScale.min;
        maxSliderValue = SliderManager.GetJsonSettings().sourceScale.max;
        defaultSliderValue = SliderManager.GetJsonSettings().sourceScale.defaultValue;

        variableName = "Source scale";

        base.Init();
        slider.onValueChanged.AddListener(updateHeatSource);
    }
    
    void updateHeatSource(float value) {
        HeatSource.instance.setScale(value);
    }
    
}
