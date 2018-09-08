using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMaker : MonoBehaviour
{
    [SerializeField]
    private Transform mLinePrefab;

    [SerializeField]
    private float mLineWidth;

    [SerializeField]
    private bool mXReverse = false;

    [SerializeField]
    private bool mYReverse = false;

    private GameObject mSelectedStar;

    private Vector3 mOldMousePosition;

    private Camera mCamera;

    private int mStarLayerMask;

	// Use this for initialization
	void Start ()
    {
        mCamera = GetComponent<Camera>();
        int layerNo = LayerMask.NameToLayer("Star");
        mStarLayerMask = 1 << layerNo;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 move = mousePosition - mOldMousePosition;
        mOldMousePosition = mousePosition;
        if (mXReverse)
        {
            move.y *= -1.0f;
        }
        if (mYReverse)
        {
            move.x *= -1.0f;
        }
        transform.Rotate(new Vector3(move.y, move.x, 0.0f));

		if (Input.GetMouseButtonDown(0))
        {
            LineProcess();
        }
	}

    void LineProcess()
    {
        Ray ray = mCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;

        //視点の先に星があれば
        if (!Physics.Raycast(ray, out hit, 10000, mStarLayerMask))
        {
            return;
        }
        GameObject newSelectedStar = hit.collider.gameObject;

        if (mSelectedStar == newSelectedStar)
        {
            return;
        }

        //選択している星があれば線を作成
        if (mSelectedStar != null)
        {
            Star selectedStarScript = mSelectedStar.GetComponent<Star>();
            selectedStarScript.OnReleased();

            float distance = Vector3.Distance(newSelectedStar.transform.position, mSelectedStar.transform.position);
            Transform line = Instantiate(mLinePrefab);
            line.position = mSelectedStar.transform.position;
            line.localScale = new Vector3(mLineWidth, mLineWidth, distance);            
            line.LookAt(newSelectedStar.transform.position);
            line.Translate(new Vector3(0, 0, distance * 0.5f));
        }
        Star newSelectedStarScript = newSelectedStar.GetComponent<Star>();
        newSelectedStarScript.OnSelected();

        mSelectedStar = newSelectedStar;
    }
}
