using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
 
public class Particle
{
    List<Particle> neighbors = new List<Particle>();
    public float temperature = 0;
    public Color color;
    public Vector3 position;
    public int index;

    public static float minTemperature;
    public static float maxTemperature;


    public void addNeighbors(List<Particle> newNeighbors) {
        this.neighbors = newNeighbors;
    }

    public Particle() {
        // TODO: this is quite goofy
        minTemperature = SliderManager.GetJsonSettings().objectStartingTemp.min;
        maxTemperature = SliderManager.GetJsonSettings().sourceStartingTemp.max;
    }

    public void SetTemperature(float newTemperature) {
        this.temperature = newTemperature;
        this.color = MapTemperatureToColor();
    }

    Color MapTemperatureToColor()
    {
        
        // Normalize temperature to a value between 0 and 1
        float normalizedTemperature = Mathf.InverseLerp(minTemperature, maxTemperature, temperature);

        // Use the gradient function to map the normalized value to a color
        Color color = GradientColor(normalizedTemperature);

        return color;
    }

    Color GradientColor(float value)
    {
        Gradient gradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = Color.blue;
        colorKeys[0].time = 0.0f;
        colorKeys[1].color = Color.red;
        colorKeys[1].time = 1.0f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        gradient.SetKeys(colorKeys, alphaKeys);

        return gradient.Evaluate(value);
    }

}

