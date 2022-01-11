using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OyunKontrol : MonoBehaviour
{
    public static OyunKontrol ok;

    [Range(0, 1)]
    public float mutSiddet;

    public Text durumText;
    public Text hizText;
    public Vector3 alanGenislik;

    [Header("Ihalar")]
    public List<IhaHareket> aktifIhalar = new List<IhaHareket>();
    public List<IhaHareket> bitmisIhalar = new List<IhaHareket>();

    int jenerasyon = 0;
    float baslaZaman;
    float maxScore;
    bool basladi;

    [Header("Iha Uretim")]
    public int uretimAdet;
    public Transform ihaParent;
    public GameObject ihaPref;
    public Vector3 uretimRange;

    [Header("Iha Uretim")]
    public Transform hedefParent;
    public Vector3 hedefUretimRange;

    [Header("Bomba")]
    public Transform bombaParent;

    private void Awake()
    {
        ok = this;
        durumText.text = "Baþlamak için Space";
    }

    private void Start()
    {
        OyunaBasla();    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            OyunaBasla();

        if (basladi)
            durumText.text = "Jenerasyon : " + jenerasyon + "\nHigh Score : " + maxScore + "\nGecen Sure : " + (((int)((Time.time - baslaZaman) * 10)) / 10f) + "\nIHA Sayi : " + (uretimAdet - bitmisIhalar.Count);
    }

    void OyunaBasla()
    {
        IhaHareket.alanGenislik = alanGenislik;
        basladi = true;
        YeniJenerasyon();
        maxScore = 0;
    }

    // oyun baþý rastgele noktalarda iha üretimi
    public void IhalariUret(int ihaAdet)
    {
        Vector3 konum = uretimRange; konum.x *= Random.Range(-1f, 1f); konum.z *= Random.Range(-1f, 1f);
        konum += ihaParent.position;

        for (int i = 0; i < ihaAdet; i++)
        {
            IhaHareket uretilen = Instantiate(ihaPref, ihaParent).GetComponent<IhaHareket>();
            uretilen.OyunBasladi(null, konum);
            aktifIhalar.Add(uretilen.GetComponent<IhaHareket>());
            uretilen.bomba.parent = bombaParent;
        }
    }


    // yeni jenerasyon baþlatma iþlemi
    public void IhalariSifirla(List<IhaHareket> beyinler, IhaHareket bestBeyin)
    {
        IhaHareket.mutSiddet = mutSiddet;

        // saðlýk durumu en iyi olan yani en baþarýlý ihanýn score'u alýnýr
        maxScore = bestBeyin.saglik;

        Vector3 konum = uretimRange; konum.x *= Random.Range(-1f, 1f); konum.z *= Random.Range(-1f, 1f);
        konum += ihaParent.position;


        // burada ise baþarýlýlarýn deðereri kopyalanýr ve mutate edilir
        bitmisIhalar.ForEach(olmus =>
        {
            olmus.OyunBasladi(beyinler[Random.Range(0, beyinler.Count)].brain.Copy(), konum);
            olmus.Mutate();
            aktifIhalar.Add(olmus);
        });

        aktifIhalar[0].OyunBasladi(bestBeyin.brain.Copy(), konum);

        bitmisIhalar.Clear();
    }

    public void IhaBitti(IhaHareket kus)
    {
        aktifIhalar.Remove(kus);
        bitmisIhalar.Add(kus);

        if (bitmisIhalar.Count == uretimAdet)
        {
            YeniJenerasyon();
        }
    }



    void YeniJenerasyon()
    {
        bitmisIhalar.Sort((x, y) => x.saglik.CompareTo(y.saglik));
        HedefKonumAyarla();

        if (bitmisIhalar.Count > 0)
        {
            IhalariSifirla(bitmisIhalar.GetRange(bitmisIhalar.Count - 6, 5) , bitmisIhalar[bitmisIhalar.Count - 1]);
        }
        else
        {
            IhalariUret(uretimAdet);
        }

        baslaZaman = Time.time;
        jenerasyon++;
    }

    void HedefKonumAyarla()
    {
        Vector3 konum = hedefUretimRange; konum.x *= Random.Range(-1f, 1f); konum.z *= Random.Range(-1f, 1f);
        hedefParent.GetChild(0).localPosition = konum;

        konum = hedefParent.GetChild(0).position;
        IhaHareket.hedefinGercekKonumu = konum;

        konum.x = IhaHareket.Yuvarla(konum.x / alanGenislik.x);
        konum.z = IhaHareket.Yuvarla(konum.z / alanGenislik.z);

        IhaHareket.hedefKonum = konum;
    }

    public void HizAyarla(float hiz)
    {
        float yeniHiz = 1 + hiz * 9;
        
        Time.timeScale = yeniHiz;
        Time.fixedDeltaTime = yeniHiz * .02f;

        hizText.text = "Hiz : " + IhaHareket.Yuvarla(yeniHiz);
    }
}