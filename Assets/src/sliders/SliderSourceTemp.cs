public class SliderSourceTemp : SliderTemplate
{
    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().sourceStartingTemp.min;
        maxSliderValue = SliderManager.GetJsonSettings().sourceStartingTemp.max;
        defaultSliderValue = SliderManager.GetJsonSettings().sourceStartingTemp.defaultValue;

        variableName = "Source temperature";

        base.Init();
    }
    
}
