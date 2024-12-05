using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Motion;
using Live2D.Cubism.Framework.MotionFade;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
CubismModel
  └── Parameters (CubismParameter[])
        └── lookupTable (Dictionary<string, CubismParameter>)

CubismFadeController
  └── CubismFadeMotionList
        └── CubismFadeMotionObjects (CubismFadeMotionData[])
              ├── ParameterIds (string[])
              └── ParameterCurves (AnimationCurve[])

motionList (List<Motion>)
  └── Motion
        ├── Parameters (CubismParameter[])
        └── Curves (AnimationCurve[])
*/

public class MotionHotkey : MonoBehaviour
{
    private CubismModel model;
    public CubismParameter[] parameters;
    public Dictionary<string, CubismParameter> lookupTable = new();
    
    private CubismFadeController fadeController;
    
    [Serializable]
    public struct Motion
    {
        public CubismParameter[] Parameters;
        public AnimationCurve[] Curves;
    }
    
    public List<Motion> motionList;

    private void Awake()
    {
        fadeController = transform.GetComponent<CubismFadeController>();
    }

    private void Start()
    {
        if (!TryGetComponent(out model)) return;
        
        parameters = model.Parameters;
        foreach (var para in parameters)
        {
            lookupTable.Add(para.Id, para);
        }
            
        int length = fadeController.CubismFadeMotionList.CubismFadeMotionObjects.Length;
            
        motionList = new List<Motion>();
        
        for (int i = 0; i < length; i++)
        {
            CubismFadeMotionData data = fadeController.CubismFadeMotionList.CubismFadeMotionObjects[i];
            int paramerterCount = data.ParameterIds.Length;
            
            Motion motion = new Motion
            {
                Parameters = new CubismParameter[paramerterCount],
                Curves = new AnimationCurve[paramerterCount]
            };
            
            for (int j = 0; j < paramerterCount; j++)
            {
                string id = data.ParameterIds[j];
                AnimationCurve curve = data.ParameterCurves[j];
                if (!lookupTable.TryGetValue(id, out CubismParameter para) || para == null) continue;
                motion.Parameters[j] = para;
                motion.Curves[j] = curve;
            }
            
            motionList.Add(motion);
        }
    }

    public void PlayMotion(int index)
    {
        if (index < 0 || index >= motionList.Count) return;
        
        StopAllCoroutines();
        
        Motion motion = motionList[index];
        for (int i = 0; i < motion.Parameters.Length; i++)
        {
            StartCoroutine(PlayMotion(motion.Parameters[i], motion.Curves[i]));
        }
    }

    public IEnumerator PlayMotion(CubismParameter parameter,AnimationCurve curve)
    {
        float t = 0;
        while (t <= curve.length)
        {
            float value = curve.Evaluate(t);
            parameter.Value = value;
            print($"{parameter.Id}:{value}");
            t += Time.deltaTime;
            yield return null;
        }
    }
}