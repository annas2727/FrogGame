using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
public class Breeding : MonoBehaviour
{
    GameObject Frog1; 
    GameObject Frog2;
    string bodyColor; 
    string patternColor;
    string patternType;

    public GameObject egg;

        public Dictionary<string, string> colorID = new Dictionary<string, string>
        {
            { "red", "primary" },
            { "orange", "secondary" },
            { "yellow", "primary" },
            { "green", "secondary" },
            { "blue", "primary" },
            { "purple", "secondary" }, 
            { "brown", "other" },
            { "white", "other" },
            { "black", "other" }
        };

    public void Breed()
    {
        bodyColor1 = Frog1.GetComponent<FrogProperties>().bodyColor;
        bodyColor2 = Frog2.GetComponent<FrogProperties>().bodyColor;
        patternColor1 = Frog1.GetComponent<FrogProperties>().patternColor;
        patternColor2 = Frog2.GetComponent<FrogProperties>().patternColor;
        patternType1 = Frog1.GetComponent<FrogProperties>().patternType;
        patternType2 = Frog2.GetComponent<FrogProperties>().patternType;

        GameObject NewFrog = Instantiate(Frog1, new Vector3(0, 0, 0), Quaternion.identity);
        NewFrog.GetComponent<FrogProperties>().DNA(bodyColor1, bodyColor2, patternColor1, patternColor2, patternType1, patternType2);
    }
    
    string ColorMix(string c1, string c2)
    {
        //Case 1: Same color x same color
        if (c1 == c2) return c1;

        //Case 2: Primary color x Primary color
        if (colors.Contains("red") && colors.Contains("yellow")) return "orange";
        else if (colors.Contains("red") && colors.Contains("blue")) return "purple";
        else if (colors.Contains("yellow") && colors.Contains("blue")) return "green";
        else if (colors.Contains("red") && colors.Contains("green")) return "brown";
        else if (colors.Contains("yellow") && colors.Contains("purple")) return "brown";
        else if (colors.Contains("blue") && colors.Contains("orange")) return "brown";
        
        //basic secondary color mixing
        else if (colors.Contains("orange") && colors.Contains("green")) return "brown";
        else if (colors.Contains("orange") && colors.Contains("purple")) return "brown";
        else if (colors.Contains("green") && colors.Contains("purple")) return "brown";

        else return "random"; 
    }

    void DNA(string bc1, string bc2, string pc1, string pc2, string pt1, string pt2)
    {
        random = Random.Range(0, 99);

        if (random < 1) bodyColor = "white";
        else if (random < 2) bodyColor = "black";
        else if (random < 17) bodyColor = bc1;
        else if (random < 33) bodyColor = bc2;
    }
    
}
*/