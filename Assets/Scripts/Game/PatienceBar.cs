using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatienceBar : MonoBehaviour
{
    public GameObject filled;

    [ContextMenu("set")]
    public void setBar()
    {
        //filled.LeanScaleY(0, 3f);
        filled.LeanMoveLocalY(-1f, 3f);
    }

    internal void StartBar(float duration)
    {
        filled.LeanMoveLocalY(-1f, duration);
    }
}
