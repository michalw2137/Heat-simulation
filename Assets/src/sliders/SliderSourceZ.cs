public class SliderSourceZ : SliderTemplate
{

    public static SliderSourceZ instance;
    void Awake() {
        instance = this;
    }

    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().sourceZ.min;
        maxSliderValue = SliderManager.GetJsonSettings().sourceZ.max;
        defaultSliderValue = SliderManager.GetJsonSettings().sourceZ.defaultValue;

        variableName = "Source Z";

        HeatSource.instance.setZ(defaultSliderValue);

        base.Init();
        slider.onValueChanged.AddListener(updateHeatSource);
    }
    
    void updateHeatSource(float value) {
        HeatSource.instance.setZ(value);
    }
    
}
