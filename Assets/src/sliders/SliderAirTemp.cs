public class SliderAirTemp : SliderTemplate
{

    public static SliderAirTemp instance;
    void Awake() {
        instance = this;
    }

    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().airStartingTemp.min;
        maxSliderValue = SliderManager.GetJsonSettings().airStartingTemp.max;
        defaultSliderValue = SliderManager.GetJsonSettings().airStartingTemp.defaultValue;

        variableName = "Air temperature";

        base.Init();
    }
    
}
