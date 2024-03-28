using System.Collections.Generic;
using UnityEngine;
using System;



public static class PixelCollider2D 
{
    
    public static List<List<Vector2Int>> Simplify_Paths_Phase_1(List<List<Vector2Int>> Unit_Paths)
    {
        List<List<Vector2Int>> Output = new List<List<Vector2Int>>();
        while (Unit_Paths.Count > 0)
        {
            List<Vector2Int> Current_Path = new List<Vector2Int>(Unit_Paths[0]);
            Unit_Paths.RemoveAt(0);
            bool Keep_Looping = true;
            while (Keep_Looping)
            {
                Keep_Looping = false;
                for (int p = 0; p < Unit_Paths.Count; p++)
                {
                    if (Current_Path[Current_Path.Count - 1] == Unit_Paths[p][0])
                    {
                        Keep_Looping = true;
                        Current_Path.RemoveAt(Current_Path.Count - 1);
                        Current_Path.AddRange(Unit_Paths[p]);
                        Unit_Paths.RemoveAt(p);
                        p--;
                    }
                    else if (Current_Path[0] == Unit_Paths[p][Unit_Paths[p].Count - 1])
                    {
                        Keep_Looping = true;
                        Current_Path.RemoveAt(0);
                        Current_Path.InsertRange(0, Unit_Paths[p]);
                        Unit_Paths.RemoveAt(p);
                        p--;
                    }
                    else
                    {
                        List<Vector2Int> Flipped_Path = new List<Vector2Int>(Unit_Paths[p]);
                        Flipped_Path.Reverse();
                        if (Current_Path[Current_Path.Count - 1] == Flipped_Path[0])
                        {
                            Keep_Looping = true;
                            Current_Path.RemoveAt(Current_Path.Count - 1);
                            Current_Path.AddRange(Flipped_Path);
                            Unit_Paths.RemoveAt(p);
                            p--;
                        }
                        else if (Current_Path[0] == Flipped_Path[Flipped_Path.Count - 1])
                        {
                            Keep_Looping = true;
                            Current_Path.RemoveAt(0);
                            Current_Path.InsertRange(0, Flipped_Path);
                            Unit_Paths.RemoveAt(p);
                            p--;
                        }
                    }
                }
            }
            Output.Add(Current_Path);
        }
        Unit_Paths.Clear();
        return Output;
    }
    public static List<List<Vector2Int>> Simplify_Paths_Phase_2(List<List<Vector2Int>> Input_Paths)
    {
        for (int pa = 0; pa < Input_Paths.Count; pa++)
        {
            for (int po = 0; po < Input_Paths[pa].Count; po++)
            {
                Vector2Int Start = new Vector2Int();
                if (po == 0)
                {
                    Start = Input_Paths[pa][Input_Paths[pa].Count - 1];
                }
                else
                {
                    Start = Input_Paths[pa][po - 1];
                }
                Vector2Int End = new Vector2Int();
                if (po == Input_Paths[pa].Count - 1)
                {
                    End = Input_Paths[pa][0];
                }
                else
                {
                    End = Input_Paths[pa][po + 1];
                }
                Vector2Int Current_Point = Input_Paths[pa][po];
                Vector2 Direction1 = Current_Point - (Vector2)Start;
                Direction1 /= Direction1.magnitude;
                Vector2 Direction2 = End - (Vector2)Start;
                Direction2 /= Direction2.magnitude;
                if (Direction1 == Direction2)
                {
                    Input_Paths[pa].RemoveAt(po);
                    po--;
                }
            }
        }
        return Input_Paths;
    }
    public static List<List<Vector2Int>> Get_Unit_Paths(Texture2D texture, float alphaCutoff)
    {
        List<List<Vector2Int>> Output = new List<List<Vector2Int>>();
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (pixelSolid(texture, new Vector2Int(x, y), alphaCutoff))
                {
                    if (!pixelSolid(texture, new Vector2Int(x, y + 1), alphaCutoff))
                    {
                        Output.Add(new List<Vector2Int>() { new Vector2Int(x, y + 1), new Vector2Int(x + 1, y + 1) });
                    }
                    if (!pixelSolid(texture, new Vector2Int(x, y - 1), alphaCutoff))
                    {
                        Output.Add(new List<Vector2Int>() { new Vector2Int(x, y), new Vector2Int(x + 1, y) });
                    }
                    if (!pixelSolid(texture, new Vector2Int(x + 1, y), alphaCutoff))
                    {
                        Output.Add(new List<Vector2Int>() { new Vector2Int(x + 1, y), new Vector2Int(x + 1, y + 1) });
                    }
                    if (!pixelSolid(texture, new Vector2Int(x - 1, y), alphaCutoff))
                    {
                        Output.Add(new List<Vector2Int>() { new Vector2Int(x, y), new Vector2Int(x, y + 1) });
                    }
                }
            }
        }
        return Output;
    }
    public static bool pixelSolid(Texture2D texture, Vector2Int point, float alphaCutoff)
    {
        if (point.x < 0 || point.y < 0 || point.x >= texture.width || point.y >= texture.height)
        {
            return false;
        }
        float pixelAlpha = texture.GetPixel(point.x, point.y).a;
        if (alphaCutoff == 0)
        {
            if (pixelAlpha != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (alphaCutoff == 1)
        {
            if (pixelAlpha == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return pixelAlpha >= alphaCutoff;
        }
    }

    public static List<List<Vector2Int>> Get_Unit_Paths1(Color[] colors, int width, int height, float alphaCutoff)
    {
        List<List<Vector2Int>> Output = new List<List<Vector2Int>>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pixelSolid1(colors, width, height, new Vector2Int(x, y), alphaCutoff))
                {
                    if (!pixelSolid1(colors, width, height, new Vector2Int(x, y + 1), alphaCutoff))
                    {
                        Output.Add(new List<Vector2Int>() { new Vector2Int(x, y + 1), new Vector2Int(x + 1, y + 1) });
                    }
                    if (!pixelSolid1(colors, width, height, new Vector2Int(x, y - 1), alphaCutoff))
                    {
                        Output.Add(new List<Vector2Int>() { new Vector2Int(x, y), new Vector2Int(x + 1, y) });
                    }
                    if (!pixelSolid1(colors, width, height, new Vector2Int(x + 1, y), alphaCutoff))
                    {
                        Output.Add(new List<Vector2Int>() { new Vector2Int(x + 1, y), new Vector2Int(x + 1, y + 1) });
                    }
                    if (!pixelSolid1(colors, width, height, new Vector2Int(x - 1, y), alphaCutoff))
                    {
                        Output.Add(new List<Vector2Int>() { new Vector2Int(x, y), new Vector2Int(x, y + 1) });
                    }
                }
            }
        }
        return Output;
    }
    public static bool pixelSolid1(Color[] colors, int width, int height, Vector2Int point, float alphaCutoff)
    {
        if (point.x < 0 || point.y < 0 || point.x >= width || point.y >= height)
        {
            return false;
        }
        int index = (point.y*width+point.x);
        float pixelAlpha = colors[index].a;
        if (alphaCutoff == 0)
        {
            if (pixelAlpha != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (alphaCutoff == 1)
        {
            if (pixelAlpha == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return pixelAlpha >= alphaCutoff;
        }
    }
}

