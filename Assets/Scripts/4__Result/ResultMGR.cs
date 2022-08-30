using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultMGR : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    private void Update()
    {
    }
    public void SetResultText(string result)
    {
        resultText.text =  result;
    }
}
