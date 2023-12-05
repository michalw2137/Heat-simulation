public class SliderAirTemp : SliderTemplate
{
    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().airStartingTemp.min;
        maxSliderValue = SliderManager.GetJsonSettings().airStartingTemp.max;
        defaultSliderValue = SliderManager.GetJsonSettings().airStartingTemp.defaultValue;

        base.Init();
    }
    
}
