using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{

    [SerializeField]
    private int mStarCount;

    [SerializeField]
    private float mRadius;

    [SerializeField]
    private Transform mStarPrefab;

	void Start ()
    {
        for (int i = 0; i < mStarCount; ++i)
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);
            float z = Random.Range(-1.0f, 1.0f);
            Transform star = GameObject.Instantiate(mStarPrefab);
            star.position = new Vector3(x, y, z).normalized * mRadius;
        }
    }
	
	void Update () {
		
	}
}
