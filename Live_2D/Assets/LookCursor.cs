using System;
using UnityEngine;

public class LookCursor : MonoBehaviour
{
    private Transform target;

    private void Start()
    {
        target = GameObject.Find("Target").transform;
    }

    private void Update()
    {
        target.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
