using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Role
{
    Guide,
    Drawer,
}

public class GlobalData
{
    public static Role role = Role.Guide;
    //public static Role role = Role.Drawer;
}
