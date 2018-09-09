using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameManager : MonoBehaviour
{
    protected GameDirector mGameDirector;

    [SerializeField]
    private Transform mCameraControllerPrefab;

    [SerializeField]
    private Transform mDebugCameraControllerPrefab;

    protected abstract void OnStart();
    protected abstract void OnUpdate();

    void Start ()
    {
        mGameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

#if UNITY_EDITOR
        if (mDebugCameraControllerPrefab)
        {
            Instantiate(mDebugCameraControllerPrefab);
        }
        else
        {
            Instantiate(mCameraControllerPrefab);
        }
#else
        Instantiate(mCameraControllerPrefab);
#endif

        OnStart();
    }

    void Update ()
    {
        OnUpdate();
	}

}
