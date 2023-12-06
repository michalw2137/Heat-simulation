public class SliderStartingTemp : SliderTemplate
{
    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().objectStartingTemp.min;
        maxSliderValue = SliderManager.GetJsonSettings().objectStartingTemp.max;
        defaultSliderValue = SliderManager.GetJsonSettings().objectStartingTemp.defaultValue;

        variableName = "Object temperature";

        base.Init();
    }
    
}
