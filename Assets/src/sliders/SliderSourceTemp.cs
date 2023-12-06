public class SliderSourceTemp : SliderTemplate
{

    public static SliderSourceTemp instance;
    void Awake() {
        instance = this;
    }

    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().sourceStartingTemp.min;
        maxSliderValue = SliderManager.GetJsonSettings().sourceStartingTemp.max;
        defaultSliderValue = SliderManager.GetJsonSettings().sourceStartingTemp.defaultValue;

        variableName = "Source temperature";

        base.Init();
    }
    
}
