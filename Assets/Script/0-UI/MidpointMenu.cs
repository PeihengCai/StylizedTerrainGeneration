using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MidpointMenu : MonoBehaviour
{
    public MainMenu menu;

    [Header("Terrain Basic Setting")]
    public TMP_InputField widthInput;
    public TMP_InputField lengthInput;
    public TMP_InputField heightInput;

    [Header("MidPoint Settings")]
    public float minHeight = 0.2f;
    public float maxHeight = 0.8f;
    public Slider minHeightSlider;
    public Slider maxHeightSlider;
    public TMP_Text minHeightText;
    public TMP_Text maxHeightText;
    public Toggle normalizationToggle;
    public Slider roughnessSlider;
    public TMP_Text roughnessText;


    // Start is called before the first frame update
    void Start()
    {
        widthInput.onEndEdit.AddListener(UpdateWidth);
        lengthInput.onEndEdit.AddListener(UpdateLength);
        heightInput.onEndEdit.AddListener(UpdateHeight);

        normalizationToggle.onValueChanged.AddListener(OnNormalizationToggleChanged);
        roughnessSlider.onValueChanged.AddListener(OnRoughnessChanged);

        maxHeightSlider.onValueChanged.AddListener(OnMaxHeightChanged);
        minHeightSlider.onValueChanged.AddListener(OnMinHeightChanged);

        maxHeightSlider.value = maxHeight;
        minHeightSlider.value = minHeight;
    }

    void OnEnable()
    {
        //basic terrain settings
        //widthInput.text = widthInput.value.ToString("F0");
        //lengthInput.text = terrainData.size.z.ToString("F0");
        //heightInput.text = terrainData.size.y.ToString("F0");

        //mid point settings
        //normalizationToggle.isOn = midpointTerrain.enableNormalization;
        //roughnessSlider.value = midpointTerrain.roughness;
        //roughnessText.text = $"{midpointTerrain.roughness:F2}";

        //initial midpointimprove
        //midpointTerrain.minHeight = minHeight;
        //midpointTerrain.maxHeight = maxHeight;
        //maxHeightSlider.value = maxHeight;
        //minHeightSlider.value = minHeight;
        menu.variables.width = 128;
        menu.variables.length = 128;
        menu.variables.height = 10;
        menu.variables.minHeight = minHeightSlider.value;
        menu.variables.maxHeight = maxHeightSlider.value;
        menu.variables.roughness = roughnessSlider.value;
        menu.variables.enableNormalization = normalizationToggle.isOn;
    }

    public void UpdateWidth(string value)
    {
        menu.variables.width = int.Parse(value);
    }

    public void UpdateLength(string value)
    {
        menu.variables.length = int.Parse(value);
    }

    public void UpdateHeight(string value)
    {
        menu.variables.midheight = int.Parse(value);
    }

    ///<summary>
    /// Mid point settings
    /// </summary>
    /// 
    public void OnNormalizationToggleChanged(bool isActive)
    {
        menu.variables.enableNormalization = isActive;
    }


    public void OnMinHeightChanged(float value)
    {
        minHeight = value;
        minHeightText.text = $"{minHeight:F2}";
        menu.variables.minHeight = minHeight;
    }
    public void OnMaxHeightChanged(float value)
    {
        maxHeight = value;
        maxHeightText.text = $"{maxHeight:F2}";
        menu.variables.maxHeight = maxHeight;
        //midpointImprove.GenerateNewTerrain();
    }

    public void OnRoughnessChanged(float value)
    {
        menu.variables.roughness = value;
        roughnessText.text = $"{value:F2}";
    }


}
