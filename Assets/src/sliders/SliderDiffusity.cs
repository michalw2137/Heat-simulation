public class SliderDiffusity : SliderTemplate
{
    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().objectThermalDiffusivity.min;
        maxSliderValue = SliderManager.GetJsonSettings().objectThermalDiffusivity.max;
        defaultSliderValue = SliderManager.GetJsonSettings().objectThermalDiffusivity.defaultValue;

        base.Init();
    }
    
}
