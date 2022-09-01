using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultMGR : MonoBehaviour
{


    [SerializeField] TextMeshProUGUI resultText;
    private void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        Debug.Log($"ResultMGRのInit()を実行します");

    }

    private void Update()
    {
        if (GameManager.instance.state != GameManager.State.Result) return;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            //とりあえず一回勝敗が決まったら（引き分けであっても）タイトル画面に遷移するようにする
            ReturnToTitle();
        }
    }

    private void Rematch()
    {
        //GameManager.instance.Rematch();
    }
    private void ReturnToTitle()
    {
        GameManager.instance.ReTurnToTitle();
    }
    public void SetResultText(string result)
    {
        resultText.text =  result;
    }
}
