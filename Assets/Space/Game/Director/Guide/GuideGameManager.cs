using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideGameManager : BaseGameManager
{
    protected override void OnStart()
    {
        int seed = Random.Range(0, int.MaxValue);
        mGameDirector.SetRandomSeed(seed);
    }

    protected override void OnUpdate()
    {
        //ランダムシードを受け取ったらmGameDirector.SetRandomSeedを呼び出す
    }
}
