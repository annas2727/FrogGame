using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, string> colors = new Dictionary<string, string>
    {
        { "red", "#ff0000" },
        { "orange", "#ff9900" },
        { "yellow", "#ffd500" },
        { "green", "#00ff00" },
        { "blue", "#0000ff" },
        { "purple", "#6f00ff" }
    };

    public List<string> patternTypes = new List<string>
    {
        "spots",
        "stripes",
        "none"
    };

    public List<Material> patternMaterials;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
