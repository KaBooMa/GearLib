using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace GearLib.Utils
{
    public class ObjParser
    {
        public static Mesh ParseObj(string[] data)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();

            List<Vector3> meshVertices = new List<Vector3>();
            List<Vector3> meshNormals = new List<Vector3>();
            List<Vector2> meshUVs = new List<Vector2>();

            Dictionary<string, int> vertexIndexMap = new Dictionary<string, int>();

            foreach (string line in data)
            {
                string[] parts = line.Trim().Split(' ');

                switch (parts[0])
                {
                    case "v":
                        vertices.Add(ParseVector3(parts));
                        break;
                    case "vn":
                        normals.Add(ParseVector3(parts));
                        break;
                    case "vt":
                        uvs.Add(ParseVector2(parts));
                        break;
                    case "f":
                        ParseFace(parts, vertices, normals, uvs, meshVertices, meshNormals, meshUVs, triangles, vertexIndexMap);
                        break;
                }
            }

            Mesh mesh = new Mesh
            {
                vertices = meshVertices.ToArray(),
                triangles = triangles.ToArray()
            };

            if (meshNormals.Count > 0)
            {
                mesh.normals = meshNormals.ToArray();
            }
            else
            {
                mesh.RecalculateNormals();
            }

            if (meshUVs.Count > 0)
            {
                mesh.uv = meshUVs.ToArray();
            }

            mesh.RecalculateBounds();
            return mesh;
        }

        private static void ParseFace(string[] parts, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uvs,
                                      List<Vector3> meshVertices, List<Vector3> meshNormals, List<Vector2> meshUVs,
                                      List<int> triangles, Dictionary<string, int> vertexIndexMap)
        {
            for (int i = 1; i < parts.Length; i++)
            {
                string key = parts[i];
                if (!vertexIndexMap.TryGetValue(key, out int index))
                {
                    string[] indices = key.Split('/');

                    int vertIndex = ParseIndex(indices[0], vertices.Count);
                    Vector3 vertex = vertices[vertIndex];

                    Vector3 normal = Vector3.zero;
                    if (indices.Length > 2 && !string.IsNullOrEmpty(indices[2]))
                    {
                        int normIndex = ParseIndex(indices[2], normals.Count);
                        normal = normals[normIndex];
                    }

                    Vector2 uv = Vector2.zero;
                    if (indices.Length > 1 && !string.IsNullOrEmpty(indices[1]))
                    {
                        int uvIndex = ParseIndex(indices[1], uvs.Count);
                        uv = uvs[uvIndex];
                    }

                    index = meshVertices.Count;
                    meshVertices.Add(vertex);
                    if (normals.Count > 0) meshNormals.Add(normal);
                    if (uvs.Count > 0) meshUVs.Add(uv);

                    vertexIndexMap[key] = index;
                }

                triangles.Add(index);
            }
        }

        private static int ParseIndex(string index, int count)
        {
            int result = int.Parse(index);
            if (result < 0)
            {
                result = count + result;
            }
            else
            {
                result = result - 1;
            }
            return result;
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
}