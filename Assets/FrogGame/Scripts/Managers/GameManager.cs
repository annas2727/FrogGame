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
        { "purple", "#6f00ff" }, 
        { "brown", "#93430a" },
        { "white", "#ffffff" },
        { "black", "#000000" }
    };

    public Dictionary<string, string> tadpoleBodyColors = new Dictionary<string, string>
    {
        { "red", "#ff7272" },
        { "orange", "#ffb13b" },
        { "yellow", "#ffea80" },
        { "green", "#93ff93" },
        { "blue", "#7693fc" },
        { "purple", "#c395ff" }, 
        { "brown", "#ffb27a" },
        { "white", "#ffffff" },
        { "black", "#8d8d8d" }
    };

    public Dictionary<string, string> patternColors = new Dictionary<string, string>
    {
        { "red", "#b50000" },
        { "orange", "#b86f00" },
        { "yellow", "#b99b02" },
        { "green", "#00d000" },
        { "blue", "#0000cc" },
        { "purple", "#5900ce" },
        { "brown", "#5a2907" },
        { "white", "#ffffff" },
        { "black", "#000000" }
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
