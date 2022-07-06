using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComp : UIComp
{
    protected override string PkgName
    {
        get { return "Common"; }
    }

    protected override string CompName
    {
        get { return "TestComp"; }
    }
}
