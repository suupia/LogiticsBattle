using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject __Title;
    [SerializeField] private GameObject __Editing;
    [SerializeField] private GameObject __Battleing;
    [SerializeField] private GameObject __Result;

    [SerializeField] public EditingMGR editingMGR;

    bool isP1Finished;
    bool isP2Finished;

    public enum State
    {
        Title,
        Editing,
        Battling,
        Result
    }
    [SerializeField] private State _state; //デバッグ用
    public State state
    {
        get { return _state; }
    }

    private void Awake()
    {
        Singletonization();
    }

    public void Singletonization() //重複して呼んでも問題ない
    {
        //シングルトン化
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //デバッグ
        Editing();
    }

    public void Editing()
    {
        isP1Finished = false;
        isP2Finished = false;
        __Editing.SetActive(true);
        _state = State.Editing;
    }

    public void Battling()
    {
        __Battleing.SetActive(true );
        _state = State.Battling;
    }

    public void FinishEditing(InputMGR.PlayerNum pNum)
    {
        if(pNum == InputMGR.PlayerNum.p1)
        {
            isP1Finished = true;
        }else if(pNum == InputMGR.PlayerNum.p2)
        {
            isP2Finished = true;
        }
        else
        {
            Debug.LogError($"FinishEditingのpNumが予期せぬ値になっています pNum:{pNum}");
        }

        if (isP1Finished && isP2Finished)
        {
            editingMGR.SwitchFriction(false);
            Battling();
        }
    }
}
