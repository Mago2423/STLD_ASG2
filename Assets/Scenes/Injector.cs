using System;
using UnityEngine;

public class Injector : MonoBehaviour

{
    public void OnTriggerEnter(Collider collision)
    {
        print($"Collected {collision.gameObject.name}");

    }
}