using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    private Transform mDrawerGameManagerPrefab;
  
    [SerializeField]
    private Transform mGuideGameManagerPrefab;

    private BaseGameManager mGameManager;

    [SerializeField]
    private Transform mCanvasPrefab;

    [SerializeField]
    private Transform mStarGeneratorPrefab;
    private StarGenerator mStarGenerator;

    private Text mText;

    [SerializeField]
    private float mMaxTime = 30;
    private float mTime;

    void Start ()
    {
        mStarGenerator = Instantiate(mStarGeneratorPrefab).gameObject.GetComponent<StarGenerator>();

        mText = Instantiate(mCanvasPrefab).Find("Text").GetComponent<Text>();
        mTime = mMaxTime;

        if (GlobalData.role == Role.Drawer)
        {
            mGameManager = Instantiate(mDrawerGameManagerPrefab).gameObject.GetComponent<DrawerGameManager>();
        }
        else if (GlobalData.role == Role.Guide)
        {
            mGameManager = Instantiate(mGuideGameManagerPrefab).gameObject.GetComponent<GuideGameManager>();
        }
    }

    void Update ()
    {
        float deltaTime = Time.deltaTime;
        mTime -= deltaTime;
        mText.text = "Time:" + mTime;
        if (mTime <= 0.0f)
        {
            mTime = 0.0f;
        }
    }

    public void SetRandomSeed(int seed)
    {
        mStarGenerator.SetSeed(seed);
    }
}
