using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    private Vector3 parent;

    private void Start()
    {
        parent = transform.parent.position;
    }

    private void Update()
    {
        transform.RotateAround(parent, Vector3.up, 20 * Time.deltaTime);
    }
}
