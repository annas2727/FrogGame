using UnityEngine;
using System.Collections;

public class FrogPadManager : MonoBehaviour
{
    public GameObject frogPadSquare; 
    public Transform frogsParent;
    public float spacingX = 0.5f;
    public float spacingZ = 0.5f;

    void Start()
    {
        StartCoroutine(GenerateGridDelayed());
    }

    IEnumerator GenerateGridDelayed()
    {
        yield return null; // wait one frame for frogs to initialize

        GenerateGrid();
    }

    void GenerateGrid()
    {
        int columns = 4;
        FrogFrogPad[] frogs = frogsParent.GetComponentsInChildren<FrogFrogPad>();

        for (int i = 0; i < frogs.Length; i++)
        {
            int col = i % columns;
            int row = i / columns;

            Vector3 position = new Vector3(col * spacingX, 0, -row * spacingZ);
            GameObject square = Instantiate(frogPadSquare, position, Quaternion.identity, transform);

            Renderer r = square.GetComponent<Renderer>();
            Material mat = new Material(r.material);
            mat.SetTexture("_BaseMap", frogs[i].renderTexture); // URP uses _BaseMap
            r.material = mat;
        }
    }
}