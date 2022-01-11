using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaKontrol : MonoBehaviour
{
    public static BombaKontrol bk;

    [Header("Prefab")]
    public GameObject bombaPref;
    public GameObject patlamaEfekt;

    [Header("Referanslar")]
    public Transform bombaUretKonum;

    [Header("Degerler")]
    public float bombaScale;

    void Start()
    {
        bk = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BombaBirak();
        }
    }

    public void BombaBirak()
    {
        Transform uretilen = Instantiate(bombaPref).transform;
        uretilen.position = bombaUretKonum.position;
        uretilen.rotation = transform.rotation;
        uretilen.GetComponent<Rigidbody>().velocity = transform.forward * 15;
    }
    
    public void PatlamaEfektiUret(Vector3 konum)
    {
        GameObject efekt = Instantiate(patlamaEfekt, konum, Quaternion.identity);
        efekt.transform.localScale = Vector3.one * bombaScale;
        new List<ParticleSystem>(efekt.GetComponentsInChildren<ParticleSystem>()).ForEach(particle => particle.startSize *= bombaScale);
        Destroy(efekt, 10);
    }
}
