using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexSphere : Cube
{
    private SphereCollider _sphereCollider;
    private float _range = 0;
    private Renderer _rend;
    [SerializeField] private float speedAnimation;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _rend = GetComponent<Renderer>();
    }
    public override void OnCollisionEnter(Collision collision)
    {
        _sphereCollider.enabled = false;
        StartCoroutine(AnimationCoroutine());


    }

    IEnumerator AnimationCoroutine()
    {
        while (_range <= 0.5f)
        {
            _range = Mathf.Lerp(_range, 1f, speedAnimation);
            _rend.material.SetFloat("_ExtrusionFactor", _range);
            yield return new WaitForFixedUpdate();
        }
        

        yield return new WaitForSeconds(5);
        _sphereCollider.enabled = true;
        _range = 0f;
        _rend.material.SetFloat("_ExtrusionFactor", 0f);
    }
}
