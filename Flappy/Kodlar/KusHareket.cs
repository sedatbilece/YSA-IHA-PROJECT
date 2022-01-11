using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KusHareket : MonoBehaviour
{
    public static float mutSiddet = .5f;
    public float ziplaKuvvet;

    public NeuralNetwork brain;
    Rigidbody2D rb;

    float baslaZaman;
    bool hayatta;

    [Header("Gozlem")]
    public float saglik;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OyunBasladi(NeuralNetwork beyin, Vector3 pos)
    {
        brain = beyin != null ? beyin : new NeuralNetwork(4, 5, 1);
        baslaZaman = Time.time;
        hayatta = true;
        transform.position = pos;
        gameObject.SetActive(true);

        //if (beyin != null)
        //    Mutate();
    }

    void FixedUpdate()
    {
        if (hayatta)
        {
            rb.velocity = Mathf.Clamp(rb.velocity.y, -25, ziplaKuvvet) * Vector3.up;
            float yukseklik = (transform.position.y + 7) / 24;
            float hiz = (rb.velocity.y + 25) / (25 + ziplaKuvvet);
            Dusun((float)Math.Round(yukseklik, 2), (float)Math.Round(hiz, 2));
        }
    }

    void Zipla()
    {
        rb.velocity = Vector2.up * ziplaKuvvet;
    }

    public void Dusun(float yukseklik, float hiz)
    {
        List<float> inputs = new List<float> { EngelUretici.boruUzaklikX, EngelUretici.boslukMerkezY, yukseklik, hiz };
        List<float> output = brain.Predict(inputs);

        if (output[0] < .5)
            Zipla();
    }

    public void Mutate()
    {
        brain.Mutate((x, bos1, bos2) =>
        {
            float offset = UnityEngine.Random.Range(-1f, 1f) * mutSiddet; //if (Math.random() < 0.1)
            return x + offset;
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Olum();
    }

    void Olum()
    {
        saglik = Time.time - baslaZaman;
        gameObject.SetActive(false);
        hayatta = false;
        FlappyKontrol.kusUretim.KusOldu(this);
    }
}
