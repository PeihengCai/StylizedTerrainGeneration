using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public Vector2[] UVs;
    public Mesh thisMesh = new Mesh();

    public MeshData(List<Vector3> v, List<int> t)
    {
        /*switch (algorithm)
        {
            case "FFT":
                vertices = new Vector3[w * h];
                Vector3 lefttop1 = new Vector3(currPos.x - (float)w / 2, currPos.y, currPos.z+ (float)h / 2);
                for (int i = 0; i < h; i++) // height
                {
                    for (int j = 0; j < w; j++) // width
                    {
                        // index = w * (i + 1) + j
                        // eg. vertice[2][1]: 3*4, i = 2, j = 1, index = 1 + 3*2 = 7
                        float x = lefttop1.x + (float)w / step;
                        float z = lefttop1.z + (float)h / step;
                        vertices[j + w*i] = 
                    }
                }
                break; 

            case "MidPoint":
                Vector3 lefttop = new Vector3(currPos.x - (float)w / 2, currPos.y, currPos.z + (float)h / 2);
                Vector3 righttop = new Vector3(currPos.x + (float)w / 2, currPos.y, currPos.z + (float)h / 2);
                Vector3 leftbottom = new Vector3(currPos.x - (float)w / 2, currPos.y, currPos.z - (float)h / 2);
                Vector3 rightbottom = new Vector3(currPos.x + (float)w / 2, currPos.y, currPos.z - (float)h / 2);
                vertices = new Vector3[4] {lefttop, righttop, leftbottom, rightbottom};
                break;
        }
        */
        vertices = v;
        triangles = t;
        thisMesh.vertices = vertices.ToArray();
        thisMesh.triangles = triangles.ToArray();
    }

    public void SetTriangle(int a, int b, int c)
    {
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);
    }
}

public class MeshGenerate : MonoBehaviour
{
    public MeshData meshData;
    public MeshFilter terrainMesh;
    public MeshCollider meshCollider;


    // Start is called before the first frame update
    void Start()
    {
        //CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMesh(string algorithm, int w, int h, Vector3 currPos, int step)
    {
        /*List<Vector3> vertices = new List<Vector3> {
            new Vector3(0, 0, 0), 
            new Vector3(0, 0, 1), 
            new Vector3(1, 0, 0), 
            new Vector3(1, 0, 1)
        };
        List<int> triangles = new List<int> {
            0, 1, 2, // 第一个三角形
            2, 1, 3  // 第二个三角形
        };*/
        switch (algorithm)
        {
            case "FFT":
                List<Vector3> vertices = new List<Vector3>();
                Vector3 lefttop1 = new Vector3(currPos.x - (float)w / 2, currPos.y, currPos.z+ (float)h / 2);
                for (int i = 0; i < h; i++) // height
                {
                    for (int j = 0; j < w; j++) // width
                    {
                        // index = w * (i + 1) + j
                        // eg. vertice[2][1]: 3*4, i = 2, j = 1, index = 1 + 3*2 = 7
                        float x = lefttop1.x + (float)w / step;
                        float z = lefttop1.z + (float)h / step;
                        vertices.Add(new Vector3(x, 0,z));
                    }
                }
                break; 

            case "MidPoint":
                Vector3 lefttop = new Vector3(currPos.x - (float)w / 2, currPos.y, currPos.z + (float)h / 2);
                Vector3 righttop = new Vector3(currPos.x + (float)w / 2, currPos.y, currPos.z + (float)h / 2);
                Vector3 leftbottom = new Vector3(currPos.x - (float)w / 2, currPos.y, currPos.z - (float)h / 2);
                Vector3 rightbottom = new Vector3(currPos.x + (float)w / 2, currPos.y, currPos.z - (float)h / 2);
                //vertices = new Vector3[4] {lefttop, righttop, leftbottom, rightbottom};
                break;
        }
        
        //meshData = new MeshData(vertices, triangles);

        terrainMesh.mesh = meshData.thisMesh;
        meshCollider.sharedMesh = meshData.thisMesh;
    }
}
