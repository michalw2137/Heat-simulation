public class SliderSourceX : SliderTemplate
{

    public static SliderSourceX instance;
    void Awake() {
        instance = this;
    }

    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().sourceX.min;
        maxSliderValue = SliderManager.GetJsonSettings().sourceX.max;
        defaultSliderValue = SliderManager.GetJsonSettings().sourceX.defaultValue;

        variableName = "Source X";

        HeatSource.instance.setX(defaultSliderValue);

        base.Init();

        slider.onValueChanged.AddListener(updateHeatSource);
    }
    
    void updateHeatSource(float value) {
        HeatSource.instance.setX(value);
    }

}
