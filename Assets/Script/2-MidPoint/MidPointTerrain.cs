﻿using UnityEngine;

public class MidPointTerrain : MonoBehaviour
{
    private Terrain terrainComponent;
    [Header("Settings")]
    [Range(0.5f, 1.5f)] public float roughness;
    public float minHeight;
    public float maxHeight;
    public bool enableNormalization;

    private int resolution;
    private float[,] heightMap;

    private void Start()
    {
        terrainComponent = Terrain.activeTerrain;
        resolution = terrainComponent.terrainData.heightmapResolution;

        GenerateHeightmap();
    }

    public void GenerateNewTerrain()
    {
        terrainComponent = Terrain.activeTerrain;
        resolution = terrainComponent.terrainData.heightmapResolution;
        GenerateHeightmap();
    }

    private void GenerateHeightmap()
    {
        heightMap = new float[resolution, resolution];
        InitializeCorners();
        PerformDiamondSquare();
        FourSlidesPosition();
        if (enableNormalization == true)
        {
            Debug.Log("Enable Normalization");
            NormalizeHeightmap();
        }
        terrainComponent.terrainData.SetHeights(0, 0, heightMap);
    }

    private void InitializeCorners()
    {
        //4 corners, add random value
        heightMap[0, 0] = Random.Range(minHeight, maxHeight);
        heightMap[0, resolution - 1] = Random.Range(minHeight, maxHeight);
        heightMap[resolution - 1, 0] = Random.Range(minHeight, maxHeight);
        heightMap[resolution - 1, resolution - 1] = Random.Range(minHeight, maxHeight);
    }

    /// <summary>
    /// diamond-Square
    /// </summary>
    private void PerformDiamondSquare()
    {
        int stepSize = resolution - 1;//offset
        float heightRange = maxHeight - minHeight;

        while (stepSize > 1)
        {
            int halfStep = stepSize / 2;

            // Diamond step
            //set the midpoint of that square to be the average of the four corner points plus a random value.
            for (int x = halfStep; x < resolution; x += stepSize)
            {
                for (int y = halfStep; y < resolution; y += stepSize)
                {
                    heightMap[x, y] = AverageCorners(x, y, halfStep) + Random.Range(-heightRange, heightRange);
                }
            }

            // Square step
            //For each diamond in the array, set the midpoint of that diamond to be the average of the four corner points plus a random value.
            for (int x = 0; x < resolution; x += halfStep)
            {
                for (int y = (x + halfStep) % stepSize; y < resolution; y += stepSize)
                {
                    heightMap[x, y] = AverageEdges(x, y, halfStep) + Random.Range(-heightRange, heightRange);
                }
            }
            stepSize /= 2;
            heightRange *= Mathf.Pow(2, -roughness);
        }
    }

    /// <summary>
    /// average the heights of the 4 corners
    /// </summary>
    /// 
    private float AverageCorners(int x, int y, int halfStep)
    {
        return (
            heightMap[x - halfStep, y - halfStep] +
            heightMap[x + halfStep, y - halfStep] +
            heightMap[x - halfStep, y + halfStep] +
            heightMap[x + halfStep, y + halfStep]
        ) * 0.25f;
    }

    /// <summary>
    /// average the heights of the edges
    /// </summary>
    private float AverageEdges(int x, int y, int halfStep)
    {
        float total = 0.0f;
        int count = 0;

        if (x - halfStep >= 0) { total += heightMap[x - halfStep, y]; count++; }
        if (x + halfStep < resolution) { total += heightMap[x + halfStep, y]; count++; }
        if (y - halfStep >= 0) { total += heightMap[x, y - halfStep]; count++; }
        if (y + halfStep < resolution) { total += heightMap[x, y + halfStep]; count++; }

        return total / count;
    }

    /// <summary>
    /// normalize the heightmap values
    /// </summary>
    private void NormalizeHeightmap()
    {
        Debug.Log("Enable");
        float min = float.MaxValue, max = float.MinValue;

        foreach (float value in heightMap)
        {
            if (value < min) min = value;
            if (value > max) max = value;
        }

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                heightMap[x, y] = (heightMap[x, y] - min) / (max - min);
            }
        }
    }

    private void FourSlidesPosition()
    {
        // set 4 slides to the lowest point
        for (int i = 0; i < resolution; i++)
        {
            heightMap[0, i] = 0.0f;
            heightMap[resolution - 1, i] = 0.0f;
            heightMap[i, 0] = 0.0f;
            heightMap[i, resolution - 1] = 0.0f;
        }
    }
}
