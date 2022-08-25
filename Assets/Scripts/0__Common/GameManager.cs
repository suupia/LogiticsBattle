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


    public enum State
    {
        Title,
        Editing,
        Battleing,
        Result
    }
    [SerializeField] private State _state; //デバッグ用
    public State state
    {
        get { return _state; }
    }

    private void Awake()
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
        _state = State.Editing;
    }

}
