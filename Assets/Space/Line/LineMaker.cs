using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMaker : MonoBehaviour
{
    [SerializeField]
    private Transform mLinePrefab;

    [SerializeField]
    private float mLineWidth = 0.5f;
    [SerializeField]
    private float mLineColliderWidthRate = 1.0f;

    [SerializeField]
    private bool mXReverse = false;

    [SerializeField]
    private bool mYReverse = false;

    private GameObject mSelectedStar;
    private GameObject mFocusedStar;

    private Vector3 mOldMousePosition;

    private Transform mNextLine;
    private GameObject mFocusedLine;

    private Camera mCamera;

    private int mStarLayerMask;

    private int mLineLayerMask;

    // Use this for initialization
    void Start ()
    {
        mCamera = GetComponent<Camera>();
        int starLayerNo = LayerMask.NameToLayer("Star");
        mStarLayerMask = 1 << starLayerNo;
        int lineLayerNo = LayerMask.NameToLayer("Line");
        mLineLayerMask = 1 << lineLayerNo;
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
    }

    public void AddLine(Ray ray)
    {
        RaycastHit hit;

        //視点の先に星がなければフォーカス状態を解除してリターン
        if (!Physics.Raycast(ray, out hit, 10000, mStarLayerMask))
        {
            if (mFocusedStar != null && mFocusedStar != mSelectedStar)
            {
                Star focusedStarScript = mFocusedStar.GetComponent<Star>();
                focusedStarScript.OnReleased();
                mFocusedStar = null;
            }
            return;
        }

        //視点の先の星をフォーカスされた星に
        GameObject newFocusedStar = hit.collider.gameObject;
        Star newFocusedStarScript = newFocusedStar.GetComponent<Star>();

        //フォーカスされた星が切り替わった時
        if (newFocusedStar != mFocusedStar)
        {
            //古いフォーカス状態の星を通常状態に
            if (mFocusedStar != null && mFocusedStar != mSelectedStar)
            {
                Star focusedStarScript = mFocusedStar.GetComponent<Star>();
                focusedStarScript.OnReleased();
            }
            //フォーカスされた星をフォーカス状態に
            newFocusedStarScript.OnFocused();
            mFocusedStar = newFocusedStar;

            if (mNextLine == null)
            {
                mNextLine = Instantiate(mLinePrefab);
                Line nextLineScript = mNextLine.GetComponent<Line>();
                nextLineScript.LineWidth = mLineWidth;
                nextLineScript.LineColliderWidthRate = mLineColliderWidthRate;
            }
            mNextLine.gameObject.SetActive(true);
            if (mSelectedStar)
            {
                float distance = Vector3.Distance(mFocusedStar.transform.position, mSelectedStar.transform.position);
                mNextLine.position = mSelectedStar.transform.position;
                mNextLine.localScale = new Vector3(mNextLine.localScale.x, mNextLine.localScale.y, distance);
                mNextLine.LookAt(mFocusedStar.transform.position);
                mNextLine.Translate(new Vector3(0, 0, distance * 0.5f));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //選択状態の星を選択した場合連結を解除
            if (mSelectedStar == newFocusedStar)
            {
                Star selectedStarScript = mSelectedStar.GetComponent<Star>();
                selectedStarScript.OnReleased();
                mSelectedStar = null;
                if (mNextLine != null)
                {
                    mNextLine.gameObject.SetActive(false);
                }
                return;
            }
            //選択している星があれば線を作成
            if (mSelectedStar != null)
            {
                Star selectedStarScript = mSelectedStar.GetComponent<Star>();
                selectedStarScript.OnReleased();
                mNextLine = null;
            }
            newFocusedStarScript.OnSelected();

            mSelectedStar = mFocusedStar;
            return;
        }
        return;
    }

    public void RemoveLine(Ray ray)
    {
        RaycastHit hit;

        //視点の先に線があれば
        if (!Physics.Raycast(ray, out hit, 10000, mLineLayerMask))
        {
            return;
        }
        if (mFocusedLine != null && Input.GetMouseButtonDown(1))
        {
            Destroy(mFocusedLine);
            mFocusedLine = null;
            return;
        }
        if (hit.collider.transform == mNextLine)
        {
            if (mFocusedLine != null)
            {
                Line oldFocusedLineScript = mFocusedLine.GetComponent<Line>();
                oldFocusedLineScript.OnReleased();
                mFocusedLine = null;
            }
            return;
        }
        if (mFocusedLine != hit.collider.gameObject)
        {
            if (mFocusedLine != null)
            {
                Line oldFocusedLineScript = mFocusedLine.GetComponent<Line>();
                oldFocusedLineScript.OnReleased();
            }
            mFocusedLine = hit.collider.gameObject;
            Line focusedLineScript = mFocusedLine.GetComponent<Line>();
            focusedLineScript.OnFocused();
        }
        return;
    }
}
