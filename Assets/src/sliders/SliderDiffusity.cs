public class SliderDiffusity : SliderTemplate
{

    public static SliderDiffusity instance;
    void Awake() {
        instance = this;
    }

    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().objectThermalDiffusivity.min;
        maxSliderValue = SliderManager.GetJsonSettings().objectThermalDiffusivity.max;
        defaultSliderValue = SliderManager.GetJsonSettings().objectThermalDiffusivity.defaultValue;

        variableName = "Thermal diffusity";

        base.Init();
    }
    
}
