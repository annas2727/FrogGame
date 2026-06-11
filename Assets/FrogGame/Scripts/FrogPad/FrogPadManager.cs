using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class FrogPadManager : MonoBehaviour
{
    public GameObject frogPadSquare; 
    public Transform frogsParent;
    public float spacingX = 2.5f;
    public float spacingZ = 2.5f;
    public Transform pad;
    public int columns = 3; 
    public float zOffset = -1f;
    public float squareScale = 0.19f;

    public float scrollSpeed = 2f;
    public CameraMovement cameraMovement;

    private float scrollOffset = 0f;
    private List<GameObject> squares = new List<GameObject>();
    private Vector3 offset;
    private float padHalfHeight;

    void Start()
    {
        StartCoroutine(GenerateGridDelayed());
    }

    IEnumerator GenerateGridDelayed()
    {
        yield return null;
        GenerateGrid();
    }

    void Update()
    {
        if (cameraMovement.frogPadOpen)
        {
            Mouse mouse = Mouse.current;
            if (mouse == null) return;

            float scroll = mouse.scroll.ReadValue().y;
            if (scroll != 0)
            {
                scrollOffset += scroll * scrollSpeed * Time.deltaTime;
                UpdateSquarePositions();
                Debug.Log("Scrolling");
            }
        }
    }

    void GenerateGrid()
    {
        FrogFrogPad[] frogs = frogsParent.GetComponentsInChildren<FrogFrogPad>();
        int rows = Mathf.CeilToInt((float)frogs.Length / columns);

        float totalWidth = (Mathf.Min(frogs.Length, columns) - 1) * spacingX;
        float totalDepth = (rows - 1) * spacingZ;
        offset = new Vector3(-totalWidth / 2f, 0, totalDepth / 2f);

        padHalfHeight = pad.localScale.z * 0.5f;

        for (int i = 0; i < frogs.Length; i++)
        {
            int col = i % columns;
            int row = i / columns;

            GameObject square = Instantiate(frogPadSquare, transform);
            square.transform.localScale = Vector3.one * squareScale;
            squares.Add(square);
            square.layer = LayerMask.NameToLayer("FrogPad");

            Renderer r = square.GetComponent<Renderer>();
            Material mat = new Material(r.material);
            mat.SetTexture("_BaseMap", frogs[i].renderTexture);
            r.material = mat;
        }

        UpdateSquarePositions();
    }

    void UpdateSquarePositions()
    {
        for (int i = 0; i < squares.Count; i++)
        {
            int col = i % columns;
            int row = i / columns;

            float zPos = pad.position.z + (-row * spacingZ + zOffset + scrollOffset);

            squares[i].transform.position = new Vector3(
                pad.position.x + col * spacingX,
                pad.position.y + 0.1f,
                zPos
            ) + offset;

            bool inBounds = zPos > pad.position.z - padHalfHeight && 
                            zPos < pad.position.z + padHalfHeight;
            //squares[i].SetActive(inBounds);
        }
    }
}