using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    private Transform mStarGeneratorPrefab;

    [SerializeField]
    private Transform mCanvasPrefab;

    private Text mText;

    [SerializeField]
    private float mMaxTime = 30;
    private float mTime;

	// Use this for initialization
	void Start ()
    {
        Instantiate(mStarGeneratorPrefab);
        mText = Instantiate(mCanvasPrefab).Find("Text").GetComponent<Text>();
        mTime = mMaxTime;
	}
	
	// Update is called once per frame
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
    
}
