using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    //Script para el encendido y apagado de un point light

    Light light;
    public bool lightEnable;
    public float minWaitTime;
    public float maxWaitTime;

    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(WaitForLight());
    }

    IEnumerator WaitForLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            light.enabled = !light.enabled;
            lightEnable = !lightEnable;
        }
    }
}
