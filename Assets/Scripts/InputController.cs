using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    void Update()
    {
        
#if UNITY_EDITOR
        Vector3 pos = Input.mousePosition;
        
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0))
#else
        if (Input.touchCount != 1)
        {
            return;
        }

        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;
        
        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
#endif
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(pos);
            RaycastHit hit;
 
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<Piece>() != null)
                {
#if UNITY_EDITOR
                    if (Input.GetKeyDown(KeyCode.Mouse0))
#else
                    if (touch.phase == TouchPhase.Began)
#endif
                    {
                        hit.transform.GetComponent<Piece>().SelectUpperPiece();
                    }
                    else
                    {
                        hit.transform.GetComponent<Piece>().SelectOtherUpperPiece();
                    }
                }
            }
        }
    }

}
