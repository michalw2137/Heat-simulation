public class SliderSourceScale : SliderTemplate
{
    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().sourceScale.min;
        maxSliderValue = SliderManager.GetJsonSettings().sourceScale.max;
        defaultSliderValue = SliderManager.GetJsonSettings().sourceScale.defaultValue;

        variableName = "Source scale";

        base.Init();
        slider.onValueChanged.AddListener(updateHeatSource);
    }
    
    void updateHeatSource(float value) {
        HeatSource.instance.setScale(value);
    }
    
}
