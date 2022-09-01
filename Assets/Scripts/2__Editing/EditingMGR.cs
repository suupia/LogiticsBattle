using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditingMGR : MonoBehaviour
{
    [SerializeField] public InputMGR p1InputMGR;
    [SerializeField] public InputMGR p2InputMGR;

    [SerializeField] public BattleMGR battleMGR;


    public bool isInitialized = false;

    bool isFristInit = true;

    [SerializeField] GameObject boxParentInWarehouse;

    [SerializeField] GameObject boxPrefab1;
    [SerializeField] GameObject boxPrefab2;
    [SerializeField] GameObject boxPrefab3;

    [SerializeField]GameObject ground;

    GameObject[] p1warehouses;
    GameObject[] p2warehouses;

    float halfOfScreen = 9;

    //フラグ
    bool isP1Finished;
    bool isP2Finished;

    private void OnEnable()
    {
        EnableInit();
    }

    private void OnDisable()
    {
        DisableInit();
    }
    public void EnableInit()
    {
        Debug.Log($"EditingMGRのEnableInit()を実行します");

        if (isFristInit)
        {
            //今はゲームで一回のみ処理する内容はない
        }

        //フラグの初期化
        isP1Finished = false;
        isP2Finished = false;


        //エクセルから倉庫にある箱の数を読み込む
        p1warehouses = new GameObject[3];
        p2warehouses = new GameObject[3];


        //エクセルから倉庫にある箱の種類を読み込み、インスタンス化する（しないとプレハブを直接参照することになってしまう）
        p1warehouses[0] = Instantiate(boxPrefab1, boxParentInWarehouse.transform);
        p1warehouses[1] = Instantiate(boxPrefab2, boxParentInWarehouse.transform);
        p1warehouses[2] = Instantiate(boxPrefab3, boxParentInWarehouse.transform);

        p2warehouses[0] = Instantiate(boxPrefab1, boxParentInWarehouse.transform);
        p2warehouses[1] = Instantiate(boxPrefab2, boxParentInWarehouse.transform);
        p2warehouses[2] = Instantiate(boxPrefab3, boxParentInWarehouse.transform);

        //倉庫に表示する
        DisplayingBoxesInWarehouse(0);
        DisplayingBoxesInWarehouse(1);
        DisplayingBoxesInWarehouse(2);

        //アタッチされているスクリプトの初期化
        p1InputMGR.Init();
        p2InputMGR.Init();


        //Groundの摩擦をつける
        SwitchFriction(true);

        isInitialized = true;

    }

    public void DisableInit()
    {
        Debug.Log($"EditingMGRのDisableInit()を実行します");


        //フラグの初期化
        isP1Finished = false;
        isP2Finished = false;

        //書くInputMGRの初期化
        p1InputMGR.Fe();
        p2InputMGR.Fe();
    }

    private void DisplayingBoxesInWarehouse(int index)
    {

        Vector2 anchorPos = new Vector2(-8, -4);
        int hMargin = 2;

        //Player1
        p1warehouses[index].transform.localPosition = anchorPos + new Vector2(hMargin * index, 0);

        //Player2
        p2warehouses[index].transform.localPosition = new Vector2(halfOfScreen,0) + anchorPos + new Vector2(hMargin * index, 0);

    }


    public void SwitchFriction(bool isOn)
    {
        if (isOn)
        {
            ground.GetComponent<BoxCollider2D>().sharedMaterial.friction = 1f;
            Debug.Log($"OnFriction value:{ground.GetComponent<BoxCollider2D>().sharedMaterial.friction}");
        }
        else
        {
            ground.GetComponent<BoxCollider2D>().sharedMaterial.friction = 0;
            Debug.Log($"OffFriction value:{ground.GetComponent<BoxCollider2D>().sharedMaterial.friction}");

        }
    }

    public void FinishEditing(InputMGR.PlayerNum pNum)
    {
        if (pNum == InputMGR.PlayerNum.p1)
        {
            isP1Finished = true;
        }
        else if (pNum == InputMGR.PlayerNum.p2)
        {
            isP2Finished = true;
        }
        else
        {
            Debug.LogError($"FinishEditingのpNumが予期せぬ値になっています pNum:{pNum}");
        }

        if (isP1Finished && isP2Finished)
        {
            SwitchFriction(false);
            GameManager.instance.Battling();
        }
    }


    public void DestroyBoxFromWarehouse()
    {
        for(int i = 0; i < p1warehouses.Length; i++)
        {
            Destroy(p1warehouses[i]);
        }

        for (int i = 0; i < p2warehouses.Length; i++)
        {
            Destroy(p2warehouses[i]);
        }
    }

    //Getter
    public GameObject[] GetBoxFromP1Warehouse()
    {
        return p1warehouses;
    }
    public GameObject[] GetBoxFromP2Warehouse()
    {
        return p2warehouses;
    }
    public float GetHalfOfScreen()
    {
        return halfOfScreen;
    }
}
