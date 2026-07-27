using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class FroglopediaGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct FrogPattern
    {
        public string name;
        public Texture2D texture;
    }

    [Header("Book")]
    public Book book;

    [Header("Cameras")]
    public Camera pageCamera;
    public Camera tocCamera;
    public TMP_Text tocText;

    [Header("Frog Rendering")]
    public Renderer[] frogRenderers;
    public FrogPattern[] patterns;

    [Header("Book Pages")]
    public Sprite front;
    public Sprite back;

    [Header("Background")]
    public Renderer backgroundRenderer;
    public Texture2D[] templateBackgrounds;

    private List<Color> bodyColorList = new List<Color>();
    private List<Color> patternColorList = new List<Color>();
    private List<Sprite> generatedPages = new List<Sprite>();

    GameManager gm;

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        foreach (var hex in gm.bodyColors.Values)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color col))
                bodyColorList.Add(col);
        }

        foreach (var hex in gm.patternColors.Values)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color col))
                patternColorList.Add(col);
        }

        StartCoroutine(GenerateAllPages());
    }

    IEnumerator GenerateAllPages()
    {
        foreach (FrogPattern pattern in patterns)
        {
            List<Color> colorsToUse =
                pattern.name == "none"
                ? new List<Color> { bodyColorList[0] }
                : bodyColorList;

            foreach (Color body in colorsToUse)
            {
                for (int i = 0; i < frogRenderers.Length; i++)
                {
                    Color quadBody =
                        pattern.name == "none"
                        ? bodyColorList[i % bodyColorList.Count]
                        : body;

                    Color patCol =
                        pattern.name == "none"
                        ? quadBody
                        : (i < patternColorList.Count
                            ? patternColorList[i]
                            : Color.white);

                    bool discovered =
                        gm.discoveredFrogs.Contains(
                            quadBody + "_" + patCol + "_" + pattern.name);

                    MaterialPropertyBlock block = new MaterialPropertyBlock();
                    frogRenderers[i].GetPropertyBlock(block);

                    block.SetTexture("_MainTex", pattern.texture);
                    block.SetColor("_BodyColor", quadBody);
                    block.SetColor("_PatternColor", patCol);
                    block.SetFloat("_Greyscale", discovered ? 0f : 1f);

                    frogRenderers[i].SetPropertyBlock(block);
                }

                int templateIndex = generatedPages.Count % templateBackgrounds.Length;

                MaterialPropertyBlock bgBlock = new MaterialPropertyBlock();
                backgroundRenderer.GetPropertyBlock(bgBlock);
                bgBlock.SetTexture("_BaseMap", templateBackgrounds[templateIndex]);
                backgroundRenderer.SetPropertyBlock(bgBlock);

                yield return new WaitForEndOfFrame();

                generatedPages.Add(CapturePage(pageCamera));
            }
        }

        // Front cover
        generatedPages.Insert(0, front);

        // TOC generated from the TOC camera (includes UI)
        generatedPages.Insert(1, CapturePage(tocCamera));

        // Back cover
        generatedPages.Add(back);

        book.bookPages = generatedPages.ToArray();
        book.currentPage = 0;
        book.UpdateSprites();

        Debug.Log($"Generated {generatedPages.Count} pages");
    }

    Sprite CapturePage(Camera cam)
    {
        RenderTexture rt = new RenderTexture(512, 768, 16);

        cam.targetTexture = rt;
        RenderTexture.active = rt;

        cam.Render();

        Texture2D tex = new Texture2D(512, 768, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;

        rt.Release();
        Destroy(rt);

        return Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );
    }
}