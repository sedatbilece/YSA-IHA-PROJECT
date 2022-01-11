using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlappyKontrol : MonoBehaviour
{
    public static FlappyKontrol kusUretim;
    public int uretilecekKusAdet;

    public Transform kusParent;
    public GameObject kusPref;
    public Text durumText;
    public Vector3 uretKonum;

    public List<KusHareket> hayattaKuslar = new List<KusHareket>();
    public List<KusHareket> olmusKuslar = new List<KusHareket>();

    int jenerasyon = 0;
    float baslaZaman;
    bool basladi;
    float maxScore;

    private void Awake()
    {
        kusUretim = this;
        durumText.text = "Baþlamak için Space";
    }

    public void KuslariUret(int kusAdet)
    {
        for (int i = 0; i < kusAdet; i++)
        {
            KusHareket uretilen = Instantiate(kusPref, kusParent).GetComponent<KusHareket>();
            uretilen.OyunBasladi(null, uretKonum);
            hayattaKuslar.Add(uretilen.GetComponent<KusHareket>());
        }
    }

    public void KuslariSifirla(KusHareket beyin)
    {
        olmusKuslar.ForEach(olmus =>
        {
            olmus.OyunBasladi(beyin.brain.Copy(), uretKonum);
            olmus.Mutate();

            hayattaKuslar.Add(olmus);
        });

        olmusKuslar.Clear();
    }

    public void KusOldu(KusHareket kus)
    {
        hayattaKuslar.Remove(kus);
        olmusKuslar.Add(kus);

        if (olmusKuslar.Count == uretilecekKusAdet)
        {
            YeniJenerasyon();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            OyunaBasla();

        if (basladi)
            durumText.text = "Jenerasyon : " + jenerasyon + "\nHigh Score : " + maxScore + "\nGecen Sure : " + (((int)((Time.time - baslaZaman) * 10)) / 10f) + "\nKus Sayi : " + (uretilecekKusAdet - olmusKuslar.Count);
    }

    void OyunaBasla()
    {
        basladi = true;
        YeniJenerasyon();

        maxScore = 0;
    }

    void YeniJenerasyon()
    {
        GetComponent<EngelUretici>().OyunBasladi();

        olmusKuslar.Sort((x, y) => x.saglik.CompareTo(y.saglik));

        if (olmusKuslar.Count > 0)
        {
            KuslariSifirla(olmusKuslar[olmusKuslar.Count - 1]);
        }
        else
        {
            KuslariUret(uretilecekKusAdet);
        }

        jenerasyon++;

        if ((Time.time - baslaZaman) > maxScore)
            maxScore = ((int)((Time.time - baslaZaman) * 10)) / 10f;

        baslaZaman = Time.time;
    }
}
