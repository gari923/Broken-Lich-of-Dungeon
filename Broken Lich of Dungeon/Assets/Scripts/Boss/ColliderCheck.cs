using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public Collider playerCollider;
    public bool colliderCheck = false;

    void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            print("enter");
            colliderCheck = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other == playerCollider)
        {
            print("stay");
            colliderCheck = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == playerCollider)
        {
            print("exit");
            colliderCheck = false;
        }
    }
}
