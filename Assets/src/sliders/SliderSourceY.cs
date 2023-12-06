public class SliderSourceY : SliderTemplate
{
    public static SliderSourceY instance;
    void Awake() {
        instance = this;
    }

    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().sourceY.min;
        maxSliderValue = SliderManager.GetJsonSettings().sourceY.max;
        defaultSliderValue = SliderManager.GetJsonSettings().sourceY.defaultValue;

        variableName = "Source Y";

        base.Init();
        slider.onValueChanged.AddListener(updateHeatSource);
    }
    
    void updateHeatSource(float value) {
        HeatSource.instance.setY(value);
    }
    
}
