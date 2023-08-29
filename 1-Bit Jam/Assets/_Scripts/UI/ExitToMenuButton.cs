using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitToMenuButton : UIButtonBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnClick()
    {
        base.OnClick();

        Debug.Log("Exit to Menu");
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
}
