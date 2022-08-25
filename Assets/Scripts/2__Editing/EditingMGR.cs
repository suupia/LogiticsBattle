using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditingMGR : MonoBehaviour
{
    [SerializeField] public InputMGR inputMGR;


    public bool isInitialized = false;

    [SerializeField] GameObject boxParentInWarehouse;

    [SerializeField] GameObject heavy_1x1;
    [SerializeField] GameObject light_1x1;

    GameObject[] warehouses;

    private void OnEnable()
    {
        if (!isInitialized)Init();
    }
    public void Init()
    {
        //エクセルから倉庫にある箱の数を読み込む
        warehouses = new GameObject[3];

        //エクセルから倉庫にある箱の種類を読み込み、インスタンス化する（しないとプレハブを直接参照することになってしまう）
        warehouses[0] = Instantiate(heavy_1x1, boxParentInWarehouse.transform);
        warehouses[1] = Instantiate(light_1x1, boxParentInWarehouse.transform);
        warehouses[2] = Instantiate(light_1x1, boxParentInWarehouse.transform);

        //倉庫に表示する
        DisplayingBoxesInWarehouse(0);
        DisplayingBoxesInWarehouse(1);
        DisplayingBoxesInWarehouse(2);

        //アタッチされているスクリプトの初期化
        inputMGR.Init();

        isInitialized = true;

    }

    private void DisplayingBoxesInWarehouse(int index)
    {

        Vector2 anchorPos = new Vector2(-8, -4);
        int hMargin = 2;

        //GameObject boxInWarehouse = Instantiate(warehouses[index] ,boxParentInWarehouse.transform);
        warehouses[index].transform.localPosition = anchorPos + new Vector2(hMargin * index, 0);
        //primitiveObject.transform.localScale = localScale;

    }




    //Getter
    public GameObject[] GetBoxFromWarehouse()
    {
        return warehouses;
    }

}
