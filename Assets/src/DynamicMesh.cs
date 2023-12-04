
using System.Collections.Generic;
using UnityEngine;

class DynamicMesh : MonoBehaviour{

    public Mesh mesh;

    public static DynamicMesh instance;

    void Awake() {
        instance = this;
    }

    public void GenerateMesh()
    {
        // Create a new mesh
        mesh = new Mesh();

        List<Particle> particles = SpawnParticles.instance.edgeParticles;

        // Set the vertex positions
        Vector3[] vertices = new Vector3[particles.Count];
        for (int i = 0; i < particles.Count; i++)
        {
            vertices[i] = new Vector3(particles[i].position.x, particles[i].position.y, particles[i].position.z);
        }
        mesh.vertices = vertices;

        // Automatically generate triangles based on the order of vertices
        mesh.triangles = GenerateTriangles(particles);

        // Create a new GameObject to hold the mesh
        GameObject newObj = new GameObject("CustomMesh");

        // Add MeshFilter and MeshRenderer components to the GameObject
        MeshFilter meshFilter = newObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newObj.AddComponent<MeshRenderer>();

        // Assign the created mesh to the MeshFilter
        meshFilter.mesh = mesh;

        // Create a new material (you can adjust this based on your preferences)
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.blue; // Set the color as needed

        // Assign the material to the MeshRenderer
        meshRenderer.material = material;
    }

    int[] GenerateTriangles(List<Particle> particles)
    {
        // Generate triangles based on the order of vertices
        List<int> triangles = new();
        
        foreach (Particle particle in particles) {
            if (particle.getNeighborsCount() == 3) {
                triangles.Add(particle.index);
                triangles.Add(particle.getNeighbors()[0].index);
                triangles.Add(particle.getNeighbors()[1].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getNeighbors()[0].index);
                triangles.Add(particle.getNeighbors()[2].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getNeighbors()[1].index);
                triangles.Add(particle.getNeighbors()[2].index);
            }

            if (particle.getNeighborsCount() == 4) {
                Vector3 oneAxisPosition = particle.getNeighbors()[0].position;
                int aAxisIndex1 = 0;
                int aAxisIndex2 = 4;

                for (int i = 1; i < 4; i++) {
                    if (particle.getNeighbors()[i].position.x == oneAxisPosition.x ||
                     particle.getNeighbors()[i].position.y == oneAxisPosition.y) {
                        aAxisIndex2 = i;
                        break;
                    }
                }

                if (aAxisIndex2 == 4) {
                    Debug.LogError("aAxisIndex2 has index 4");
                }
                
                int bAxisIndex1 = 4;
                int bAxisIndex2 = 4;
                for (int i = 1; i < 4; i++) {
                    if (i != aAxisIndex2) {
                        bAxisIndex1 = i;
                    }
                }

                if (bAxisIndex1 == 4) {
                    Debug.LogError("bAxisIndex1 has index 4");
                }

                for (int i = bAxisIndex1; i < 4; i++) {
                    if (i != aAxisIndex2) {
                        bAxisIndex2 = i;
                    }
                }

                if (bAxisIndex2 == 4) {
                    Debug.LogError("bAxisIndex2 has index 4");
                }

                triangles.Add(particle.index);
                triangles.Add(particle.getNeighbors()[aAxisIndex1].index);
                triangles.Add(particle.getNeighbors()[bAxisIndex1].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getNeighbors()[aAxisIndex1].index);
                triangles.Add(particle.getNeighbors()[bAxisIndex2].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getNeighbors()[aAxisIndex2].index);
                triangles.Add(particle.getNeighbors()[bAxisIndex1].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getNeighbors()[aAxisIndex2].index);
                triangles.Add(particle.getNeighbors()[bAxisIndex2].index);
            }
        }


        return triangles.ToArray();
    }
}