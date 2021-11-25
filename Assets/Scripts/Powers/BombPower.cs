using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPower : PowerBase
{
    void Start()
    {
        Activator.onClick.AddListener(delegate { Select(); });
    }

    public override void Select()
    {
        base.Select();
    }
}
