using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMGR : MonoBehaviour
{
    EditingMGR editingMGR;

    bool isFristInit = true;

    [SerializeField] PlayerNum pNum;

    public enum PlayerNum
    {
        p1,p2
    }

    //全体
    GameObject[] boxFromWarehouse;

    //Selecting
    GameObject selectedBox;
    [SerializeField] int listIndex;
    List<int> notSelectedIndexes; //まだ選ばれていない箱のインデックス


    //Moving
    [SerializeField] GameObject factory;
    GameObject boxPlaced; //Factoryにおいて操作するゲームオブジェクト
    [SerializeField] float speed; //箱の移動の速さ
    float p1LeftEndPos = -7;
    float p1RightEndPos = -1;
    float p2LeftEndPos = 1;
    float p2RightEndPos = 7;

    //Placing

    //初期化フラグ
    bool isFirstSelecting;
    bool isFirstMoving;
    bool isFirstPlacing;

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

        if (isFristInit)
        {
            editingMGR = GameManager.instance.editingMGR;
            notSelectedIndexes = new List<int>();

            isFristInit = false;
        }

        //初期化フラグの初期化
        isFirstSelecting = true;
        isFirstMoving = true;
        isFirstPlacing = true;

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


    }

    public void Fe() //finalize
    {

        //倉庫の箱を削除する
        editingMGR.DestroyBoxFromWarehouse();

        //配置した箱を削除する
        for (int i = 0; i < factory.transform.childCount; i++)
        {
            Destroy(factory.transform.GetChild(i).gameObject);
        }



        //ステップを戻す
        _step = Step.Selecting;

        //リストを削除
        notSelectedIndexes.Clear();
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
            editingMGR.FinishEditing(pNum);
        }
        else
        {
            Debug.LogError($"InputMGRのstepが予期せぬ値になっています step:{step}");
        }

        //デバッグ
        if (Input.GetKeyDown(KeyCode.Alpha1)) Debug.LogWarning($"{string.Join(",", notSelectedIndexes)}");



    }

    //Selecting
    private void FirstSelecting()
    {
        isFirstSelecting = false;
        selectedBox = boxFromWarehouse[notSelectedIndexes[listIndex]];

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
            Debug.Log($"boxFromWarehouse.Length:{boxFromWarehouse.Length}, listIndex:{listIndex}, notSelectedIndexes:{string.Join(",", notSelectedIndexes)}");
            //Debug.Log($"boxFromWarehouse[{i}]:{boxFromWarehouse[i]}");

            if (notSelectedIndexes.Count != 0 && i == notSelectedIndexes[listIndex]) //リストの要素があることの確認が必要
            {
                Debug.Log($"緑色にします");
                boxFromWarehouse[i].GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (notSelectedIndexes.Contains(i))
            {
                Debug.Log($"白色にします");
                boxFromWarehouse[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                Debug.Log($"灰色にします");
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

        //回転
        if (Input.GetKeyDown(RotateKeyCode()))
        {
            RotateBox();
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
        if(pNum == PlayerNum.p1)
        {
            if (hInput <0 && boxPlaced.transform.position.x <= p1LeftEndPos)
            {
                return false;
            }else if (hInput > 0 && boxPlaced.transform.position.x >= p1RightEndPos)
            {
                return false;
            }
            else
            {
                return true;
            }
        }else if(pNum == PlayerNum.p2)
        {
            if (hInput < 0 && boxPlaced.transform.position.x <= p2LeftEndPos)
            {
                return false;
            }
            else if (hInput > 0 && boxPlaced.transform.position.x >= p2RightEndPos)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Debug.LogError($"pNumが予期せぬ値になっています pNum:{pNum}");
            return false;
        }
    }

    private void RotateBox()
    {
        //反時計回りに90度回転
        boxPlaced.transform.Rotate(new Vector3(0,0,90));
    }


    //Placing
    private void FristPlacing()
    {
    }

    private void Placing()
    {
        boxPlaced.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        boxPlaced.GetComponent<Rigidbody2D>().mass = selectedBox.GetComponent<Box>().mass;

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
    private KeyCode RotateKeyCode()
    {
        if (pNum == PlayerNum.p1)
        {
            return KeyCode.Q;
        }
        else if (pNum == PlayerNum.p2)
        {
            return KeyCode.U;
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
