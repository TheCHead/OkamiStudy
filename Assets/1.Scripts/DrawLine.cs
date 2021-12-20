using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawLine : MonoBehaviour
{
    [SerializeField] private GameObject brushPrefab;
    [SerializeField] private LineRenderer linePrefab;
    [Tooltip("Min distance from last point for the new point to be created.")]
    [SerializeField] private float newPointDiffThreshold = 0.1f;

    private LineRenderer _currentLine;
    private List<Vector3> _pointerPositions = new List<Vector3>();
    private float drawDistanceFromCamera = 5f;
    private GameObject _brush;
    


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = drawDistanceFromCamera;
            Vector3 tempPointerPos = Camera.main.ScreenToWorldPoint(mousePos);
            if (Vector3.Distance(tempPointerPos, _pointerPositions[_pointerPositions.Count - 1]) >
                newPointDiffThreshold)
            {
                UpdateLine(tempPointerPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _brush.SetActive(false);
        }
        
    }

    private void CreateLine()
    {
        _currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        _pointerPositions.Clear();
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = drawDistanceFromCamera;
        Vector3 newLinePos = Camera.main.ScreenToWorldPoint(mousePos);
        _pointerPositions.Add(newLinePos);
        _pointerPositions.Add(newLinePos);
        _currentLine.SetPosition(0, _pointerPositions[0]);
        _currentLine.SetPosition(1, _pointerPositions[1]);
        
        if (_brush != null)
        {
            _brush.SetActive(true);
            _brush.transform.position = mousePos;
        }
        else
        {
            _brush = Instantiate(brushPrefab, mousePos, Quaternion.identity);
        }
    }

    private void UpdateLine(Vector3 newPointerPos)
    {
        _brush.transform.position = newPointerPos;
        _pointerPositions.Add(newPointerPos);
        _currentLine.positionCount++;
        _currentLine.SetPosition(_currentLine.positionCount - 1, newPointerPos);
    }
}
