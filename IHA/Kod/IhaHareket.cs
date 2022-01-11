using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Ýha konum(x, z pos) , Ýha forward(x, z forward) , hedef konum(x, z pos)
public class IhaHareket : MonoBehaviour
{
    public static Vector3 alanGenislik;
    public static Vector3 hedefKonum;
    public static Vector3 hedefinGercekKonumu;

    public static float mutSiddet = .5f;
    public float ileriHiz;
    public float donmeHiz;

    public NeuralNetwork brain;
    public Transform bomba;

    [Header("Gozlem")]
    public float saglik;
    bool hayatta;

    public void OyunBasladi(NeuralNetwork beyin, Vector3 pos)
    {
        // oyun baþýnda random deðerler atanýr

        if(UnityEngine.Random.Range(0f, 1f) < .2f)
        {
            brain = new NeuralNetwork(6, 5, 2);
        }
        else
        {
            brain = beyin != null ? beyin : new NeuralNetwork(6, 5, 2);
        }

        hayatta = true;
        transform.position = pos;
        gameObject.SetActive(true);
        transform.rotation = Quaternion.LookRotation(Vector3.back);
        bomba.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (hayatta)
        {
            Dusun();
            transform.position += transform.forward * ileriHiz * Time.deltaTime;

            if (transform.position.x < 0 || transform.position.z < 0 || transform.position.x > alanGenislik.x || transform.position.z > alanGenislik.z)
                DisariCikti();
        }
    }

    List<float> DegerleriHesapla()
        // iha pozisyonu ve nereye gideceði
    {
        Vector3 ihaPos = transform.position;
        ihaPos.x = Yuvarla(ihaPos.x / alanGenislik.x);
        ihaPos.z = Yuvarla(ihaPos.z / alanGenislik.z);

        Vector3 ileri = transform.forward;
        ileri.x = Yuvarla((ileri.x + 1) / 2);
        ileri.z = Yuvarla((ileri.z + 1) / 2);


        return new List<float> { ihaPos.x, ihaPos.z, ileri.x, ileri.z, hedefKonum.x, hedefKonum.z };
    }

    int sayac = 10;

    float timer;
    bool pozitif;

    public void Dusun()
    {// önceki jenerasyon deðerleri verilir model eðitilir
        List<float> inputs = DegerleriHesapla();

        List<float> output = brain.Predict(inputs);

       

        if (output[0] < .5)
            BombaAt();

        float don = output[1] * 2 - 1;
        timer += Time.deltaTime * donmeHiz * don;

        

        if (timer > 360)
        {
            DisariCikti();
        }
        else if (timer < -360)
        {
            DisariCikti();
        }

        transform.eulerAngles += Vector3.up * Time.deltaTime * donmeHiz * don;
    }

    void BombaAt()//bombanýn hareket ve hedefe çarpýnca yok olmasý fonksiyonu
    {
        Vector3 bombaKonum = transform.position + transform.forward * 31.2f;
        bombaKonum.y = hedefinGercekKonumu.y;
        float mesafe = (hedefinGercekKonumu - bombaKonum).magnitude;
        saglik = 1 - (mesafe / 100);

        IhaBitti();
        // çarpým noktasý alýnýr
        bomba.position = bombaKonum;
        bomba.gameObject.SetActive(true);
    }

    void DisariCikti()// area dýþýna çýkanlar direkt elenir
    {
        if (hayatta)
        {
            saglik = 0;
            IhaBitti();
        }
    }

    void IhaBitti()// ihanýn hayatý sonlandýrýlýr
    {
        hayatta = false;
        gameObject.SetActive(false);
        OyunKontrol.ok.IhaBitti(this);
    }

    public void Mutate()// mutate iþlemi
    {
        brain.Mutate((x, bos1, bos2) =>
        {
            float offset = UnityEngine.Random.Range(-1f, 1f) * mutSiddet; //if (Math.random() < 0.1)
            return x + offset;
        });
    }

    public static float Yuvarla(float deger)
    {
        return (float)Math.Round(deger, 2);
    }
}
