﻿using System.Collections.Generic;
using HighPrecisionStepperJuggler.MachineLearning;
using UnityEngine;

public class GradientDescentView : MonoBehaviour
{
    [SerializeField] private GameObject _dataPointPrefab;
    [SerializeField] private GameObject _coordinateSystem;
    [SerializeField] private GameObject _gradientDescentLine;

    public bool DisplayDataOnYAxis;
    public bool IsDisplayingHeightData;

    public GradientDescent GradientDescent
    {
        set => _gradientDescent = value;
    }
    
    private GradientDescent _gradientDescent;
    
    private List<GameObject> _dataPointList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < _gradientDescent.TrainingSets.Length; i++)
        {
            _dataPointList.Add(Instantiate(_dataPointPrefab, _coordinateSystem.transform));
        }
    }

    private void Update()
    {
        var sets = _gradientDescent.TrainingSets;

        var divisor = IsDisplayingHeightData ? 20f : 2f;
        var timeScaler = IsDisplayingHeightData ? 10f : 100f;
        if (DisplayDataOnYAxis)
        {
            for (int i = 0; i < sets.Length; i++)
            {
                _dataPointList[i].transform.localPosition = new Vector3(sets[i].t_1 * timeScaler, sets[i].y / divisor, 0f);
            }
        }
        else
        {
            for (int i = 0; i < sets.Length; i++)
            {
                _dataPointList[i].transform.localPosition = new Vector3(sets[i].y / divisor, sets[i].t_1 * timeScaler, 0f);
            }
        }


        
        var dir = DisplayDataOnYAxis ? Vector3.up : Vector3.right;
        _gradientDescentLine.transform.localPosition = dir * _gradientDescent.Hypothesis.Parameters.Theta_0 / divisor;
        
        var slope = _gradientDescent.Hypothesis.Parameters.Theta_1 / divisor;
        var rotation = DisplayDataOnYAxis 
            ? Quaternion.Euler(0f, 0f, Mathf.Atan2(slope, 100f) * Mathf.Rad2Deg)
            : Quaternion.Euler(0f, 0f, (Mathf.PI / 2f - Mathf.Atan2(slope, 100f)) * Mathf.Rad2Deg);

        _gradientDescentLine.transform.localRotation = rotation;
    }
}
