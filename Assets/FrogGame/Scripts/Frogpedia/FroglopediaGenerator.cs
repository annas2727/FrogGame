using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FroglopediaGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct FrogPattern
    {
        public string name;
        public Texture2D texture;
    }

    public Book book;
    public Camera pageCamera;
    public Renderer[] frogRenderers; // drag 9 quads here
    public FrogPattern[] patterns;
    public Sprite front;
    public Sprite TOC; 
    public Sprite back;
    
    private List<Color> bodyColorList = new List<Color>();
    private List<Color> patternColorList = new List<Color>();
    private List<Sprite> generatedPages = new List<Sprite>();
    public Renderer backgroundRenderer; // the plane behind the quads
    public Texture2D[] templateBackgrounds; // your 4 template textures
    
    GameManager gm; 
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        foreach (var hex in gm.bodyColors.Values)
        {
            Color col;
            if (ColorUtility.TryParseHtmlString(hex, out col))
                bodyColorList.Add(col);
        }

        foreach (var hex in gm.patternColors.Values)
        {
            Color col;
            if (ColorUtility.TryParseHtmlString(hex, out col))
                patternColorList.Add(col);
        }

        StartCoroutine(GenerateAllPages());
    }

    IEnumerator GenerateAllPages()
    {
        foreach (FrogPattern pattern in patterns)
        {
            List<Color> colorsToUse = pattern.name == "none" ? 
                new List<Color> { bodyColorList[0] } : bodyColorList;

            foreach (Color body in colorsToUse)
            {
                for (int i = 0; i < frogRenderers.Length; i++)
                {
                    Color quadBody = pattern.name == "none" ?
                        bodyColorList[i % bodyColorList.Count] : // cycle through body colors
                        body; // use current body color for patterned frogs
                    
                    Color patCol = pattern.name == "none" ? quadBody :
                        i < patternColorList.Count ? patternColorList[i] : Color.white;
                    
                    bool discovered = gm.discoveredFrogs.Contains(quadBody + "_" + patCol + "_" + pattern.name);

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

                generatedPages.Add(CapturePage());
            }
        }

        generatedPages.Insert(0, TOC);
        generatedPages.Insert(0, front);
        generatedPages.Add(back);

        book.bookPages = generatedPages.ToArray();
        book.currentPage = 0;
        book.UpdateSprites();

        Debug.Log("Generated " + generatedPages.Count + " pages");
    }

    Sprite CapturePage()
    {
        RenderTexture rt = new RenderTexture(512, 768, 16);
        pageCamera.targetTexture = rt;
        pageCamera.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(512, 768, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, 512, 768), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        pageCamera.targetTexture = null;
        rt.Release();

        return Sprite.Create(tex, new Rect(0, 0, 512, 768), new Vector2(0.5f, 0.5f));
    }
}