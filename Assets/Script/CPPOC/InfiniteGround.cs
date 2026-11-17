
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class InfiniteGround : MonoBehaviour
{
    public float groundLenghth = 30f;
    public GameObject questions;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trigger"))
        {
            Instantiate(questions, new Vector3(1.99f, 0, 3f), Quaternion.identity);
        }
    }
}
