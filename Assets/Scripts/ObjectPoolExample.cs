using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolExample : MonoBehaviour
{
    /*
    private ObjectPool<Transform> _transformPool;

    // Start is called before the first frame update
    void Start()
    {
        CreateTransformPool();
    }
    
    void Update()
    {
        Transform transform = _transformPool.Get();
        
        _transformPool.Release(transform);
    }

    void CreateTransformPool()
    {
        _transformPool = new ObjectPool<Transform>(() =>
        {
            //return Instantiate(GameObject); //Instantiate whatever prefab you're using

        }, transform =>
        {
            transform.GameObject().SetActive(true);
        }, transform1 =>
        {
            transform.GameObject().SetActive(false);
        }, transform =>
        {
            Destroy(transform);
        }, false, 20, 40);
    }
    */
}
