using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class MidTerrainSetting
{
    public int divisions;
    public float xSize;
    public float zSize;
    public float maxHeight;
    public float minHeight;
    public float roughness = 2.0f;
    public float heightDampenerPower = 2.0f;
    //public Gradient gradient;
}

public class MidPointTerrain : MonoBehaviour
{
    public MidTerrainSetting settings;

    private Vector3[] mVerts;
    private int mVertCount;

    private void Start()
    {
        CreateMidTerrain();
    }

    void CreateMidTerrain()
    {
        int divisions = settings.divisions;
        float xSize = settings.xSize;
        float zSize = settings.zSize;
        float maxHeight = settings.maxHeight;
        float minHeight = settings.minHeight;
        //Gradient gradient = settings.gradient;
        float heightDampener = Mathf.Pow(settings.heightDampenerPower, -1 * settings.roughness);

        //vertex array
        mVertCount = (divisions + 1) * (divisions + 1);
        mVerts = new Vector3[mVertCount];
        int[] tris = new int[divisions * divisions * 2 * 3];
        Vector2[] uvs = new Vector2[mVertCount];
        //Color[] colors = new Color[mVertCount];
        float halfSize_x = xSize * 0.5f;
        float halfSize_z = zSize * 0.5f;
        float divisionSize_x = xSize / divisions;
        float divisionSize_z = zSize / divisions;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //build up triangles
        int triOffset = 0;

        for (int i = 0; i <= divisions; i++)
        {
            for (int j = 0; j <= divisions; j++)
            {
                //int index = i * (divisions + 1) + j;

                mVerts[i * (divisions + 1) + j] = new Vector3(-halfSize_x + j * divisionSize_x, 0.0f, halfSize_z - i * divisionSize_z);

                //mVerts[index].y = 0;
                //float normalizedHeight = Mathf.InverseLerp(heightMin, heightMax, mVerts[index].y);
                //colors[index] = gradient.Evaluate(normalizedHeight);
                uvs[i * (divisions + 1) + j] = new Vector2((float)i / divisions, (float)j / divisions);
                if (i < divisions && j < divisions)
                {
                    int topleft = i * (divisions + 1) + j;
                    int botleft = (i + 1) * (divisions + 1) + j;

                    //first triangle
                    tris[triOffset] = topleft;
                    tris[triOffset + 1] = topleft + 1;
                    tris[triOffset + 2] = botleft + 1;

                    //second triangle
                    tris[triOffset + 3] = topleft;
                    tris[triOffset + 4] = botleft + 1;
                    tris[triOffset + 5] = botleft;

                    triOffset += 6;
                }
            }
        }

        //set the 4 corners of the array to initial value
        mVerts[0].y = Random.Range(minHeight, maxHeight);
        mVerts[divisions].y = Random.Range(minHeight, maxHeight);
        mVerts[mVerts.Length - 1].y = Random.Range(minHeight, maxHeight);
        mVerts[mVerts.Length - 1 - divisions].y = Random.Range(minHeight, maxHeight);

        //Diamond-square
        int iterations = (int)Mathf.Log(divisions, 2);
        int numSquares = 1;
        int squareSize = divisions;

        for (int i = 0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {
                    //calling diamond square
                    DiamondSquare(row, col, squareSize, minHeight, maxHeight);

                    col += squareSize;

                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            minHeight *= heightDampener;
            maxHeight *= heightDampener;
        }

        /*
        for (int i = 0; i < mVertCount; i++)
        {
            float normalizedHeight = Mathf.InverseLerp(settings.heightMin, settings.heightMax, mVerts[i].y);
            colors[i] = gradient.Evaluate(normalizedHeight);
        }
        */


        //setup mesh
        mesh.vertices = mVerts;
        mesh.uv = uvs;
        //mesh.colors = colors;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        
        
    }

    void DiamondSquare(int row, int col, int size, float minHeight, float maxHeight)
    {

        int halfSize = (int)(size * 0.5f);
        int divisions = settings.divisions;

        //diamond step
        //set the midpoint of that square to be the average of the four corner points plus a random value.
        int topleft = row * (divisions + 1) + col;
        int botleft = (row + size) * (divisions + 1) + col;
        int mid = (int)(row + halfSize) * (divisions + 1) + (int)(col + halfSize);

        mVerts[mid].y = (mVerts[topleft].y + mVerts[topleft + size].y + mVerts[botleft].y + mVerts[botleft + size].y) * 0.25f + Random.Range(-minHeight, maxHeight);

        //square step
        //For each diamond in the array, set the midpoint of that diamond to be the average of the four corner points plus a random value.
        mVerts[topleft + halfSize].y = (mVerts[topleft].y + mVerts[topleft + size].y + mVerts[mid].y) / 3 + Random.Range(-minHeight, maxHeight);
        mVerts[mid - halfSize].y = (mVerts[topleft].y + mVerts[botleft].y + mVerts[mid].y) / 3 + Random.Range(-minHeight, maxHeight);
        mVerts[mid + halfSize].y = (mVerts[topleft + size].y + mVerts[botleft + size].y + mVerts[mid].y) / 3 + Random.Range(-minHeight, maxHeight);
        mVerts[botleft + halfSize].y = (mVerts[botleft].y + mVerts[botleft + size].y + mVerts[mid].y) / 3 + Random.Range(-minHeight, maxHeight);

    }


}
