using System;
using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Framework.Expression;
using UnityEngine;

public class ExpressionHotkey : MonoBehaviour
{
    public CubismExpressionController _controller;
    
    [Serializable]
    public struct ExpressionMapping //熱鍵設定
    {
        public KeyCode key; //鍵位
        public int expressionIndex; //表情號碼
        public string name; //表情名子
    }

    public List<ExpressionMapping> _mappings; // 熱鍵列表
        
    void Start()
    {
        for (int i = 0; i < _controller.ExpressionsList.CubismExpressionObjects.Length; i++)
        {
            print($"{i} : {_controller.ExpressionsList.CubismExpressionObjects[i].name}");
        }
        //Keycode -> expressionIndex(int)
        _mappings = new List<ExpressionMapping>();
        int expressionLength = _controller.ExpressionsList.CubismExpressionObjects.Length;
        for (int i = 0; i < expressionLength; i++)
        {
            KeyCode keycode = KeyCode.Alpha0 + i; //Alpha0, Alpha1, Alpha2......
            _mappings.Add(new ExpressionMapping()
            {
                key = keycode, //Alpha0, Alpha1, Alpha2......
                expressionIndex = i, //表情號碼0, 1, 2......
                name = _controller.ExpressionsList.CubismExpressionObjects[i].name
            });
        }

    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _mappings.Count; i++)
        {
            if (Input.GetKey(_mappings[i].key))
            {
                _controller.CurrentExpressionIndex = _mappings[i].expressionIndex;
            }
        }
    }
}