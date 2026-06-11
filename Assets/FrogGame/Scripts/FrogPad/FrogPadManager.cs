using UnityEngine;
using System.Collections;

public class FrogPadManager : MonoBehaviour
{
    public GameObject frogPadSquare; 
    public Transform frogsParent;
    public float spacingX = 0.5f;
    public float spacingZ = 0.5f;
    public Transform pad;

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
        int rows = Mathf.CeilToInt((float)frogs.Length / columns);

        float totalWidth = (Mathf.Min(frogs.Length, columns) - 1) * spacingX;
        float totalDepth = (rows - 1) * spacingZ;
        Vector3 offset = new Vector3(-totalWidth / 2f, 0, totalDepth / 2f);

        for (int i = 0; i < frogs.Length; i++)
        {
            int col = i % columns;
            int row = i / columns;

            Vector3 position = pad.position + new Vector3(col * spacingX, 1, -row * spacingZ) + offset;
            GameObject square = Instantiate(frogPadSquare, position, Quaternion.identity, transform);

            Renderer r = square.GetComponent<Renderer>();
            Material mat = new Material(r.material);
            mat.SetTexture("_BaseMap", frogs[i].renderTexture);
            r.material = mat;
        }
    }
}