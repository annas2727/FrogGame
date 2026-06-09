using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Breeding : MonoBehaviour
{
    
    GameObject FrogL;
    GameObject FrogR;

    public GameObject NewFrogPrefab; 

    public enum FrogColor { red, orange, yellow, green, blue, purple, brown, white, black, grey }

    public bool breedTogether = false;



    public void ConnectFrog(GameObject Frog, bool isL)
    {
        if(isL)
            FrogL = Frog;
        else
            FrogR = Frog;
        if (FrogL != null && FrogR != null)
        {
            breedTogether = true;
            Debug.Log("Here is where they breed");

            //Reset
            FrogL = null;
            FrogR = null;
        }
    }

    private void DNAMix()
    {
        FrogLife FrogSkinL = FrogL.GetComponent<FrogLife>();
        FrogLife FrogSkinR = FrogR.GetComponent<FrogLife>();
    
        string bodyColor = OffspringColor(FrogSkinL.bodyColorName, FrogSkinL.bodyColorName);
        string patternColor = OffspringColor(FrogSkinR.bodyColorName, FrogSkinR.bodyColorName);

        

    }

    private string OffspringColor(string FrogAColor, string FrogBColor)

    {
        int roll = UnityEngine.Random.Range(0, 100); // 0-99

        if (roll < 1) return FrogColor.white.ToString(); // 1% White
        else if (roll < 25) return FrogAColor; // 24% ColorA
        else if (roll < 75)
        {    // 50% Mix
            Enum.TryParse<FrogColor>(FrogAColor, true, out FrogColor FrogColorA);
            Enum.TryParse<FrogColor>(FrogBColor, true, out FrogColor FrogColorB);
            string Mixed = Mix(FrogColorA, FrogColorB);
            if (Mixed == "5050")
            {
                roll = UnityEngine.Random.Range(0, 100);
                if (roll < 50) return FrogAColor;
                else return FrogBColor;
            }
            else return Mixed;
        }
        else if (roll < 99) return FrogBColor; //24% ColorB
        else return FrogColor.black.ToString(); // 1% Black
    }

    private Dictionary<(FrogColor, FrogColor), FrogColor> Recipes = new()
    {
        // Primary -> Secondary
        { (FrogColor.red, FrogColor.yellow), FrogColor.orange },
        { (FrogColor.red, FrogColor.blue), FrogColor.purple },
        { (FrogColor.yellow, FrogColor.blue), FrogColor.green },
    };

    public string Mix(FrogColor a, FrogColor b)
    {
        // Same color
        if (a == b)
            return a.ToString();

        // Sort so I don't have to define all this shit twice
        if ((int)a > (int)b)
            (a, b) = (b, a);

        // The Rare Grey Frog
        if (a == FrogColor.white && b == FrogColor.black)
            return FrogColor.grey.ToString();

        //Grey rules
        if (a == FrogColor.white && b == FrogColor.grey)
            return FrogColor.white.ToString();
        if (a == FrogColor.black && b == FrogColor.grey)
            return FrogColor.black.ToString();

        // White or Black are recessive
        if (b == FrogColor.white || b == FrogColor.black || b == FrogColor.grey)
            return a.ToString();

        // Brown contamination
        if (a == FrogColor.brown || b == FrogColor.brown)
            return FrogColor.brown.ToString();

        // Primary combinations
        if (Recipes.TryGetValue((a, b), out FrogColor result))
            return result.ToString();

        // Adjacent on color wheel = 50/50
        if (IsAdjacent(a, b))
            return "5050";

        // Everything else = Brown
        return FrogColor.brown.ToString();
    }

    private static bool IsAdjacent(FrogColor a, FrogColor b)
    {
        return
            (a == FrogColor.red && b == FrogColor.orange) ||
            (a == FrogColor.orange && b == FrogColor.yellow) ||
            (a == FrogColor.yellow && b == FrogColor.green) ||
            (a == FrogColor.green && b == FrogColor.blue) ||
            (a == FrogColor.blue && b == FrogColor.purple) ||
            (a == FrogColor.purple && b == FrogColor.red);
    }
}
