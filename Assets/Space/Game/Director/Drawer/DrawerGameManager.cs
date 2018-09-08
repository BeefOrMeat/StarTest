using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerGameManager : BaseGameManager
{
	protected override void OnStart ()
    {
        //-1が出ないようにする為だけど他の回避方法でも別にいい
        int seed = Random.Range(0, int.MaxValue);
        mGameDirector.SetRandomSeed(seed);
	}
	
	protected override void OnUpdate ()
    {

	}
}
