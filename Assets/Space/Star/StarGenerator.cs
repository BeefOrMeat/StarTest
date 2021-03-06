﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{

    [SerializeField]
    private int mStarCount;

    [SerializeField]
    private float mStarColliderRadius = 1.0f;

    [SerializeField]
    private float mRadius;

    [SerializeField]
    private Transform mStarPrefab;

    private List<Star> mStarList = new List<Star>();

    private bool StarCollision(Vector3 aPos, float ar, Vector3 bPos, float br)
    {
        float rx = aPos.x - bPos.x;
        float ry = aPos.y - bPos.y;
        float rz = aPos.z - bPos.z;
        return rx * rx + ry * ry + rz * rz <= ar * br;
    }

    void Start ()
    {
        //星が重なってる場合でも10回試してダメだったらあきらめて重なって表示する
        for (int i = 0; i < mStarCount; ++i)
        {
            int limit = 10;
            Transform star = Instantiate(mStarPrefab);
            Star starScript = star.GetComponent<Star>();
            starScript.Radius = mStarColliderRadius;
            while (limit > 0)
            {
                float x = Random.Range(-1.0f, 1.0f);
                float y = Random.Range(0.0f, 1.0f);
                float z = Random.Range(-1.0f, 1.0f);
                star.position = new Vector3(x, y, z).normalized * mRadius;

                //既にある星と重なるか判定する
                bool overlaped = false;
                foreach (Star generatedStarScript in mStarList)
                {
                    if (StarCollision(
                        starScript.transform.position, starScript.Radius,
                        generatedStarScript.transform.position, generatedStarScript.Radius
                        ))
                    {
                        overlaped = true;
                        break;
                    }
                }
                if (!overlaped)
                {
                    break;
                }
                --limit;
            }
            mStarList.Add(starScript);
        }
    }
	
	void Update () {
		
	}
}
