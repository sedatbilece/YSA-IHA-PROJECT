using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class EngelUretici : MonoBehaviour
{
    public static float boruUzaklikX;
    public static float boslukMerkezY;

    [Header("Referanslar")]
    public Transform engelParent;
    public GameObject engelPref;

    [Header("Degerler")]
    public float spawnPerSecond;
    public float hareketHiz;
    public float kusKonumX;
    public float yGenislik;

    List<Transform> engeller = new List<Transform>();
    List<Transform> gecilmisler = new List<Transform>();

    Coroutine uretimCor;

    [Header("Gozlem")]
    public Transform siradakiEngel;

    int uretilenAdet = 1;

    [Header("Zemin")]
    public Transform zemin;
    public float resetPos;
    Vector3 startPos;

    public void OyunBasladi()
    {
        if(uretimCor != null)
        {
            engeller.ForEach(engel => Destroy(engel.gameObject));
            gecilmisler.ForEach(engel => Destroy(engel.gameObject));
            engeller.Clear();
            gecilmisler.Clear();
            StopCoroutine(uretimCor);
        }
        else
        {
            startPos = zemin.position;
        }

        uretimCor = StartCoroutine(SureliUretim());
        siradakiEngel = engeller[0];
    }

    void FixedUpdate()
    {
        if(uretimCor != null)
        {
            for (int i = engeller.Count - 1; i >= 0; i--)
            {
                engeller[i].position += Time.deltaTime * Vector3.left * hareketHiz;

                if (engeller[i].position.x < kusKonumX)
                {
                    StartCoroutine(EngelSil(engeller[i]));
                }
            }

            for (int i = gecilmisler.Count - 1; i >= 0; i--)
            {
                gecilmisler[i].position += Time.deltaTime * Vector3.left * hareketHiz;
            }

            boruUzaklikX = (siradakiEngel.position.x - kusKonumX) / (engelParent.position.x - kusKonumX);
            boslukMerkezY = siradakiEngel.position.y / yGenislik / 2 + .5f;

            boruUzaklikX = (float)System.Math.Round(boruUzaklikX, 2);
            boslukMerkezY = (float)System.Math.Round(boslukMerkezY, 2);

            zemin.position += Time.deltaTime * Vector3.left * hareketHiz;

            if (zemin.position.x <= resetPos)
                zemin.position = startPos;
        }
    }

    IEnumerator SureliUretim()
    {
        while (true)
        {
            EngelUret();
            yield return new WaitForSeconds(spawnPerSecond);
        }
    }

    IEnumerator EngelSil(Transform silinen)
    {
        engeller.Remove(silinen);
        gecilmisler.Add(silinen);
        siradakiEngel = engeller[0];
        yield return new WaitForSeconds(2);
        if (silinen)
        {
            gecilmisler.Remove(silinen);
            Destroy(silinen.gameObject);
        }
    }

    public void EngelUret()
    {
        Transform uretilen = Instantiate(engelPref, engelParent).transform;
        uretilen.localPosition = Random.Range(-1f, 1f) * yGenislik * Vector3.up;
        uretilen.name = "Engel " + uretilenAdet;
        engeller.Add(uretilen);

        uretilenAdet++;
    }
}
