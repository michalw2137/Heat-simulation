using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json;
using System.IO;
using System;

public class spawnParticles : MonoBehaviour
{
    private static float distanceBetweenParticles = 1;
    private static Vector3 increaseX = new Vector3(distanceBetweenParticles, 0, 0);
    private static Vector3 increaseY = new Vector3(0, distanceBetweenParticles, 0);
    private static Vector3 increaseZ = new Vector3(0, 0, distanceBetweenParticles);

    private static Vector3 decreaseX = new Vector3(-distanceBetweenParticles, 0, 0);
    private static Vector3 decreaseY = new Vector3(0, -distanceBetweenParticles, 0);
    private static Vector3 decreaseZ = new Vector3(0, 0, -distanceBetweenParticles);

    private static List<Vector3> expansionDirections = new List<Vector3> {
        increaseX,
        increaseY,
        increaseZ,
        decreaseX,
        decreaseY,
        decreaseZ
    };

    private Dictionary<Vector3, Particle> allParticlesMap = new Dictionary<Vector3, Particle>();

    public GameObject particleObject;

    [Serializable]
    public class SerializableVector3List
    {
        public List<Vector3> positions;

        public SerializableVector3List(List<Vector3> positions)
        {
            this.positions = positions;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Particle startingParticle = new() {
            position = particleObject.transform.position
        };

        SpawnParticleWithNeighbors(startingParticle, 0);
        
        Debug.LogWarning("FINISHED SPAWNING PARTICLES");
        Debug.LogWarning($"Particles count: {allParticlesMap.Count}");

        // Specify the file path where you want to save the JSON
        string filePath = "particleList.json";

        
        // Serialize and save the list to a file using JsonUtility
        List<Vector3> positionList = new List<Vector3>(allParticlesMap.Keys);
        string json = JsonUtility.ToJson(new SerializableVector3List(positionList), true);
        File.WriteAllText(filePath, json);

        Debug.Log(json);

        foreach(Vector3 position in allParticlesMap.Keys) {
            // Instantiate the prefab at a position based on 'i'
            GameObject sphere = Instantiate(particleObject, position, Quaternion.identity);
            sphere.transform.parent = this.transform;
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

            if(PointCollidesWithGameObject(neighborPosition, gameObject)) {
                neighbors.Add(particle);
                Debug.Log($"Neighbor at {startingParticle.position} collides with shape, adding it to neighbors list");
            } else {
                Debug.Log($"Neighbor at {startingParticle.position} is out of bounds");
                newParticles.Remove(particle);
            }
        }

        startingParticle.addNeighbors(neighbors);
        Debug.Log($"Added neighbors count: {neighbors.Count}");

        foreach(Particle neighbor in newParticles) {
            index += 1000;
            SpawnParticleWithNeighbors(neighbor, index);
        }
    }  

    // Function to check if a point collides with a GameObject's collider
    bool PointCollidesWithGameObject(Vector3 point, GameObject gameObject)
    {
        // Ensure the GameObject has a collider
        if (gameObject.TryGetComponent<Collider>(out var collider))
        {
            // Check if the point is within the bounds of the collider
            return collider.bounds.Contains(point);
        }
        else
        {
            Debug.LogWarning("The GameObject doesn't have a Collider component.");
            return false;
        }
    }
}