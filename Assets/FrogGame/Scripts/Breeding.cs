using UnityEngine;

public class Breeding : MonoBehaviour
{
    GameObject Frog1; 
    GameObject Frog2;
    string bodyColor; 
    string patternColor;
    string patternType;

    public GameObject egg;

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
    
    void DNA(string bc1, string bc2, string pc1, string pc2, string pt1, string pt2)
    {
        
    }
}
