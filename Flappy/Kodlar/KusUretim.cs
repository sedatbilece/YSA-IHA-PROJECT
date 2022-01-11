using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KusUretim : MonoBehaviour
{
    //public static KusUretim kusUretim;

    //public GameObject kusPref;
    //public Vector3 uretKonum;

    //List<KusHareket> hayattaKuslar = new List<KusHareket>();
    //List<KusHareket> olmusKuslar = new List<KusHareket>();

    //private void Awake()
    //{
    //    kusUretim = this;
    //}

    //public void KuslariUret(int kusAdet)
    //{
    //    for (int i = 0; i < kusAdet; i++)
    //    {
    //        KusHareket uretilen = Instantiate(kusPref).GetComponent<KusHareket>();
    //        uretilen.OyunBasladi(null, uretKonum);
    //        hayattaKuslar.Add(uretilen.GetComponent<KusHareket>());
    //    }
    //}

    //public void KuslariSifirla(NeuralNetwork beyin)
    //{
    //    olmusKuslar.ForEach(olmus =>
    //    {
    //        olmus.OyunBasladi(beyin.Copy(), uretKonum);
    //        olmus.Mutate();
    //        hayattaKuslar.Add(olmus);
    //    });

    //    olmusKuslar.Clear();
    //}
    
    //public void KusOldu(KusHareket kus)
    //{
    //    hayattaKuslar.Remove(kus);
    //    olmusKuslar.Add(kus);

    //    if(hayattaKuslar.Count == 0)
    //    {

    //    }
    //}
}
