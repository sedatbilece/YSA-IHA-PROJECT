using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DusmanKontrol : MonoBehaviour
{
    public static DusmanKontrol dk;

    public Material[] oluTankMat;

    [Header("Kuvvet")]
    public float yukari;
    public float disari;
    public float donme;

    [Header("Degerler")]
    public float genislik;

    void Start()
    {
        dk = this;
    }

    public void TankPatlat(Transform tank, Vector3 bombaKonum)
    {
        tank.tag = "Untagged";
        bombaKonum = tank.position - bombaKonum;
        bombaKonum.y = 0;
        new List<MeshRenderer>(tank.GetComponentsInChildren<MeshRenderer>()).ForEach(mesh => mesh.materials = oluTankMat);
        tank.GetComponent<Rigidbody>().velocity = Vector3.up * (yukari + Random.Range(-3, 3)) + bombaKonum.normalized * (disari + Random.Range(-2, 2));
        tank.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere.normalized * donme;
    }
}