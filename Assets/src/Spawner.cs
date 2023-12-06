using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;
using System.IO;
using System;

public class SpawnParticles : MonoBehaviour
{
    public float distanceBetweenParticles = 1;

    List<Vector3> expansionDirections;

    public Dictionary<Vector3, Particle> allParticlesMap = new Dictionary<Vector3, Particle>();

    public Dictionary<Particle, GameObject> particleObjects = new();

    public GameObject particleObject;
    public GameObject heatSource; // TODO: probably shouldnt be here
    public static SpawnParticles instance;

    [Serializable]
    public class SerializableVector3List
    {
        public List<Vector3> positions;

        public SerializableVector3List(List<Vector3> positions)
        {
            this.positions = positions;
        }
    }

    public List<Particle> airParticles = new();
    public List<Particle> edgeParticles = new();

    private void Awake() {
        instance = this;

        Vector3 increaseX = new Vector3(distanceBetweenParticles, 0, 0);
        Vector3 increaseY = new Vector3(0, distanceBetweenParticles, 0);
        Vector3 increaseZ = new Vector3(0, 0, distanceBetweenParticles);

        Vector3 decreaseX = new Vector3(-distanceBetweenParticles, 0, 0);
        Vector3 decreaseY = new Vector3(0, -distanceBetweenParticles, 0);
        Vector3 decreaseZ = new Vector3(0, 0, -distanceBetweenParticles);

        expansionDirections = new List<Vector3> {
            increaseX,
            increaseY,
            increaseZ,
            decreaseX,
            decreaseY,
            decreaseZ
        };
    }

    public void setThermalDiffusities(float diffusity) {
        foreach (var kvp in particleObjects) { // TODO: perhaps have straight list of particles lol
            Particle particle = kvp.Key;
            particle.thermalDiffusivity = diffusity;
        }
    }

    public void setStartingTemp() {
        foreach (var kvp in particleObjects) { // TODO: perhaps have straight list of particles lol
            Particle particle = kvp.Key;
            GameObject ball = kvp.Value;

            if (particle.type == "air") {
                particle.SetTemperature(SliderAirTemp.instance.currentValue);
            } else if (PointCollidesWithGameObject(particle.position, heatSource)) {
                Debug.Log($"({particle.position}) set to temperature {SliderSourceTemp.instance.currentValue}");
                particle.SetTemperature(SliderSourceTemp.instance.currentValue);
            } else {
                particle.SetTemperature(SliderStartingTemp.instance.currentValue);
            }

            ball.GetComponent<Renderer>().material.color = particle.color; // TODO: make gameObject field in Particle class
        }
    }


    void FixedUpdate() {
        if (!InputManager.instance.isSimulationOn) {
            return;
        }

        foreach (var kvp in particleObjects) { // TODO: perhaps have straight list of particles lol
            Particle particle = kvp.Key;
            GameObject ball = kvp.Value;

            if (PointCollidesWithGameObject(particle.position, heatSource)) {
                particle.SetTemperature(SliderSourceTemp.instance.currentValue);
                particle.newTemperature = particle.temperature;
            } 

            particle.calculateNewTemperature();
            
            particle.SetTemperature(particle.newTemperature);
            ball.GetComponent<Renderer>().material.color = particle.color; // TODO: make gameObject field in Particle class

        }

        // foreach (var kvp in particleObjects) { // TODO: perhaps have straight list of particles lol
        //     Particle particle = kvp.Key;
        //     GameObject ball = kvp.Value;
            
        //     particle.SetTemperature(particle.newTemperature);
        //     ball.GetComponent<Renderer>().material.color = particle.color; // TODO: make gameObject field in Particle class
        // }

        // Debug.Log("Updated all particles temperature");

        DynamicMesh.instance.updateColors();
    }

