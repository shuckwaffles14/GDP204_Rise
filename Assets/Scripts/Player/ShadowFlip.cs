using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFlip : MonoBehaviour
{
    [SerializeField] private Transform _shadowCaster;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float xScale = _renderer.flipX ? -1 : 1;
        Vector3 newScale = new Vector3(xScale, _shadowCaster.localScale.y, _shadowCaster.localScale.z);
        _shadowCaster.localScale = newScale;
    }
}
