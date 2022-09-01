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
    [SerializeField] public ResultMGR resultMGR;

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
        Debug.Log($"_stateをEditingに移行します");
        isP1Finished = false;
        isP2Finished = false;
        __Editing.SetActive(true);
        _state = State.Editing;
    }

    public void Battling()
    {
        Debug.Log($"_stateをBattlingに移行します");

        __Battleing.SetActive(true );
        _state = State.Battling;
    }

    public void Result()
    {
        Debug.Log($"_stateをResultに移行します");

        __Result.SetActive(true);
        _state = State.Result;
    }



    public void ReTurnToTitle()
    {
        Debug.Log($"ReTurnToTitleを実行します");


        //いったん全部非表示
        __Editing.SetActive(false);
        __Battleing.SetActive(false);
        __Result.SetActive(false);

        Editing();
    }
}
