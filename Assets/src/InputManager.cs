using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the "Enter" key is pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            foreach (var kvp in SpawnParticles.instance.particleObjects)
            {
                Particle particle = kvp.Key;
                GameObject gameObject = kvp.Value;

                // Now you can use 'particle' and 'gameObject' as needed
                particle.SetTemperature(particle.temperature + 100);
                gameObject.GetComponent<Renderer>().material.color = particle.color; // TODO: make gameObject field in Particle class
            }            
        }
    }
}
