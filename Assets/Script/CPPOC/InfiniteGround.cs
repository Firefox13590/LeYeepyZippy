
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class InfiniteGround : MonoBehaviour
{
    public float groundLenghth = 30f;
    public GameObject sol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trigger"))
        {
            Instantiate(sol, new Vector3(0, 0, 0),Quaternion.identity);
        }
    }
}
