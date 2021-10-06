using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesolveCube : Cube
{
    private BoxCollider _boxCollider;
    private float _range = 0;
    private Renderer _rend;
    [SerializeField] private float speedAnimation;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _rend = GetComponent<Renderer>();
    }
    public override void OnCollisionEnter(Collision collision)
    {
        _boxCollider.enabled = false;
        StartCoroutine(AnimationCoroutine());


    }

    IEnumerator AnimationCoroutine()
    {
        while (_range<=0.9f)
        {   
            _range = Mathf.Lerp(_range, 1f, speedAnimation);
            _rend.material.SetFloat("_Level", _range);
            yield return new WaitForFixedUpdate();
        }
        _rend.material.SetFloat("_Level", 1f);
      
        yield return new WaitForSeconds(5);
        _boxCollider.enabled = true;
        _range = 0f;
        _rend.material.SetFloat("_Level", 0f);
    }

    
}
