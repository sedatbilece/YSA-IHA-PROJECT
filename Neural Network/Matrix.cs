using System.Collections;
using System.Collections.Generic;
using System;

public class Matrix
{
    float[,] data;
    int rows, cols;

    public Matrix(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;

        data = new float[rows, cols];
    }

    public Matrix Copy()
    {
        Matrix m = new Matrix(rows, cols);
        m.data = data.Clone() as float[,];
        return m;
    }

    public static Matrix FromArray(List<float> arr)
    {
        Matrix carpim = new Matrix(arr.Count, 1);
        carpim.Map((e, i, _) => arr[i]);
        return carpim;
    }

    public static Matrix Multiply(Matrix a, Matrix b)
    {
        if (a.cols != b.rows)
        {
            UnityEngine.Debug.Log("Columns of A must match rows of B.");
            return null;
        }

        Matrix carpim = new Matrix(a.rows, b.cols);
        carpim.Map((e, i, j) =>
        {
            
            float sum = 0;
            for (int k = 0; k < a.cols; k++)
            {
                sum += a.data[i,k] * b.data[k,j];
            }

            return sum;
        });

        return carpim;
    }

    public List<float> ToArray()
    {
        List<float> arr = new List<float>();

        for (int i = 0; i < this.rows; i++)
            for (int j = 0; j < this.cols; j++)
                arr.Add(data[i,j]);

        return arr;
    }

    public Matrix Randomize()
    {
        Map((a, b, c) => UnityEngine.Random.Range(-1f, 1f));
        return this;
    }

    public Matrix Add(Matrix n)
    {
        if (rows != n.rows || cols != n.cols)
        {
            UnityEngine.Debug.Log("kolonlar eþleþmiyor");
            return null;
        }

        Map((e, i, j) => e + n.data[i,j]);
        return this;
    }

    public void Map(Func<float, int, int, float> action)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                float deger = data[i,j];
                data[i,j] = action(deger, i, j);
            }
        }
    }
}



