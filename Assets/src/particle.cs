using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
 
public class Particle
{
    List<Particle> edgeNeighbors = new();

    List<Particle> neighbors = new List<Particle>();
    public float temperature = 0;
    public float newTemperature = 0;

    public float thermalDiffusivity;
    public Color color;
    public Vector3 position;
    public int index;

    public static float minTemperature;
    public static float maxTemperature;
    internal string type;

    public List<Particle> getEdgesNeighbors() {
        if (edgeNeighbors.Count > 0) {
            return edgeNeighbors;
        }
        
        edgeNeighbors.Clear();
        foreach (Particle neighbor in this.neighbors) {
            if (neighbor.type == "edge") {
                edgeNeighbors.Add(neighbor);
            }
        }
        return edgeNeighbors;
    }

    public int getEdgesNeighborsCount() {
        return getEdgesNeighbors().Count;
    }
    
    public void addNeighbors(List<Particle> newNeighbors) {
        this.neighbors = newNeighbors;
    }

    public List<Particle> getNeighbors() {
        return this.neighbors;
    }

    public void MarkNeighborsAs(string type) {
        foreach (Particle neighbor in neighbors) {
            Debug.Log($"Marked {neighbor.position} as {type}");
            neighbor.type = type;
        }
    }

    public void calculateNewTemperature() {
        var distance =  SpawnParticles.instance.distanceBetweenParticles / 100f;
        var diffusity = thermalDiffusivity;
        var time = Time.deltaTime;

        float F = diffusity * time / Mathf.Pow(distance, 2);

        // F = 1f/6f;

        if (F > 1f/6f) {
            Debug.LogWarning($"F > 1/6 ({F})");
        }

        float leftPart = this.temperature * (1 - F);
    
        float neighborsSum = 0;

        for (int i = 0; i < 6; i ++) {
            if (i < neighbors.Count) {
                neighborsSum += neighbors[i].temperature;
            } else {
                neighborsSum += SliderAirTemp.instance.currentValue;
            }
        }

        float rightPart = F * neighborsSum;

        this.newTemperature = leftPart + rightPart;

        // Debug.Log($"Old temp: {temperature}, New temp: {newTemperature}");
    }

    public Particle() {
        // thermalDiffusivity = SliderDiffusity.instace.currentValue;

        // // TODO: this is quite goofy
        // minTemperature = SliderStartingTemp.instace.minSliderValue;
        // maxTemperature = SliderSourceTemp.instace.currentValue;

        // SetTemperature(SliderManager.GetJsonSettings().objectStartingTemp.defaultValue);
    }

    public void SetTemperature(float newTemperature) {
        if (this.type == "air") {
            this.temperature = SliderAirTemp.instance.currentValue;
            this.color = Color.cyan;
            return;
        }

        this.temperature = newTemperature;
        this.color = MapTemperatureToColor();
    }

    Color MapTemperatureToColor()
    {
        
        // Normalize temperature to a value between 0 and 1
        float normalizedTemperature = Mathf.InverseLerp(SliderStartingTemp.instance.minSliderValue, SliderSourceTemp.instance.currentValue, temperature);

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

