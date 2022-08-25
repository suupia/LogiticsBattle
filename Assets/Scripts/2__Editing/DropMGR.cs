using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class DropMGR : MonoBehaviour
{
    bool isEntered;
    void Start()
    {
        //while (!isEntered) transform.position = new Vector3(transform.position.x, transform.position.y - Mathf.Epsilon, transform.position.z);

        for (int i = 0; i < 1000; i++)
        {
            if (isEntered) break;

            transform.position = new Vector3(transform.position.x, transform.position.y - Mathf.Epsilon, transform.position.z);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isEntered = true;
        Debug.Log("何かにあたりました");
    }

}
