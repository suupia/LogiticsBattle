using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMGR : MonoBehaviour
{
    EditingMGR editingMGR;

    [SerializeField] PlayerNum pNum;

    public enum PlayerNum
    {
        p1,p2
    }

    //全体
    [SerializeField] GameObject ground;
    GameObject[] boxFromWarehouse;

    //Selecting
    GameObject selectedBox;
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
        notSelectedIndexes = new List<int>();
        if (pNum == PlayerNum.p1)
        {
            boxFromWarehouse = editingMGR.GetBoxFromP1Warehouse();
        }
        else if (pNum == PlayerNum.p2)
        {
            boxFromWarehouse = editingMGR.GetBoxFromP2Warehouse();
        }
        else
        {
            Debug.LogError($"pNumが予期せぬ値になっています pNum:{pNum}");
        }

        for (int i = 0; i < boxFromWarehouse.Length; i++)
        {
            notSelectedIndexes.Add(i);
        }
        listIndex = 0;

        ChangeBoxColor();

        //Groundの摩擦をつける
        SwitchFriction(true);
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
        else if (_step == Step.Finish)
        {
            //Groundの摩擦を0にする
            SwitchFriction(false);
            GameManager.instance.Battling();
        }
        else
        {
            Debug.LogError($"InputMGRのstepが予期せぬ値になっています step:{step}");
        }

        //デバッグ
        //if(Input.GetKeyDown(KeyCode.Alpha0)) Debug.LogWarning($"{string.Join(",", placedIndexes)}");
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
        if (Input.GetKeyDown(RightKeyCode()))
        {
            Debug.Log($"Selectingにおいて、右キーが押されました");

            listIndex++;

            if (listIndex >= notSelectedIndexes.Count)
            {
                listIndex = 0;
            }

            selectedBox = boxFromWarehouse[notSelectedIndexes[listIndex]];

            ChangeBoxColor();

        }

        //左
        if (Input.GetKeyDown(LeftKeyCode()))
        {
            Debug.Log($"Selectingにおいて、左キーが押されました");

            listIndex--;

            if (listIndex <= -1)
            {
                listIndex = notSelectedIndexes.Count - 1;
            }

            selectedBox = boxFromWarehouse[notSelectedIndexes[listIndex]];
            ChangeBoxColor();

        }

        //決定
        if (Input.GetKeyDown(DecisionKeyCode()))
        {
            Debug.Log($"Selectingにおいて、決定キーが押されました");
            _step = Step.Moving;
            isFirstSelecting = true;
        }


    }
    private void ChangeBoxColor()
    {
        //Debug.Log($"ChangeBoxColorを実行します");
        for (int i = 0; i < boxFromWarehouse.Length; i++)
        {
            Debug.Log($"boxFromWarehouse.Length:{boxFromWarehouse.Length}, listIndex:{listIndex}");
            if (notSelectedIndexes.Count != 0 && i == notSelectedIndexes[listIndex]) //リストの要素があることの確認が必要
            {
                boxFromWarehouse[i].GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (notSelectedIndexes.Contains(i))
            {
                boxFromWarehouse[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                boxFromWarehouse[i].GetComponent<SpriteRenderer>().color = Color.gray;
            }

        }
    }

    //Moving
    private void FirstMoving()
    {
        boxPlaced = Instantiate(selectedBox);
        boxPlaced.name = "BoxPlaced";
        boxPlaced.transform.parent = factory.transform;
        if (pNum == PlayerNum.p1)
        {
            boxPlaced.transform.position = new Vector2(-4.5f, 4);

        }
        else if (pNum == PlayerNum.p2)
        {
            boxPlaced.transform.position = new Vector2(-4.5f + editingMGR.GetHalfOfScreen(), 4);
        }
        else
        {
            Debug.LogError($"pNumが予期せぬ値になっています pNum:{pNum}");
        }
        boxPlaced.GetComponent<SpriteRenderer>().color = Color.white;


        boxPlaced.SetActive(true);

        isFirstMoving = false;
    }

    private void Moving()
    {
        float hInput = 0;

        if (Input.GetKey(RightKeyCode())) //右
        {
            hInput = 1;
        }
        else if (Input.GetKey(LeftKeyCode())) //左
        {
            hInput = -1;
        }
        else
        {
            hInput = 0;
        }

        //移動
        if (hInput != 0)
        {
            Debug.Log("Movingにおいて、移動キーが入力されました。");

            MoveBox(hInput);
        }

        //配置
        if (Input.GetKeyDown(DecisionKeyCode()))
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
        selectedBox = boxFromWarehouse[notSelectedIndexes[listIndex]];
        notSelectedIndexes.Remove(notSelectedIndexes[listIndex]);
        if (listIndex == notSelectedIndexes.Count) listIndex--; //要素の削除に合わせてイテレータの位置を変える
        ChangeBoxColor();

        if (notSelectedIndexes.Count == 0)
        {
            Debug.Log($"すべての箱を配置し終わりました");
            _step = Step.Finish;
            isFirstPlacing = true;
        }
        else
        {
            _step = Step.Selecting;
            isFirstPlacing = true;
        }


    }



    private void SwitchFriction(bool isOn)
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


    //プレイヤーに依ってキーが異なる
    private KeyCode LeftKeyCode()
    {
        if (pNum == PlayerNum.p1)
        {
            return KeyCode.A;
        }
        else if (pNum == PlayerNum.p2)
        {
            return KeyCode.J;
        }
        else
        {
            Debug.LogError($"pNumが予期せぬ値になっています pNum:{pNum}");
            return KeyCode.Escape;
        }
    }

    private KeyCode RightKeyCode()
    {
        if (pNum == PlayerNum.p1)
        {
            return KeyCode.D;
        }
        else if (pNum == PlayerNum.p2)
        {
            return KeyCode.L;
        }
        else
        {
            Debug.LogError($"pNumが予期せぬ値になっています pNum:{pNum}");
            return KeyCode.Escape;
        }
    }

    private KeyCode DecisionKeyCode()
    {
        if (pNum == PlayerNum.p1)
        {
            return KeyCode.E;
        }
        else if (pNum == PlayerNum.p2)
        {
            return KeyCode.O;
        }
        else
        {
            Debug.LogError($"pNumが予期せぬ値になっています pNum:{pNum}");
            return KeyCode.Escape;
        }
    }
}
