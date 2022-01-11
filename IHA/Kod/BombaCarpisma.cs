using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaCarpisma : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // yön  atama vektörü
    private void FixedUpdate()
    {
        rb.velocity += Vector3.down * 10 * Time.deltaTime;
        Vector3 yon = transform.forward;
        yon.y = 0;
        transform.rotation = Quaternion.LookRotation(rb.velocity + yon.normalized * 10);
    }




    // bomba tanka denk gelirse patlama iþlemi
    private void OnTriggerEnter(Collider other)
    {
        new List<Collider>(Physics.OverlapSphere(transform.position, DusmanKontrol.dk.genislik)).ForEach(collider =>
        {
            if (collider.tag == "Tank")
            {
                DusmanKontrol.dk.TankPatlat(collider.transform, transform.position);
            }
        });

        BombaKontrol.bk.PatlamaEfektiUret(transform.position);
        Destroy(gameObject);
    }
}
