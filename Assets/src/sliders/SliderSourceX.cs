public class SliderSourceX : SliderTemplate
{
    void Start() {
        minSliderValue = SliderManager.GetJsonSettings().sourceX.min;
        maxSliderValue = SliderManager.GetJsonSettings().sourceX.max;
        defaultSliderValue = SliderManager.GetJsonSettings().sourceX.defaultValue;

        variableName = "Source X";

        base.Init();

        slider.onValueChanged.AddListener(updateHeatSource);
    }
    
    void updateHeatSource(float value) {
        HeatSource.instance.setX(value);
    }

}
