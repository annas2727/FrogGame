using UnityEngine;
using System.Collections;

public class FrogPadManager : MonoBehaviour
{
    public GameObject frogPadSquare; 
    public Transform frogsParent;
    public float spacingX = 2.5f;
    public float spacingZ = 2.5f;
    public Transform pad;
    public int columns = 3; 
    public float zOffset = -0.4f;
    public float squareScale = 0.19f;

    void Start()
    {
        StartCoroutine(GenerateGridDelayed());
    }

    IEnumerator GenerateGridDelayed()
    {
        yield return null;
        GenerateGrid();
    }

    void GenerateGrid()
    {
        FrogFrogPad[] frogs = frogsParent.GetComponentsInChildren<FrogFrogPad>();
        int rows = Mathf.CeilToInt((float)frogs.Length / columns);

        float totalWidth = (Mathf.Min(frogs.Length, columns) - 1) * spacingX;
        float totalDepth = (rows - 1) * spacingZ;
        Vector3 offset = new Vector3(-totalWidth / 2f, 0, totalDepth / 2f);

        for (int i = 0; i < frogs.Length; i++)
        {
            int col = i % columns;
            int row = i / columns;

            GameObject square = Instantiate(frogPadSquare, transform);
            square.transform.localScale = Vector3.one * squareScale;
            square.transform.position = new Vector3(
                pad.position.x + col * spacingX,
                pad.position.y + 0.1f,
                pad.position.z + (-row * spacingZ + zOffset)
            ) + offset;

            Renderer r = square.GetComponent<Renderer>();
            Material mat = new Material(r.material);
            mat.SetTexture("_BaseMap", frogs[i].renderTexture);
            r.material = mat;
        }
    }
}