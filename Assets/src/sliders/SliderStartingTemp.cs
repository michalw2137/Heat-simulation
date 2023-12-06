public class SliderStartingTemp : SliderTemplate
{

    public static SliderStartingTemp instance;
    void Awake() {
        instance = this;
    }

    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().objectStartingTemp.min;
        maxSliderValue = SliderManager.GetJsonSettings().objectStartingTemp.max;
        defaultSliderValue = SliderManager.GetJsonSettings().objectStartingTemp.defaultValue;

        variableName = "Object temperature";

        base.Init();
    }
    
}
