using UnityEngine;

public class KeyController : MonoBehaviour
{
    //Script en escena 2
    //Script en llave final luego de derrotar al jefe final
    SphereCollider sphere;
    void Start()
    {
        sphere = GetComponent<SphereCollider>();
    }

    void Update()
    {
        transform.Rotate(1, 1, 0);
    }

    public void ColliderEnable()
    {
        sphere.enabled = true;
    }
}