    // Start is called before the first frame update
    void Start()
    {
        Particle startingParticle = new() {
            position = particleObject.transform.position
        };

        SpawnParticleWithNeighbors(startingParticle, 0);

        Debug.Log(airParticles.Count);

        Debug.LogWarning("FINISHED SPAWNING PARTICLES");
        Debug.LogWarning($"Particles count: {allParticlesMap.Count}");
        Debug.LogWarning($"Edges count: {edgeParticles.Count}");

        // Specify the file path where you want to save the JSON
        string filePath = "particleList.json";

        
        // Serialize and save the list to a file using JsonUtility
        List<Vector3> positionList = new List<Vector3>(allParticlesMap.Keys);
        string json = JsonUtility.ToJson(new SerializableVector3List(positionList), true);
        File.WriteAllText(filePath, json);

        Debug.Log(json);

        // Specify the file path where you want to save the JSON
        string edgeFilePath = "edgeParticles.json";

        // Serialize and save the list to a file using JsonUtility
        List<Vector3> edgePositionList = new();
        foreach (Particle edge in edgeParticles) {
            edgePositionList.Add(edge.position);
        }

        string edgJjson = JsonUtility.ToJson(new SerializableVector3List(edgePositionList), true);
        File.WriteAllText(edgeFilePath, edgJjson);


        foreach(Vector3 position in allParticlesMap.Keys) {
            // Map instantiated game objects to Particle scripts
            Particle particle = allParticlesMap[position];
            particle.SetTemperature(particle.temperature);

            if (particle.type != "air") {
                // Instantiate the prefab at a position based on 'i'
                GameObject sphere = Instantiate(particleObject, position, Quaternion.identity);

                particleObjects[particle] = sphere;

                // Add this as parent so editor isn't flooded
                sphere.transform.parent = this.transform;
            }
        }

        Debug.ClearDeveloperConsole();

        // set temperature for particles touching heat source
        foreach (var kvp in particleObjects) { // TODO: perhaps have straight list of particles lol
            Particle particle = kvp.Key;
            GameObject ball = kvp.Value;

            if (particle.type == "air") {
                particle.SetTemperature(278f);
                ball.GetComponent<Renderer>().material.color = Color.white; // TODO: make gameObject field in Particle class

                continue;
            }

            // if (particle.type == "edge") {
            //     ball.GetComponent<Renderer>().material.color = Color.black; // TODO: make gameObject field in Particle class

            //     continue;
            // }

            if (PointCollidesWithGameObject(particle.position, heatSource)) {
                particle.SetTemperature(2500f); // TODO: move heat temp to HeatSource and read it from here
                ball.GetComponent<Renderer>().material.color = particle.color; // TODO: make gameObject field in Particle class

                // Debug.Log($"Set {particle.position} to heat source temperature {particle.temperature}");
            }
        }
    }

    // Update is called once per frame
    void SpawnParticleWithNeighbors(Particle startingParticle, int index = 0) {
        Debug.Log($"Spawning neighbours for Particle#{index} at {startingParticle.position}");
        // Create neighbors in all directions
        List<Particle> neighbors = new();
        List<Particle> newParticles = new();

        foreach(Vector3 expansionDirection in expansionDirections) {
            Debug.Log($"Trying to expand in {expansionDirection}...");
            Vector3 neighborPosition = startingParticle.position + expansionDirection;
            Particle particle;

            // Check if such particle already exists
            if (allParticlesMap.ContainsKey(neighborPosition)) {
                particle = allParticlesMap[neighborPosition]; 
                Debug.Log($"Particle at {neighborPosition} already exists");
            } else {
                // Create new particle
                particle = new Particle();
                particle.position = neighborPosition;
                index ++;
                particle.index = index;
                Debug.Log($"Particle at {neighborPosition} not found, creating it with index {index}");

                allParticlesMap[neighborPosition] = particle;
                newParticles.Add(particle);
            }

            if (PointCollidesWithGameObject(neighborPosition, gameObject)) {
                neighbors.Add(particle);
                Debug.Log($"Neighbor at {startingParticle.position} collides with shape, adding it to neighbors list");
            } else {
                Debug.Log($"Neighbor at {startingParticle.position} is out of bounds");
                newParticles.Remove(particle);

                particle.type = "air";

                airParticles.Add(particle);
            }
        }

        startingParticle.addNeighbors(neighbors);
        Debug.Log($"Added neighbors count: {neighbors.Count}");

        if (neighbors.Count < expansionDirections.Count) {
            Debug.Log($"Particle {startingParticle.position} has only {neighbors.Count} neighbors, marking it as edge");
            startingParticle.type = "edge";
            edgeParticles.Add(startingParticle);
            startingParticle.index = edgeParticles.Count - 1;
        }

        foreach(Particle neighbor in newParticles) {
            index += 1000;
            SpawnParticleWithNeighbors(neighbor, index);
        }
    }  

    // Function to check if a point collides with any of the GameObject's colliders
    bool PointCollidesWithGameObject(Vector3 point, GameObject gameObject)
    {
        // Get all colliders attached to the GameObject
        Collider[] colliders = gameObject.GetComponents<Collider>();

        if (colliders.Length > 0)
        {
            // Check if the point is within the bounds of any of the colliders
            foreach (Collider collider in colliders)
            {
                if (collider.bounds.Contains(point))
                {
                    return true; // Point is inside one of the colliders
                }
            }

            return false; // Point is outside all colliders
        }
        else
        {
            // If the GameObject doesn't have any Collider components, log a warning
            Debug.LogWarning("The GameObject doesn't have any Collider components.");
            return false;
        }
    }
}
