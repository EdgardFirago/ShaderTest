using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleCube : Cube
{
    [SerializeField] GameObject Decal;

    public override void OnCollisionEnter(Collision collision)
    {
            Instantiate(Decal, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
    }
}
