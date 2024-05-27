using UnityEngine;
using System.Collections.Generic;
using System.IO;
using GearLib;
using System.Linq;
using System;
using System.Globalization;

namespace GearLib.Utils;

public class ObjParser
{
    public static Mesh ParseObj(string[] data)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<string> faces = new List<string>();


        foreach (string line in data)
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
                        faces.Add(parts[i]);
                    }
                    break;
            }
        }

        // Rebuilding obj for Unity
        List<Vector3> unity_vertices = new List<Vector3>();
        List<Vector3> unity_normals = new List<Vector3>();
        List<Vector2> unity_uvs = new List<Vector2>();
        List<int> unity_faces = new List<int>();
        foreach (string face in faces)
        {
            string[] face_data = face.Replace("f ", "").Split(' ');
            foreach (string d in face_data)
            {
                string[] ds = face.Split('/');
                Vector3 unity_vertex = vertices[Convert.ToInt32(ds[0])-1];
                unity_vertices.Add(unity_vertex);

                Vector3 unity_normal = normals[Convert.ToInt32(ds[2])-1];
                unity_normals.Add(unity_normal);

                if (ds[1] != "")
                {
                    Vector2 unity_uv = uvs[Convert.ToInt32(ds[1])-1];
                    unity_uvs.Add(unity_uv);
                }

                unity_faces.Add(unity_vertices.Count-1);
            }
        }

        Mesh mesh = new Mesh
        {
            vertices = unity_vertices.ToArray(),
            normals = unity_normals.ToArray(),
            uv = unity_uvs.ToArray(),
            triangles = unity_faces.ToArray()
        };
        mesh.RecalculateNormals(); // Recalculate normals based on the vertices and triangles
        return mesh;
    }

    private static Vector3 ParseVector3(string[] parts)
    {
        float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
        float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
        float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
        return new Vector3(x, y, z);
    }

    private static Vector2 ParseVector2(string[] parts)
    {
        float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
        float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
        return new Vector2(x, y);
    }
}