using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMGR : MonoBehaviour
{
    EditingMGR editingMGR;

    [SerializeField] GameObject player1fcty;
    [SerializeField] GameObject player2fcty;

    float amount = 30;

    public void Init()
    {
        Debug.Log($"BattleMGRのInit()を実行します");

        editingMGR = GameManager.instance.editingMGR;
    }

    private void Update()
    {
        if (GameManager.instance.state != GameManager.State.Battling) return;


        if (Input.GetKeyDown(KeyCode.R))
        {
            LaunchingBox();
        }
    }

    private void LaunchingBox()
    {
        for(int i = 0; i < player1fcty.transform.childCount; i++)
        {
            player1fcty.transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * amount,ForceMode2D.Impulse);
        }
    }


}
