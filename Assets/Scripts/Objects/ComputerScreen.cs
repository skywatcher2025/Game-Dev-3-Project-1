using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ComputerScreen : MonoBehaviour, IHittable
{
    [Header("Light Cutout Parameters")]
    [Range(1f, 5f)][SerializeField] private float _lightCutoutTimeMax;
    
    //Rigidbody2D _rigidBody;
    private Flash _flash;
    private Light2D _light;

    // Start is called before the first frame update
    void Start()
    {
        //_rigidBody = GetComponent<Rigidbody2D>();
        _flash = GetComponent<Flash>();
        _light = GetComponentInChildren<Light2D>();
    }

    public void TakeHit()
    {
        StartCoroutine(HandleLightCutout());
        _flash.StartFlash();
    }

    IEnumerator HandleLightCutout()
    {
        _light.enabled = false;
        float cutoutTime = Random.Range(0, _lightCutoutTimeMax);
        yield return new WaitForSeconds(cutoutTime);
        _light.enabled = true;
    }
}
