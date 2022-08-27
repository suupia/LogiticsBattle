using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMGR : MonoBehaviour
{
    EditingMGR editingMGR;

    [SerializeField] GameObject player1fcty;
    [SerializeField] GameObject player2fcty;

    float amount = 30;

    float waitTime = 2;

    InputMGR.PlayerNum winPNum;

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

        for (int i = 0; i < player2fcty.transform.childCount; i++)
        {
            player2fcty.transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * amount, ForceMode2D.Impulse);
        }

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);

        editingMGR.SwitchFriction(true);
    }

    private void Judge()
    {
        int p1Point =0;
        int p2Point=0;

        for (int i = 0; i < player1fcty.transform.childCount; i++)
        {
            if(player1fcty.transform.GetChild(i).gameObject.transform.position.x > 0)
            {
                p1Point++;
            }else if(player1fcty.transform.GetChild(i).gameObject.transform.position.x < 0)
            {
                p2Point++;
            }
            else
            {
                //とりあえず
                p1Point++;
            }
            
        }

        for (int i = 0; i < player2fcty.transform.childCount; i++)
        {
            if (player2fcty.transform.GetChild(i).gameObject.transform.position.x > 0)
            {
                p1Point++;
            }
            else if (player1fcty.transform.GetChild(i).gameObject.transform.position.x < 0)
            {
                p2Point++;
            }
            else
            {
                //とりあえず
                p1Point++;
            }
        }


    }
}
