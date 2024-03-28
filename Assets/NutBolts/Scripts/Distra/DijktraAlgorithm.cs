using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijktraAlgorithm 
{
    public static bool[] FREE;
    public static int[] TRACE;
    public static int S, F, N;
    public static int[,] C;
    public static int MAXC = 1000;
    public static int[] D;
    public static FieldMini field;
    public static void InputFirst(int s, int f)
    {

        S = s; F = f;
        D = new int[N];
        TRACE = new int[N];

        for (int i = 0; i < N; i++)
        {
            D[i] = MAXC;

        }
       
        for (int i = 0; i < N; i++)
        {
            int c = i / field.WIDTH;
            int r = i % field.WIDTH;
            FREE[i] = true;
            
        }
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
            {

                if (field.joints[i, j] >0 && FREE[i] && FREE[j])
                    C[i, j] = field.joints[i,j];
                else
                    C[i, j] = MAXC;
                if (i == j) C[i, j] = 0;
            }
        D[S] = 0;
    }
    public static void InputSecond(int s, int f)
    {

        S = s; F = f;
        D = new int[N];
        TRACE = new int[N];

        for (int i = 0; i < N; i++)
        {
            D[i] = MAXC;
        }
       
        for (int i = 0; i < N; i++)
        {
            int c = i / field.WIDTH;
            int r = i % field.WIDTH;
            FREE[i] = true;          
        }
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
            {

                if (field.joints[i, j] >0 && FREE[i] && FREE[j])
                    C[i, j] = field.joints[i,j];
                else
                    C[i, j] = MAXC;
                if (i == j) C[i, j] = 0;
            }
        D[S] = 0;
    }
    public static void Init(int w, int h)
    {
        field = new FieldMini(w, h);
        N = field.WIDTH * field.HEIGHT;
        FREE = new bool[N];
        C = new int[N, N];
    }
    public static int[] Output()
    {
        
        int i, u, v, min;
        while (true)
        {
            u = -1;
            min = MAXC;
            for (i = 0; i < N; i++)
            {
                if (FREE[i] && D[i] < min)
                {
                    min = D[i];
                    u = i;
                }
            }
            if (u == F || u == -1) break;
            FREE[u] = false;
            for (v = 0; v < N; v++)
            {
                if (FREE[v] && D[v] > D[u] + C[u, v])
                {
                    D[v] = D[u] + C[u, v];
                    TRACE[v] = u;
                }
            }
        }
        if (D[F] == MAXC)
        {
            return null;
        }
        else
        {
            List<int> result = new List<int>();
            int tmp = F;
            string str = "";
            while (F != S)
            {
                F = TRACE[F];
                result.Add(F);

            }
            result.Remove(S);
            result.Reverse();
            result.Add(tmp);
            for (int k = 0; k < result.Count; k++)
            {
                str += result[k] + ", ";
            }
           
            result.Add(F);
            return result.ToArray();

        }

    }
}
public class FieldMini
{
    public int WIDTH, HEIGHT;
    public int[,] joints;
    public FieldMini(int W,int H)
    {
        this.WIDTH = W;
        this.HEIGHT = H;
        int N = W * H;
        joints = new int[W * H, W * H];
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
            {
                joints[i, j] = 0;
            }
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {

                if (i / WIDTH == j / WIDTH)
                {
                    if ((i + 1 < N && i + 1 == j) || i - 1 >= 0 && i - 1 == j)
                    {
                        joints[i, j] = 2;
                    }
                }
                else if (i % WIDTH == j % WIDTH)
                {
                    if ((i + WIDTH == j && i + WIDTH < N) || (i - WIDTH >= 0 && i - WIDTH == j))
                    {
                        joints[i, j] = 2;
                    }
                }else if(i/WIDTH+1==j/WIDTH && i % WIDTH + 1 == j % WIDTH)
                {
                    if(i+WIDTH+1<N)
                    {
                        joints[i, j] = 3;
                    }
                }else if(i/WIDTH+1==j/WIDTH && i % WIDTH - 1 == j % WIDTH)
                {
                    if (i +WIDTH+1<N)
                    {
                        joints[i, j] = 3;
                    }
                }else if(i/WIDTH-1==j/WIDTH && i % WIDTH - 1 == j % WIDTH)
                {
                    if (i - WIDTH  >= 0)
                    {
                        joints[i, j] = 3;
                    }
                }else if(i/WIDTH-1==j/WIDTH && i % WIDTH + 1 == j % WIDTH)
                {
                    if (i - WIDTH  >= 0)
                    {
                        joints[i, j] = 3;
                    }
                }

            }
        }
    }
}
