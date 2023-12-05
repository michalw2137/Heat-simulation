
using System.Collections.Generic;
using UnityEngine;

class DynamicMesh : MonoBehaviour{

    public Mesh mesh;

    public static DynamicMesh instance;

    void Awake() {
        instance = this;

        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void updateColors() {
        var vertices = mesh.vertices;

        int verticesCount = vertices.Length;

        Color[] colors = new Color[verticesCount];

        int i = 0;
        foreach (var vertex in vertices)
        {
            var particle = SpawnParticles.instance.allParticlesMap[vertex];
            colors[i++] = particle.color; // Assuming Particle has a 'color' field
        }

        mesh.colors = colors; // Set the colors array
    }

    public void GenerateMesh()
    {
        return;

        // Create a new mesh
        mesh = new Mesh();

        List<Particle> particles = SpawnParticles.instance.edgeParticles;

        // Set the vertex positions
        Vector3[] vertices = new Vector3[particles.Count];
        Color[] colors = new Color[particles.Count];

        for (int i = 0; i < particles.Count; i++)
        {
            vertices[i] = new Vector3(particles[i].position.x, particles[i].position.y, particles[i].position.z);
            colors[i] = particles[i].color; // Assuming Particle has a 'color' field
        }
        Debug.Log("vertices: " + vertices.Length);

        mesh.vertices = vertices;
        mesh.colors = colors; // Set the colors array

        // Automatically generate triangles based on the order of vertices
        var tr = GenerateTriangles(particles);

        int j = 0;
        while (j < tr.Length) {
            if (tr[j++] == vertices.Length) {
                Debug.LogError("TOO HIGH INDEX at j=" + j);
            }
            // Debug.Log($"triangle: {tr[j++]}, {tr[j++]}, {tr[j++]}");
        }

        Debug.Log("triangles indices: " + tr.Length);
        mesh.triangles = tr; 

        // Create a new GameObject to hold the mesh
        GameObject newObj = new GameObject("CustomMesh");

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        // Add MeshFilter and MeshRenderer components to the GameObject
        MeshFilter meshFilter = newObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newObj.AddComponent<MeshRenderer>();

        // Assign the created mesh to the MeshFilter
        meshFilter.mesh = mesh;

        // Create a new material with your custom shader
        Material material = new Material(Shader.Find("Custom/VertexColorShader")); // Use the name of your custom shader
        // Set other material properties as needed
        material.color = Color.blue; // Set the color as needed

        // Assign the material to the MeshRenderer
        meshRenderer.material = material;
    }


    int[] GenerateTriangles(List<Particle> particles)
    {
        // Generate triangles based on the order of vertices
        List<int> triangles = new();
        
        int edge2 = 0;
        int edge3 = 0;
        int edge4 = 0;
        int edge5 = 0;
        int edge6 = 0;

        foreach (Particle particle in particles) {
            if (particle.getEdgesNeighborsCount() == 1) {
                Debug.LogError("XDDDDDDDDDDDD 11111");
            }

            if (particle.getEdgesNeighborsCount() == 2) {
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[1].index); 
                Debug.LogError("XDDDDDDDDDDDD 2222");

                edge2 ++;
            }


            if (particle.getEdgesNeighborsCount() == 3) {
                // 0
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);

                // 1
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);

                edge3 ++;
            }

            if (particle.getEdgesNeighborsCount() == 4) {
                // 0 
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);
                
                // 1
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);

                // 2
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);

                edge4++;
            }

            if (particle.getEdgesNeighborsCount() == 5) {
                // 0 
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);
                
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[4].index);
                
                // 1
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[4].index);

                // 2
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);
                triangles.Add(particle.getEdgesNeighbors()[4].index);

                // 3
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);
                triangles.Add(particle.getEdgesNeighbors()[4].index);

                edge5 ++;
            }

            if (particle.getEdgesNeighborsCount() == 6) {
                // 0 
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);
                
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[4].index);
                
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[0].index);
                triangles.Add(particle.getEdgesNeighbors()[5].index);

                // 1
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[4].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[1].index);
                triangles.Add(particle.getEdgesNeighbors()[5].index);
                
                // 2
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);
                triangles.Add(particle.getEdgesNeighbors()[4].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[2].index);
                triangles.Add(particle.getEdgesNeighbors()[5].index);
                
                // 3
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);
                triangles.Add(particle.getEdgesNeighbors()[4].index);

                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[3].index);
                triangles.Add(particle.getEdgesNeighbors()[5].index);
                
                // 4
                triangles.Add(particle.index);
                triangles.Add(particle.getEdgesNeighbors()[4].index);
                triangles.Add(particle.getEdgesNeighbors()[5].index);

                edge6++;
            }
        }

        Debug.Log("Particles with 2 edge neighbors: " + edge3);
        Debug.Log("Particles with 3 edge neighbors: " + edge3);
        Debug.Log("Particles with 4 edge neighbors: " + edge4);
        Debug.Log("Particles with 5 edge neighbors: " + edge5);
        Debug.Log("Particles with 6 edge neighbors: " + edge6);
        Debug.Log("Total particles: " + particles.Count);
        Debug.Log("Edge 2, 3, 4, 5 particles: " + (edge2+edge3+edge4+edge5));

        return triangles.ToArray();
    }
}