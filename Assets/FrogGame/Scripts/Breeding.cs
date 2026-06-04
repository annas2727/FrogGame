
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Breeding : MonoBehaviour
{
    GameObject Frog1; 
    GameObject Frog2;
    public enum FrogColor { red, orange, yellow, green, blue, purple, brown, white, black, grey }

    private Dictionary<(FrogColor, FrogColor), FrogColor> Recipes = new()
    {
        // Primary -> Secondary
        { (FrogColor.red, FrogColor.yellow), Col.orange },
        { (FrogColor.red, FrogColor.blue), Col.blue },
        { (FrogColor.yellow, FrogColor.blue), Col.green },
    };

    public string Mix(Colour a, Colour b)
    {
        // Same color
        if (a == b)
            return a.ToString();

        // Sort so I don't have to define all this shit twice
        if ((int)a > (int)b)
            (a, b) = (b, a);

        // The Rare Grey Frog
        if (a == FrogColor.white && b == FrogColor.black)
            return b.ToString();

        // White or Black are recessive
        if (a == FrogColor.white || a == FrogColor.black)
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

    private static bool IsAdjacent(Col a, Col b)
    {
        return
            (a == FrogColor.Red && b == FrogColor.Ora) ||
            (a == FrogColor.Ora && b == FrogColor.Yel) ||
            (a == FrogColor.Yel && b == FrogColor.Gre) ||
            (a == FrogColor.Gre && b == FrogColor.Blu) ||
            (a == FrogColor.Blu && b == FrogColor.Pur) ||
            (a == FrogColor.Pur && b == FrogColor.Red);
    }
    
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
}
