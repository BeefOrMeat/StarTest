﻿using System.Collections;
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

    private GameObject mSelectedStar;
    private GameObject mFocusedStar;

    private Transform mNextLine;
    private GameObject mFocusedLine;

    private Camera mCamera;

    private int mStarLayerMask;
    private int mLineLayerMask;

#if UNITY_EDITOR 
    private bool mXReverse = false;
    private bool mYReverse = false;
    private float mMoveX;
    private float mMoveY;
#endif

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
#if UNITY_EDITOR
        Vector3 move = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (mXReverse)
        {
            move.y *= -1.0f;
        }
        if (mYReverse)
        {
            move.x *= -1.0f;
        }
        mMoveX += move.x * 0.05f;
        mMoveY += move.y * 0.05f;

        mMoveY = Mathf.Clamp(mMoveY, -Mathf.PI, Mathf.PI);
        Vector3 lookAtPos = Vector3.zero;
        float rad = mMoveX + Mathf.PI * 0.5f;
        lookAtPos.x = Mathf.Cos(rad);
        lookAtPos.z = Mathf.Sin(rad);
        lookAtPos *= 10.0f;

        transform.LookAt(lookAtPos);
        transform.Rotate(new Vector3(mMoveY * Mathf.Rad2Deg, 0.0f, 0.0f));
#endif
        Ray ray = mCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        AddLine(ray);
        RemoveLine(ray);
    }

    public void AddLine(Ray ray)
    {
        RaycastHit hit;

        //視点の先に星がなければフォーカス状態を解除してリターン
        if (!Physics.Raycast(ray, out hit, 10000, mStarLayerMask))
        {
            if (mFocusedStar && mFocusedStar != mSelectedStar)
            {
                Star focusedStarScript = mFocusedStar.GetComponent<Star>();
                focusedStarScript.OnReleased();
                mFocusedStar = null;
            }
            if (mNextLine)
            {
                mNextLine.gameObject.SetActive(false);
            }
            //何もない部分をクリックしていたら連結を解除
            if (Input.GetMouseButtonDown(0))
            {
                if (mSelectedStar)
                {
                    Star selectedStarScript = mSelectedStar.GetComponent<Star>();
                    selectedStarScript.OnReleased();
                    mSelectedStar = null;
                    if (mNextLine)
                    {
                        mNextLine.gameObject.SetActive(false);
                    }
                }
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
            if (mFocusedStar && mFocusedStar != mSelectedStar)
            {
                Star focusedStarScript = mFocusedStar.GetComponent<Star>();
                focusedStarScript.OnReleased();
            }
            //フォーカスされた星をフォーカス状態に
            newFocusedStarScript.OnFocused();
            mFocusedStar = newFocusedStar;

            if (!mNextLine)
            {
                mNextLine = Instantiate(mLinePrefab);
                Line nextLineScript = mNextLine.GetComponent<Line>();
                nextLineScript.LineWidth = mLineWidth;
                nextLineScript.LineColliderWidthRate = mLineColliderWidthRate;
            }
            if (mSelectedStar)
            {
                mNextLine.gameObject.SetActive(true);
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
                if (mNextLine)
                {
                    mNextLine.gameObject.SetActive(false);
                }
                return;
            }
            //選択している星があれば線を作成
            if (mSelectedStar)
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
            if (mFocusedLine != null)
            {
                Line oldFocusedLineScript = mFocusedLine.GetComponent<Line>();
                oldFocusedLineScript.OnReleased();
                mFocusedLine = null;
            }
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