using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMGR : MonoBehaviour
{
    [SerializeField] GameObject player1fcty;
    [SerializeField] GameObject player2fcty;

    float amount = 30;

    float waitTime = 2;

    bool isFirstLaunchBattle;

    private void OnEnable()
    {
        EnableInit();
    }



    public void EnableInit()
    {
        Debug.Log($"BattleMGRのInit()を実行します");
        isFirstLaunchBattle = true;
    }

    private void Update()
    {
        if (GameManager.instance.state != GameManager.State.Battling) return;


        if (Input.GetKeyDown(KeyCode.R) && isFirstLaunchBattle)
        {
            LaunchingBox();
        }
    }

    private void LaunchingBox()
    {
        isFirstLaunchBattle = false;

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

        GameManager.instance.editingMGR.SwitchFriction(true);

        Judge();
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

        string result;
        if (p1Point > p2Point)
        {
            result = $"Player1 Wins !\n(p1:{p1Point}, p2:{p2Point})";
            Debug.Log($"Player1 Wins ! (p1:{p1Point}, p2:{p2Point})");

        }else if(p1Point < p2Point)
        {
            result = $"Player2 Wins !\n(p1:{p1Point}, p2:{p2Point})";
            Debug.Log($"Player2 Wins ! (p1:{p1Point}, p2:{p2Point})");
        }
        else
        {
            result = $"Draw\n(p1:{p1Point}, p2:{p2Point})";
            Debug.Log($"Draw (p1:{p1Point}, p2:{p2Point})");

        }
        GameManager.instance.resultMGR.SetResultText(result);
        Debug.Log(result);

        GameManager.instance.Result();


    }
}
