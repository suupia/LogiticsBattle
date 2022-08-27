using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropMGR : MonoBehaviour
{

    [SerializeField] LayerMask m_LayerMask;
    float prePosX; //動いたかどうかの判定に必要
    float initPosY = 4.0f; 
    float smallDistance = 0.000001f; //当たり判定のために必要


    void Start()
    {
        m_LayerMask = ~0;
        DropUntilHit();

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode

    //    //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
    //    Gizmos.DrawWireCube(transform.position, transform.localScale);
    //}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    isEntered = true;
    //    Debug.Log("何かにあたりました");
    //}

    private void Update()
    {
        if (Mathf.Approximately(prePosX, transform.position.x)) return;
        //prePosX = transform.position.x;
        //DropUntilHit();
        if (Input.GetKeyDown(KeyCode.U))
        {
            DropUntilHit();
        }
    }

    private void DropUntilHit()
    {
        transform.position = new Vector3(transform.position.x, initPosY, transform.position.z);
        while (!IsHitSomething())
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - smallDistance, transform.position.z);
            Debug.Log($"y座用は{transform.position.y}");
        }

        Debug.Log($"落下の判定が終わりました");

        //Box Colider 2D をオンにして、Rigidbody 2D をDynamicにすると、0.0045程度の誤差が出るがしかたがないとする

        
    }

    private bool IsHitSomething()
    {
        return Physics2D.OverlapBox(gameObject.transform.position, transform.localScale, 0) != null ? true: false;
    }

}
