using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationData : MonoBehaviour
{
    private List<Star> mStars;

    [SerializeField]
    private Transform mStarPrefab;

    [SerializeField]
    private Transform mLinePrefab;

    [SerializeField]
    private Transform mLineBegin;
    [SerializeField]
    private Transform mLineEnd;

    void Start ()
    {
	}

    public void Normalize(float radius)
    {
        transform.position = Vector3.zero;
        List<Star> stars = GetChildrenStars();
        foreach (Star star in stars)
        {
            if (Vector3.Distance(Vector3.zero, star.transform.position) < radius * 0.5f)
            {
                star.transform.position += new Vector3(0.0f, 0.0f, radius);
            }
            Vector3 pos = star.transform.position;
            star.transform.position = pos.normalized * radius;
        }
    }

    private List<Star> GetChildrenStars()
    {
        List<Star> stars = new List<Star>();
        foreach (Transform child in transform)
        {
            Star star = child.GetComponent<Star>();
            if (!star)
            {
                continue;
            }
            stars.Add(star);
        }
        return stars;
    }

    public void AddStar()
    {
        Transform star = Instantiate(mStarPrefab, transform);
        star.name = star.name + "(" + GetChildrenStars().Count() + ")";
    }

    public void AddLine(float lineWidth, float linePadding)
    {
        if (!mLineBegin || !mLineEnd)
        {
            return;
        }
        Transform line = Instantiate(mLinePrefab, transform);
        Line lineScript = line.GetComponent<Line>();
        lineScript.LineWidth = lineWidth;
        float distance = Vector3.Distance(mLineBegin.transform.position, mLineEnd.transform.position);
        line.position = mLineEnd.transform.position;
        line.localScale = new Vector3(lineWidth, lineWidth, distance - linePadding * 2.0f);
        line.LookAt(mLineBegin.transform.position);
        line.Translate(new Vector3(0, 0, distance * 0.5f));
    }
	
}
