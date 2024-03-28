using Library;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGeometry : MonoBehaviour
{
    private Texture2D m_Texture;
    private Color[] m_Colors;
    PolygonCollider2D PGC2D;
    PolygonCollider2D pl;
    SpriteRenderer spriteRend;
    Color zeroAlpha = Color.clear;
    Vector2 mPosClick = new Vector2(-1000, -1000);
    public int erSize;
    public int r = 10;
    [Range(0, 1)]
    public float alphaCutoff = 0.5f;
    Vector3 localScale = Vector3.one;
    // Start is called before the first frame update
    void Start()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        var tex = spriteRend.sprite.texture;
        m_Texture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        m_Texture.filterMode = FilterMode.Bilinear;
        m_Texture.wrapMode = TextureWrapMode.Clamp;
        m_Colors = tex.GetPixels();
        m_Texture.SetPixels(m_Colors);
        m_Texture.Apply();
        spriteRend.sprite = Sprite.Create(m_Texture, spriteRend.sprite.rect, new Vector2(0.5f, 0.5f));
        PGC2D = GetComponent<PolygonCollider2D>();
        pl = PGC2D;
        PGC2D.enabled = true;
        
        localScale=transform.localScale;
        GenerateCollider();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    mPosClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    UpdateTexture();
        //    GenerateCollider();
        //    return;
        //}
    }
    public void Init(List<Vector2> pivots)
    {

    }
    public void UpdateTexture()
    {
        int w = (int)((float)m_Texture.width*localScale.x);
        int h =(int)((float)m_Texture.height*localScale.y);
        var mousePos = mPosClick - (Vector2)spriteRend.bounds.min;
        mousePos.x *= w / spriteRend.bounds.size.x;
        mousePos.y *= h / spriteRend.bounds.size.y;
        Vector2Int p = new Vector2Int((int)mousePos.x, (int)mousePos.y);
        Vector2Int start = new Vector2Int();
        Vector2Int end = new Vector2Int();
        float erX = (float)(erSize) / localScale.x;
        float erY = (float)(erSize) / localScale.y;
        Vector2Int erSizeVector2=new Vector2Int((int)erX,(int)erY);
        start.x = Mathf.Clamp(p.x - erSizeVector2.x, 0, w);
        start.y = Mathf.Clamp(p.y - erSizeVector2.y, 0, h);
        end.x = Mathf.Clamp(p.x + erSizeVector2.x, 0, w);
        end.y = Mathf.Clamp(p.y + erSizeVector2.y, 0, h);

        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                Vector2 pixel = new Vector2(x, y);
                float dis = Vector2.Distance(pixel, p);
                if (dis > erSize)
                {
                    continue;
                }
                m_Colors[x + y * w] = zeroAlpha;
            }
        }
        m_Texture.SetPixels(m_Colors);
        m_Texture.Apply();
        spriteRend.sprite = Sprite.Create(m_Texture, spriteRend.sprite.rect, new Vector2(0.5f, 0.5f));
    }
    void UpdateColliderSquare(Vector2 begin, Vector2 finish)
    {

        int w = m_Texture.width;
        int h = m_Texture.height;

        var beginPos = begin - (Vector2)spriteRend.bounds.min;
        beginPos.x *= w / spriteRend.bounds.size.x;
        beginPos.y *= h / spriteRend.bounds.size.y;
        Vector2Int pBegin = new Vector2Int((int)beginPos.x, (int)beginPos.y);

        var finishPos = finish - (Vector2)spriteRend.bounds.min;
        finishPos.x *= w / spriteRend.bounds.size.x;
        finishPos.y *= h / spriteRend.bounds.size.y;
        Vector2Int pFinish = new Vector2Int((int)finishPos.x, (int)finishPos.y);

        Vector2Int start = new Vector2Int();
        Vector2Int end = new Vector2Int();

        Vector2Int tmp = pBegin;
        start.x = Mathf.Clamp(Mathf.Min(pBegin.x, pFinish.x) - 10, 0, w);
        start.y = Mathf.Clamp(Mathf.Min(pBegin.y, pFinish.y) - 10, 0, h);
        end.x = Mathf.Clamp(Mathf.Max(pFinish.x, pBegin.x) + 10, 0, w);
        end.y = Mathf.Clamp(Mathf.Max(pFinish.y, pBegin.y) + 10, 0, h);
        Vector2 dir = pFinish - beginPos;

        Vector2Int center = new Vector2Int((start.x + end.x) / 2, (start.y + end.y) / 2);
        Vector2Int m = new Vector2Int(center.x - start.x, center.y - start.y);
        float radius = Mathf.Sqrt(m.x * m.x + m.y * m.y) / 2;
        int iRadius = (int)radius;
        dir = Vector2.right;
        Vector2 l = new Vector2(start.x, start.y);
        Vector2 p = new Vector2(center.x, center.y);
        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                Vector2 pixel = new Vector2(x, y);

                Vector2 linePos = p;
                float d = Vector2.Dot(pixel - l, dir) / dir.sqrMagnitude;
                d = Mathf.Clamp01(d);
                linePos = Vector2.Lerp(l, p, d);

                if ((pixel - linePos).sqrMagnitude <= 100)
                {
                    m_Colors[x + y * w] = zeroAlpha;
                }
                l = new Vector2(x, y);
            }
        }
    }
    void UpdateCollider(Vector2 begin, Vector2 finish, bool circle = false)
    {
        int w = m_Texture.width;
        int h = m_Texture.height;
        var beginPos = begin - (Vector2)spriteRend.bounds.min;
        beginPos.x *= w / spriteRend.bounds.size.x;
        beginPos.y *= h / spriteRend.bounds.size.y;
        Vector2Int pBegin = new Vector2Int((int)beginPos.x, (int)beginPos.y);

        var finishPos = finish - (Vector2)spriteRend.bounds.min;
        finishPos.x *= w / spriteRend.bounds.size.x;
        finishPos.y *= h / spriteRend.bounds.size.y;
        Vector2Int pFinish = new Vector2Int((int)finishPos.x, (int)finishPos.y);

        Vector2Int start = new Vector2Int();
        Vector2Int end = new Vector2Int();

        Vector2Int tmp = pBegin;
        start.x = Mathf.Clamp(Mathf.Min(pBegin.x, pFinish.x) - r, 0, w);
        start.y = Mathf.Clamp(Mathf.Min(pBegin.y, pFinish.y) - r, 0, h);
        end.x = Mathf.Clamp(Mathf.Max(pFinish.x, pBegin.x) + r, 0, w);
        end.y = Mathf.Clamp(Mathf.Max(pFinish.y, pBegin.y) + r, 0, h);
        Vector2 dir = pFinish - beginPos;
        if (!circle)
        {
            for (int x = start.x; x < end.x; x++)
            {
                for (int y = start.y; y < end.y; y++)
                {
                    Vector2 pixel = new Vector2(x, y);
                    Vector2 linePos = pBegin;

                    float d = Vector2.Dot(pixel - tmp, dir) / dir.sqrMagnitude;
                    d = Mathf.Clamp01(d);
                    linePos = Vector2.Lerp(tmp, pFinish, d);

                    if ((pixel - linePos).sqrMagnitude <= r * r)
                    {
                        m_Colors[x + y * w] = zeroAlpha;
                    }
                }
            }
        }
        else
        {
            Vector2Int center = new Vector2Int((start.x + end.x) / 2, (start.y + end.y) / 2);
            Vector2Int m = new Vector2Int(center.x - start.x, center.y - start.y);
            float radius = Mathf.Sqrt(m.x * m.x + m.y * m.y) / 2;
            int iRadius = (int)radius;
            dir = Vector2.right;
            Vector2 l = new Vector2(start.x, start.y);
            Vector2 p = new Vector2(center.x, center.y);
            for (int x = start.x; x < end.x; x++)
            {
                for (int y = start.y; y < end.y; y++)
                {
                    Vector2 pixel = new Vector2(x, y);
                    if ((pixel - center).sqrMagnitude <= radius * radius)
                    {
                        Vector2 linePos = p;
                        float d = Vector2.Dot(pixel - l, dir) / dir.sqrMagnitude;
                        d = Mathf.Clamp01(d);
                        linePos = Vector2.Lerp(l, p, d);

                        if ((pixel - linePos).sqrMagnitude <= r * r)
                        {
                            m_Colors[x + y * w] = zeroAlpha;
                        }
                        l = new Vector2(x, y);
                    }

                }
            }
        }

        m_Texture.SetPixels(m_Colors);
        m_Texture.Apply();
        spriteRend.sprite = Sprite.Create(m_Texture, spriteRend.sprite.rect, new Vector2(0.5f, 0.5f));
        PGC2D.enabled = true;

    }
    private bool locked = false;
    void GenerateCollider()
    {

        if (locked)
        {
            return;
        }
        locked = true;

        SpriteRenderer SR = GetComponent<SpriteRenderer>();
        Sprite SRSprite = SR.sprite;
        Texture2D SRTxt = SRSprite.texture;
        int width = SRTxt.width;
        int height = SRTxt.height;
        Vector2 pivot = SRSprite.pivot;
        float minBoundX = SRSprite.bounds.min.x;
        float minBoundY = SRSprite.bounds.min.y;
        float maxBoundX = SRSprite.bounds.max.x;
        float maxBoundY = SRSprite.bounds.max.y;
        Color[] colors = SRTxt.GetPixels(0, 0, width, height);
        //Regenerate(colors, width, height, pivot, minBoundX, minBoundY, maxBoundX, maxBoundY);
        Loom.Instance.RunAsync(() =>
        {
            Regenerate(colors, width, height, pivot, minBoundX, minBoundY, maxBoundX, maxBoundY);
        });

    }
    public void Regenerate(Color[] colors, int width, int height, Vector2 pivot, float minBoundX, float minBoundY, float maxBoundX, float maxBoundY)
    {
        List<List<Vector2Int>> Pixel_Paths = new List<List<Vector2Int>>();
      

        Pixel_Paths = PixelCollider2D.Get_Unit_Paths1(colors, width, height, alphaCutoff);
        Pixel_Paths = PixelCollider2D.Simplify_Paths_Phase_1(Pixel_Paths);
        Pixel_Paths = PixelCollider2D.Simplify_Paths_Phase_2(Pixel_Paths);
        List<List<Vector2>> World_Paths = new List<List<Vector2>>();
        World_Paths = Finalize_Paths(Pixel_Paths, width, height, pivot, minBoundX, minBoundY, maxBoundX, maxBoundY);

        Loom.Instance.QueueOnMainThread(() => {
            if (PGC2D)
            {
                PGC2D.pathCount = 0;
                PGC2D.pathCount = World_Paths.Count;
                for (int p = 0; p < World_Paths.Count; p++)
                {
                    PGC2D.SetPath(p, World_Paths[p].ToArray());
                }
                //Debug.Log("Finish regenerate 1");
                Pixel_Paths.Clear();
                World_Paths.Clear();
            }

            locked = false;
        });
    }
    private List<List<Vector2>> Finalize_Paths(List<List<Vector2Int>> Pixel_Paths, int width, int height, Vector2 pivot, float minBoundX, float minBoundY, float maxBoundX, float maxBoundY)
    {
        pivot.x *= Mathf.Abs(maxBoundX - minBoundX);
        pivot.x /= width;
        pivot.y *= Mathf.Abs(maxBoundY - minBoundY);
        pivot.y /= height;

        List<List<Vector2>> Output = new List<List<Vector2>>();
        for (int p = 0; p < Pixel_Paths.Count; p++)
        {
            List<Vector2> Current_List = new List<Vector2>();
            for (int o = 0; o < Pixel_Paths[p].Count; o++)
            {
                Vector2 point = Pixel_Paths[p][o];
                point.x *= Mathf.Abs(maxBoundX - minBoundX);
                point.x /= width;
                point.y *= Mathf.Abs(maxBoundY - minBoundY);
                point.y /= height;
                point -= pivot;
                Current_List.Add(point);
            }
            Output.Add(Current_List);
        }
        return Output;
    }
}
