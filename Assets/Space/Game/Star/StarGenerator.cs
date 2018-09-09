using System.Collections;
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

    [SerializeField]
    private float mStarRadius = 1.0f;

    [SerializeField]
    private float mStarRadiusVariance = 0.5f;

    private List<Star> mStarList = new List<Star>();

    private bool mGenerated;
    private int mSeed = -1;

    public void SetSeed(int seed)
    {
        mSeed = seed;
    }

    public void AddConstellation(ConstellationData data)
    {
        List<Star> stars = data.GetChildrenStars();
        foreach (Star star in stars)
        {
            star.transform.SetParent(null);
            star.Radius = mStarColliderRadius;

            mStarList.Add(star);
        }
    }

    private void Update()
    {
        if (mGenerated)
        {
            return;
        }
        if (mSeed == -1)
        {
            return;
        }
        Random.InitState(mSeed);
        GenerateStars();
        mGenerated = true;
    }

    private bool StarCollision(Vector3 aPos, float ar, Vector3 bPos, float br)
    {
        float rx = aPos.x - bPos.x;
        float ry = aPos.y - bPos.y;
        float rz = aPos.z - bPos.z;
        return rx * rx + ry * ry + rz * rz <= ar * br;
    }

    private void GenerateStars ()
    {
        //星が重なってる場合でも10回試してダメだったらあきらめて重なって表示する
        for (int i = 0; i < mStarCount; ++i)
        {
            int limit = 10;
            Transform star = Instantiate(mStarPrefab);
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
                        star.transform.position, mStarColliderRadius,
                        generatedStarScript.transform.position, mStarColliderRadius
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
            Star starScript = star.GetComponent<Star>();
            mStarList.Add(starScript);
        }

        //星の大きさにばらつきを作る
        foreach (Star star in mStarList)
        {
            float scale = mStarRadius + Random.Range(-mStarRadiusVariance, mStarRadiusVariance);
            star.transform.localScale = Vector3.one * scale;
            star.Radius = mStarColliderRadius;
        }
    }

}
