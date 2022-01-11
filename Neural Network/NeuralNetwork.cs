using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeuralNetwork
{
    int input_nodes, hidden_nodes, output_nodes;
    public Matrix weights_ih, weights_ho;
    Matrix bias_h, bias_o;


    //sigmoid aktivasyon fonksiyonumuz
    ActivationFunction activation_function = new ActivationFunction(
    (x, bos1, bos2) => (float)(1 / (1 + Math.Exp(-x))),
    (y, bos1, bos2) => y * (1 - y));


    // burada modelimizin giriþ çýkýç ve hidden layer parametreleri verilmekte  
    public NeuralNetwork(int in_nodes, int hid_nodes, int out_nodes)
    {
        input_nodes = in_nodes;
        hidden_nodes = hid_nodes;
        output_nodes = out_nodes;

        weights_ih = new Matrix(this.hidden_nodes, this.input_nodes);
        weights_ho = new Matrix(this.output_nodes, this.hidden_nodes);
        weights_ih.Randomize();
        weights_ho.Randomize();

        bias_h = new Matrix(this.hidden_nodes, 1);
        bias_o = new Matrix(this.output_nodes, 1);
        bias_h.Randomize();
        bias_o.Randomize();
    }

    // aðýmýzýn kopyasýný oluþturma Constructoru 
    // sonraki jenerasyonlar için en iyi örnek kopyalanýrken kullanýlacak
    public NeuralNetwork(NeuralNetwork kopya)
    {
        NeuralNetwork a = kopya;
        input_nodes = a.input_nodes;
        hidden_nodes = a.hidden_nodes;
        output_nodes = a.output_nodes;

        weights_ih = a.weights_ih.Copy();
        weights_ho = a.weights_ho.Copy();

        bias_h = a.bias_h.Copy();
        bias_o = a.bias_o.Copy();
    }

    public List<float> Predict(List<float> input_array)
    {
        //input deðerlerimiz
        Matrix inputs = Matrix.FromArray(input_array);

        // input ile aðýrlýk çarpýmý
        Matrix hidden = Matrix.Multiply(this.weights_ih, inputs);

        //bias eklendi
        hidden.Add(bias_h);
        // sigmoid aktivasyon fonksiyonu
        hidden.Map(activation_function.func);


        // çýktýnýn çýktýsý alýnýyor
        Matrix output = Matrix.Multiply(this.weights_ho, hidden);
        output.Add(bias_o);
        output.Map(activation_function.func);

        // deðer döndürülüyor 0-1 aralýðýnda 
        return output.ToArray();
    }

    public NeuralNetwork Copy()
    {
        return new NeuralNetwork(this);
    }

    public void Mutate(Func<float, int, int, float>  func)
    {// verilen deðerleri günceller

        weights_ih.Map(func);
        weights_ho.Map(func);
        bias_h.Map(func);
        bias_o.Map(func);
    }
}

public class ActivationFunction
{
    public Func<float, int, int, float> func, dfunc;

    public ActivationFunction(Func<float, int, int, float> func, Func<float, int, int, float> dfunc)
    {
        this.func = func;
        this.dfunc = dfunc;
    }
}