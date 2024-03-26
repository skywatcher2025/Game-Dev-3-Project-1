using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _parallaxOffest = .9f;

    private Vector2 _startPos;
    private Camera _mainCamera;
    private Vector2 _travel => (Vector2)_mainCamera.transform.position - _startPos; //Only calculates when referenced

    private void Awake()
    {
        _mainCamera = Camera.main; //No longer causes issues (contrary to old documentation)
    }

    void Start()
    {
        _startPos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = _startPos + new Vector2(_travel.x * _parallaxOffest, 0f);
        transform.position = new Vector2(newPosition.x, transform.position.y);
    }
}
