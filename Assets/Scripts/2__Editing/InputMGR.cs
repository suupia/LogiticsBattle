using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMGR : MonoBehaviour
{
    EditingMGR editingMGR;

    //Selecting
    GameObject selectedBox;
    int selectedIndex;


    //Placing
    [SerializeField] GameObject factory;
    GameObject boxPlaced; //Factoryにおいて操作するゲームオブジェクト
    GameObject boxAtDropPoint; //FactoryにおいてboxPlacedの落下地点を表示するゲームオブジェクト
    [SerializeField] float speed; //箱の移動の速さ

    //初期化フラグ
    bool isFirstSelecting = true;
    bool isFirstPlacing = true;
    bool isFirstSelectingBoxByIndex = true;

    public enum Step
    {
        Selecting,
        Placing
    }
    [SerializeField] private Step _step; //デバッグ用

    public Step step
    {
        get { return _step; }
    }


    public void Init()
    {
        Debug.Log($"InputMGRのInit()を実行します");

        editingMGR = GameManager.instance.editingMGR;
        selectedIndex = 0;
        SelectingBoxByIndex(0);
    }

    private void Update()
    {
        if (GameManager.instance.state != GameManager.State.Editing) return;

        if(_step == Step.Selecting)
        {
            if(isFirstSelecting)FirstSelecting();
            Selecting();
        }
        else if (_step == Step.Placing)
        {
            if(isFirstPlacing)FirstPlacing();
            Placing();

        }
        else
        {
            Debug.LogError($"InputMGRのstepが予期せぬ値になっています step:{step}");
        }

    }
    private void FirstSelecting()
    {
        isFirstSelecting = false;
    }
    private void Selecting()
    {
        //右
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log($"Selectingにおいて、右キーが押されました");
            if (selectedIndex == editingMGR.GetBoxFromWarehouse().Length - 1)
            {
                selectedIndex = 0;
            }
            else
            {
                selectedIndex++;

            }
            SelectingBoxByIndex(selectedIndex);
        }

        //左
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log($"Selectingにおいて、左キーが押されました");
            if (selectedIndex == 0)
            {
                selectedIndex = editingMGR.GetBoxFromWarehouse().Length - 1;
            }
            else
            {
                selectedIndex--;

            }
            SelectingBoxByIndex(selectedIndex);
        }

        //決定
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Selectingにおいて、決定キーが押されました");
            _step = Step.Placing;
        }


    }

    private void SelectingBoxByIndex(int index)
    {
        if (isFirstSelectingBoxByIndex)
        {
            selectedBox = editingMGR.GetBoxFromWarehouse()[index];
            selectedBox.GetComponent<SpriteRenderer>().color = Color.green;
            isFirstSelectingBoxByIndex = false;
        }
        else
        {
            selectedBox.GetComponent<SpriteRenderer>().color = Color.white;
            selectedBox = editingMGR.GetBoxFromWarehouse()[index];
            selectedBox.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void FirstPlacing()
    {
        boxPlaced = Instantiate(selectedBox);
        boxPlaced.name = "BoxPlaced";
        boxPlaced.transform.parent = factory.transform;
        boxPlaced.transform.position = new Vector2(-4.5f,4);
        boxPlaced.GetComponent<SpriteRenderer>().color = Color.white;

        boxAtDropPoint = Instantiate(selectedBox);
        boxAtDropPoint.name = "BoxAtDropPoint";
        boxAtDropPoint.transform.parent = factory.transform;
        boxAtDropPoint.transform.position = new Vector2(boxPlaced.transform.position.x,0);
        boxAtDropPoint.GetComponent<SpriteRenderer>().color = Color.white;
        boxAtDropPoint.AddComponent<DropMGR>();



        boxPlaced.SetActive(true);
        boxAtDropPoint.SetActive(true);

        isFirstPlacing = false;
    }

    private void Placing()
    {
        float hInput = Input.GetAxisRaw("Horizontal");

        //移動
        if (hInput != 0)
        {
            Debug.Log("Placingにおいて、移動キーが入力されました。");

            MoveBox(hInput);
        }

        //配置

    }

    private void MoveBox(float hInput)
    {
        if (CanMove(hInput))
        {
            boxPlaced.transform.position += new Vector3(hInput * speed, 0);
            boxAtDropPoint.transform.position += new Vector3(hInput * speed, 0);

        }
        else
        {
            //Debug.Log($"GanMove({horizontalInput},{verticalInput}はfalseです)");
            return;
        }
    }

    private bool CanMove(float hInput)
    {
        return true;
    }



    private void PlaceBox()
    {

    }
}
