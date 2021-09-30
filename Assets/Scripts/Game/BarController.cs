using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    public GameObject filled;
    [ContextMenu("Set 50%")]
    public void SetBar()
    {
        filled.LeanMoveX(filled.transform.position.x + 4f, 3f);
    }
}
