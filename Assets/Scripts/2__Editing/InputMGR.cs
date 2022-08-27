using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMGR : MonoBehaviour
{
    EditingMGR editingMGR;

    //Selecting
    GameObject selectedBox;
    [SerializeField]int selectedIndex; //デバッグようにserialize
    List<int> placedIndexes; //Placeした箱のインデックスを保持しておく
    [SerializeField] int listIndex;
    List<int> notSelectedIndexes; //まだ選ばれていない箱のインデックス


    //Moving
    [SerializeField] GameObject factory;
    GameObject boxPlaced; //Factoryにおいて操作するゲームオブジェクト
    [SerializeField] float speed; //箱の移動の速さ

    //Placing

    //初期化フラグ
    bool isFirstSelecting = true;
    bool isFirstMoving = true;
    bool isFirstPlacing = true;
    bool isFirstSelectingBoxByIndex = true;

    public enum Step
    {
        Selecting,
        Moving,
        Placing,
        Finish

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
        placedIndexes = new List<int>();
        notSelectedIndexes = new List<int>();
        for(int i = 0; i < editingMGR.GetBoxFromWarehouse().Length; i++)
        {
            notSelectedIndexes.Add(i);
        }
        listIndex = 0;
        SelectingBoxByIndex(0,0);
    }

    private void Update()
    {
        if (GameManager.instance.state != GameManager.State.Editing) return;

        if (_step == Step.Selecting)
        {
            if (isFirstSelecting) FirstSelecting();
            Selecting();
        }
        else if (_step == Step.Moving)
        {
            if (isFirstMoving) FirstMoving();
            Moving();

        }
        else if (_step == Step.Placing)
        {
            if (isFirstPlacing) FristPlacing();
            Placing();
        }
        else
        {
            Debug.LogError($"InputMGRのstepが予期せぬ値になっています step:{step}");
        }

        //デバッグ
        if(Input.GetKeyDown(KeyCode.Alpha0)) Debug.LogWarning($"{string.Join(",", placedIndexes)}");
        if (Input.GetKeyDown(KeyCode.Alpha1)) Debug.LogWarning($"{string.Join(",", notSelectedIndexes)}");



    }

    //Selecting
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

            int preIndex = listIndex;

            listIndex++;

            if(listIndex >= notSelectedIndexes.Count)
            {
                listIndex = 0;
            }

            SelectingBoxByIndex(preIndex,notSelectedIndexes[listIndex]);
        }

        //左
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log($"Selectingにおいて、左キーが押されました");

            int preIndex = listIndex;

            listIndex--;

            if (listIndex <= -1)
            {
                listIndex = notSelectedIndexes.Count-1;
            }

            SelectingBoxByIndex(preIndex,notSelectedIndexes[listIndex]);
        }

        //決定
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Selectingにおいて、決定キーが押されました");
            _step = Step.Moving;
            isFirstSelecting = true;
        }


    }

    private void SelectingBoxByIndex(int preIndex, int postIndex)
    {
        if (isFirstSelectingBoxByIndex)
        {
            selectedBox = editingMGR.GetBoxFromWarehouse()[postIndex];
            selectedBox.GetComponent<SpriteRenderer>().color = Color.green;
            isFirstSelectingBoxByIndex = false;
        }
        else
        {
            if (notSelectedIndexes.Contains(preIndex))
            {
                selectedBox.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                selectedBox.GetComponent<SpriteRenderer>().color = Color.gray;
            }
            selectedBox = editingMGR.GetBoxFromWarehouse()[postIndex];
            selectedBox.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }


    //Moving
    private void FirstMoving()
    {
        boxPlaced = Instantiate(selectedBox);
        boxPlaced.name = "BoxPlaced";
        boxPlaced.transform.parent = factory.transform;
        boxPlaced.transform.position = new Vector2(-4.5f, 4);
        boxPlaced.GetComponent<SpriteRenderer>().color = Color.white;


        boxPlaced.SetActive(true);

        isFirstMoving = false;
    }

    private void Moving()
    {
        float hInput = Input.GetAxisRaw("Horizontal");

        //移動
        if (hInput != 0)
        {
            Debug.Log("Movingにおいて、移動キーが入力されました。");

            MoveBox(hInput);
        }

        //配置
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Movingにおいて、決定キーが押されました");
            _step = Step.Placing;
            isFirstMoving = true;
        }

    }

    private void MoveBox(float hInput)
    {
        if (CanMove(hInput))
        {
            boxPlaced.transform.position += new Vector3(hInput * speed, 0);

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


    //Placing
    private void FristPlacing()
    {
        boxPlaced.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
    private void Placing()
    {
        selectedBox = editingMGR.GetBoxFromWarehouse()[notSelectedIndexes[listIndex]];
        selectedBox.GetComponent<SpriteRenderer>().color = Color.gray;
        notSelectedIndexes.Remove(notSelectedIndexes[listIndex]);

        if(notSelectedIndexes.Count == 0)
        {
            Debug.Log($"すべての箱を配置し終わりました");
            _step = Step.Finish;
        }
        _step = Step.Selecting;
        isFirstPlacing = true;
        
    }

    private void PlacingBoxByIndex(int index)
    {


        
    }
}
