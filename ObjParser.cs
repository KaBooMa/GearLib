using UnityEngine;
using System.Collections.Generic;
using System.IO;
using GearLib;

public class ObjParser
{
    public static Mesh ParseObj(string path)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] parts = line.Trim().Split(' ');

                switch (parts[0])
                {
                    case "v": // Vertex position
                        Vector3 vertex = ParseVector3(parts);
                        vertices.Add(vertex);
                        break;
                    case "vn": // Vertex normal
                        Vector3 normal = ParseVector3(parts);
                        normals.Add(normal);
                        break;
                    case "vt": // Texture coordinate (UV)
                        Vector2 uv = ParseVector2(parts);
                        uvs.Add(uv);
                        break;
                    case "f": // Face
                        for (int i = 1; i < parts.Length; i++)
                        {
                            string[] faceIndices = parts[i].Split('/');
                            int vertexIndex = int.Parse(faceIndices[0]) - 1;

                            triangles.Add(vertexIndex);
                        }
                        break;
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals(); // Recalculate normals based on the vertices and triangles
            return mesh;
        }
        else
        {
            Plugin.Log.LogError($"Failed to load OBJ {path}!");
            return null;
        }
    }

    private static Vector3 ParseVector3(string[] parts)
    {
        float x = float.Parse(parts[1]);
        float y = float.Parse(parts[2]);
        float z = float.Parse(parts[3]);
        return new Vector3(x, y, z);
    }

    private static Vector2 ParseVector2(string[] parts)
    {
        float x = float.Parse(parts[1]);
        float y = float.Parse(parts[2]);
        return new Vector2(x, y);
    }
}