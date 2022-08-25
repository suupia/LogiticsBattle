using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMGR : MonoBehaviour
{
    [SerializeField] float speed; //箱の移動の速さ
    GameObject selectedBox;
    int selectedIndex;

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

    private void OnEnable()
    {
        selectedIndex = 0;
        selectedBox = EditingMGR.instance.GetBoxFromWarehouse()[0];
    }

    private void Update()
    {
        if (GameManager.instance.state != GameManager.State.Editing) return;

        if(_step == Step.Selecting)
        {
            Selecting();
        }
        else if (_step == Step.Placing)
        {
            Move();

        }
        else
        {
            Debug.LogError($"InputMGRのstepが予期せぬ値になっています step:{step}");
        }

    }

    private void Selecting()
    {
        //右
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log($"右キーが押されました");
            if (selectedIndex == EditingMGR.instance.GetBoxFromWarehouse().Length - 1)
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
            Debug.Log($"左キーが押されました");

            if (selectedIndex == 0)
            {
                selectedIndex = EditingMGR.instance.GetBoxFromWarehouse().Length - 1;
            }
            else
            {
                selectedIndex--;

            }
            SelectingBoxByIndex(selectedIndex);

        }


    }

    private void SelectingBoxByIndex(int index)
    {
        selectedBox.GetComponent<SpriteRenderer>().color = Color.white;

        selectedBox = EditingMGR.instance.GetBoxFromWarehouse()[index];
        selectedBox.GetComponent<SpriteRenderer>().color = Color.red;

    }


    private void Move()
    {
        float hInput = Input.GetAxisRaw("Horizontal");

        if (hInput != 0)
        {
            //Debug.Log("移動キーが入力されました。");

            if (CanMove(hInput))
            {
                transform.position += new Vector3(hInput * speed, 0);

            }
            else
            {
                //Debug.Log($"GanMove({horizontalInput},{verticalInput}はfalseです)");
                return;
            }
        }
    }

    private bool CanMove(float hInput)
    {
        return true;
    }
}
