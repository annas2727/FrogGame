using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Dictionary<string, string> bodyColors = new Dictionary<string, string>
    {
        { "red", "#ff0000" },
        { "orange", "#ff9900" },
        { "yellow", "#ffd500" },
        { "green", "#00ff00" },
        { "blue", "#0000ff" },
        { "purple", "#6f00ff" }
    };

    public Dictionary<string, string> patternColors = new Dictionary<string, string>
    {
        { "red", "#b50000" },
        { "orange", "#b86f00" },
        { "yellow", "#b99b02" },
        { "green", "#00d000" },
        { "blue", "#0000cc" },
        { "purple", "#5900ce" }
    };

    public List<string> patternTypes = new List<string>
    {
        "spots",
        "stripes",
        "none"
    };

    public List<Material> patternMaterials;
    public Material eyeMaterial;

    public Material GetPatternMaterial(string patternType)
    {
        int index = patternType switch
        {
            "none"   => 0,
            "spots"   => 1,
            "stripes" => 2,
            _         => 3
        };

        return patternMaterials.Count > index ? patternMaterials[index] : null;
    }

}
