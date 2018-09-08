using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameManager : MonoBehaviour
{
    protected GameDirector mGameDirector;

    [SerializeField]
    private Transform mCameraControllerPrefab;

    protected abstract void OnStart();
    protected abstract void OnUpdate();

    void Start ()
    {
        mGameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        Instantiate(mCameraControllerPrefab);

        OnStart();
    }

    void Update ()
    {
        OnUpdate();
	}

}
