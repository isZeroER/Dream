using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FacingCamera : MonoBehaviour
{
    [SerializeField] private Transform characterRoot;
    [SerializeField] private Transform environmentRoot;
    
    void Update()
    {
        for (int i = 0; i < characterRoot.childCount; i++)
        {
            characterRoot.GetChild(i).GetChild(0).localRotation = Camera.main.transform.rotation;
        }

        for (int i = 0; i < environmentRoot.childCount; i++)
        {
            environmentRoot.GetChild(i).localRotation = Camera.main.transform.rotation;
        }
    }
}
