using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
 
public class Particle
{
    List<Particle> neighbors = new List<Particle>();
    public float temperature = 0;
    public Vector3 position;
    public int index;

    public void addNeighbors(List<Particle> newNeighbors) {
        this.neighbors = newNeighbors;
    }

}

