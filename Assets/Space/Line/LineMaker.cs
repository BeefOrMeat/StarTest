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
    private GameObject mFocusedStar;

    private Vector3 mOldMousePosition;

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

        bool use = false;
        if (!use)
        {
            use = AddLineProcess();
        }
        if (!use)
        {
            use = RemoveLineProcess();
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>操作が行われればtrue</returns>
    bool AddLineProcess()
    {
        Ray ray = mCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
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
            return false;
        }

        //視点の先の星をフォーカスされた星に
        GameObject newFocusedStar = hit.collider.gameObject;
        Star newFocusedStarScript = newFocusedStar.GetComponent<Star>();

        //フォーカスされた星が選択状態だったら処理しない
        if (mSelectedStar == newFocusedStar)
        {
            return true;
        }

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
        }

        if (Input.GetMouseButtonDown(0))
        {
            //選択している星があれば線を作成
            if (mSelectedStar != null)
            {
                Star selectedStarScript = mSelectedStar.GetComponent<Star>();
                selectedStarScript.OnReleased();

                float distance = Vector3.Distance(mFocusedStar.transform.position, mSelectedStar.transform.position);
                Transform line = Instantiate(mLinePrefab);
                line.position = mSelectedStar.transform.position;
                line.localScale = new Vector3(mLineWidth, mLineWidth, distance);
                line.LookAt(mFocusedStar.transform.position);
                line.Translate(new Vector3(0, 0, distance * 0.5f));
            }
            newFocusedStarScript.OnSelected();

            mSelectedStar = mFocusedStar;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>操作が行われればtrue</returns>
    bool RemoveLineProcess()
    {
        Ray ray = mCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;

        //視点の先に線があれば
        if (!Physics.Raycast(ray, out hit, 10000, mLineLayerMask))
        {
            return false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(hit.collider.gameObject);
            return true;
        }
        else
        {

        }
        return true;
    }
}
