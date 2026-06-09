
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Breeding : MonoBehaviour
{
    GameObject FrogL;
    GameObject FrogR;
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



    private OffspringColor(FrogColor a, FrogColor b)
    {
        int roll = Random.Range(0, 100); // 0-99

        if (roll < 1)          // 1% White
            return 1;
        else if (roll < 25)    // 24% Color1
            return 2;
        else if (roll < 75)    // 50% Color2
            return 3;
        else if (roll < 99)    // 24% Color3
            return 4;
        else                   // 1% Color4
            return 5;
    }

    private Dictionary<(FrogColor, FrogColor), FrogColor> Recipes = new()
    {
        // Primary -> Secondary
        { (FrogColor.red, FrogColor.yellow), FrogColor.orange },
        { (FrogColor.red, FrogColor.blue), FrogColor.purple },
        { (FrogColor.yellow, FrogColor.blue), FrogColor.green },
    };

    public FrogColor Mix(FrogColor a, FrogColor b)
    {
        // Same color
        if (a == b)
            return a;

        // Sort so I don't have to define all this shit twice
        if ((int)a > (int)b)
            (a, b) = (b, a);

        // The Rare Grey Frog
        if (a == FrogColor.white && b == FrogColor.black)
            return FrogColor.grey;

        //Grey rules
        if (a == FrogColor.white && b == FrogColor.grey)
            return FrogColor.white;
        if (a == FrogColor.black && b == FrogColor.grey)
            return FrogColor.black;

        // White or Black are recessive
        if (b == FrogColor.white || b == FrogColor.black || b == FrogColor.grey)
            return a;

        // Brown contamination
        if (a == FrogColor.brown || b == FrogColor.brown)
            return FrogColor.brown;

        // Primary combinations
        if (Recipes.TryGetValue((a, b), out FrogColor result))
            return result;

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
