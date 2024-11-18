using System.Collections.Generic;
using Live2D.Cubism.Framework.Expression;
using UnityEngine;

public struct expresionMapping
{
    public KeyCode key;
    public int expressionIndex;
    public string name;
}

public class ExpressionHotKey : MonoBehaviour
{
    private CubismExpressionData[] getExpressionDatas;
    
    private List<expresionMapping> expresionMappings;
    
    private void Start()
    {
        getExpressionDatas = transform.GetComponent<CubismExpressionController>().ExpressionsList.CubismExpressionObjects;
        expresionMappings = new List<expresionMapping>();
        
        for (int i = 0; i < getExpressionDatas.Length; i++)
        {
            expresionMappings.Add(new expresionMapping()
            {
                key =  KeyCode.Alpha0 + i,
                expressionIndex = i,
                name = getExpressionDatas[i].name
            });
        }
    }
    
    private void Update()
    {
        for (int i = 0; i < expresionMappings.Count; i++)
        {
            if (Input.GetKey(expresionMappings[i].key))
            {
                //getExpressionDatas.CurrentExpressionIndex = expresionMappings[i].expressionIndex;
            }
        }
    }
}
