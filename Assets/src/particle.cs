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

        string neighborsTemps = "";

        for (int i = 0; i < 6; i ++) {
            if (i < neighbors.Count) {
                neighborsSum += neighbors[i].temperature;

                neighborsTemps += neighbors[i].temperature + ", ";
            } else {
                neighborsSum += SliderAirTemp.instance.currentValue;

                neighborsTemps += SliderAirTemp.instance.currentValue + ", ";
            }
        }

        float rightPart = F * neighborsSum / 6.0f;

        this.newTemperature = leftPart + rightPart;

        // if (temperature != newTemperature && temperature < SliderSourceTemp.instance.currentValue) 
        //     Debug.Log($"Old: {temperature}, New: {newTemperature}, F: {F}, n: {neighborsTemps}, right: {rightPart}, left: {leftPart}");
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

    Color MapTemperatureToColor() {
        // Map temperature values to a color gradient
        float normalizedTemperature = Mathf.Clamp01((temperature - 253.0f) / (2273.0f - 253.0f));

        // Define the custom color scale
        Color color0 = new Color(0.5f, 0, 1); 
        Color color1 = new Color(0, 0, 1); 
        Color color2 = new Color(0, 0.5f, 1);
        Color color3 = new Color(0, 1, 1);
        Color color4 = new Color(0, 1, 0.5f);
        Color color5 = new Color(0, 1, 0);
        Color color6 = new Color(0.5f, 1, 0);
        Color color7 = new Color(1, 1, 0);
        Color color8 = new Color(1, 0.5f, 0);
        Color color9 = new Color(1, 0, 0);
        Color color10 = new Color(0.5f, 0, 0);

        Color color;

        if (normalizedTemperature < 0.1f) {
            float t = 10 * normalizedTemperature;
            color = Color.Lerp(color0, color1, t);
        } 
        else if (normalizedTemperature < 0.2f) {
            float t = 10 * (normalizedTemperature - 0.1f);
            color = Color.Lerp(color1, color2, t);
        } 
        else if (normalizedTemperature < 0.3f) {
            float t = 10 * (normalizedTemperature - 0.2f);
            color = Color.Lerp(color2, color3, t);
        } 
        else if (normalizedTemperature < 0.4f) {
            float t = 10 * (normalizedTemperature - 0.3f);
            color = Color.Lerp(color3, color4, t);
        } 
        else if (normalizedTemperature < 0.5f) {
            float t = 10 * (normalizedTemperature - 0.4f);
            color = Color.Lerp(color4, color5, t);
        } 
        else if (normalizedTemperature < 0.6f) {
            float t = 10 * (normalizedTemperature - 0.5f);
            color = Color.Lerp(color5, color6, t);
        } 
        else if (normalizedTemperature < 0.7f) {
            float t = 10 * (normalizedTemperature - 0.6f);
            color = Color.Lerp(color6, color7, t);
        } 
        else if (normalizedTemperature < 0.8f) {
            float t = 10 * (normalizedTemperature - 0.7f);
            color = Color.Lerp(color7, color8, t);
        } 
        else if (normalizedTemperature < 0.9f) {
            float t = 10 * (normalizedTemperature - 0.8f);
            color = Color.Lerp(color8, color9, t);
        } 
        else {
            float t = 10 * (normalizedTemperature - 0.9f);
            color = Color.Lerp(color9, color10, t);
        } 

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

