using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationMaker : MonoBehaviour
{
    [SerializeField]
    private Transform mLinePrefab;

    [SerializeField]
    private float mLineWidth = 0.5f;
    [SerializeField]
    private float mLineColliderWidthRate = 1.0f;
    [SerializeField]
    private float mLinePadding = 0.5f;

    private GameObject mSelectedStar;
    private GameObject mFocusedStar;

    private Transform mNextLine;
    private GameObject mFocusedLine;


    private int mStarLayerMask;
    private int mLineLayerMask;

    [SerializeField]
    private Camera mCamera;

#if UNITY_EDITOR 
    private bool mXReverse = false;
    private bool mYReverse = false;
    private float mMoveX;
    private float mMoveY;
#endif

    // Use this for initialization
    void Start ()
    {
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

        mCamera.transform.LookAt(lookAtPos);
        mCamera.transform.Rotate(new Vector3(mMoveY * Mathf.Rad2Deg, 0.0f, 0.0f));

        Ray ray = mCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        AddLine(ray);
        RemoveLine(ray);
#endif
    }

    public void AddLine(Ray ray)
    {
        RaycastHit hit;

        //視点の先に星がなければフォーカス状態を解除してリターン
        if (!Physics.Raycast(ray, out hit, 10000, mStarLayerMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                MakeLine();
            }
            return;
        }

        //視点の先の星をフォーカスされた星に
        GameObject newFocusedStar = hit.collider.gameObject;
        Star newFocusedStarScript = newFocusedStar.GetComponent<Star>();

        //フォーカスされた星が切り替わった場合
        if (newFocusedStar != mFocusedStar)
        {
            ReleaseFocusedStar();

            //フォーカスされた星をフォーカス状態に
            newFocusedStarScript.OnFocused();
            mFocusedStar = newFocusedStar;

            //選択している星が既にある場合はその星との連結をシミュレートした線を表示する
            if (mSelectedStar)
            {
                //選択状態の星がフォーカス状態の星だったら線を非表示に
                if (mSelectedStar == mFocusedStar)
                {
                    if (mNextLine)
                    {
                        mNextLine.gameObject.SetActive(false);
                    }
                }
                //選択状態の星とフォーカス状態の星が違ければ２点を結ぶ線を表示
                else
                {
                    if (!mNextLine)
                    {
                        mNextLine = Instantiate(mLinePrefab);
                        Line nextLineScript = mNextLine.GetComponent<Line>();
                        nextLineScript.LineWidth = mLineWidth;
                        nextLineScript.LineColliderWidthRate = mLineColliderWidthRate;
                    }
                    mNextLine.gameObject.SetActive(true);
                    float distance = Vector3.Distance(mFocusedStar.transform.position, mSelectedStar.transform.position);
                    mNextLine.position = mSelectedStar.transform.position;
                    mNextLine.localScale = new Vector3(mNextLine.localScale.x, mNextLine.localScale.y, distance - mLinePadding * 2.0f);
                    mNextLine.LookAt(mFocusedStar.transform.position);
                    mNextLine.Translate(new Vector3(0, 0, distance * 0.5f));
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            MakeLine();
        }
    }

    private void MakeLine()
    {
        //２点の星が別の星であれば線を連結
        if (mFocusedStar != mSelectedStar)
        {
            if (mFocusedStar && mSelectedStar)
            {
                Star selectedStarScript = mSelectedStar.GetComponent<Star>();
                selectedStarScript.OnReleased();

                Star focusedStarScript = mFocusedStar.GetComponent<Star>();
                focusedStarScript.OnSelected();
                //線をシミュレートしてる位置で固定する
                mNextLine = null;
            }
            mSelectedStar = mFocusedStar;
        }
        //同じ星を選択した場合
        else
        {
            if (mSelectedStar)
            {
                Star selectedStarScript = mSelectedStar.GetComponent<Star>();
                selectedStarScript.OnReleased();
                mSelectedStar = null;
            }
        }
        //シミュレート線を非表示に
        if (mNextLine)
        {
            mNextLine.gameObject.SetActive(false);
        }
    }
    

    //フォーカス状態の星を通常状態に
    private void ReleaseFocusedStar()
    {
        if (mFocusedStar && mFocusedStar != mSelectedStar)
        {
            Star focusedStarScript = mFocusedStar.GetComponent<Star>();
            focusedStarScript.OnReleased();
            mFocusedStar = null;
        }
    }
    
    public void RemoveLine(Ray ray)
    {
        RaycastHit hit;

        //視点の先に線があれば
        if (!Physics.Raycast(ray, out hit, 10000, mLineLayerMask))
        {
            ReleaseFocusedLine();
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
            ReleaseFocusedLine();
            return;
        }
        if (mFocusedLine != hit.collider.gameObject)
        {
            ReleaseFocusedLine();
            FocuseLine(hit.collider.gameObject);
        }
        return;
    }

    private void FocuseLine(GameObject lineObject)
    {
        mFocusedLine = lineObject;
        Line focusedLineScript = mFocusedLine.GetComponent<Line>();
        focusedLineScript.OnFocused();
    }

    private void ReleaseFocusedLine()
    {
        if (!mFocusedLine)
        {
            return;
        }
        Line oldFocusedLineScript = mFocusedLine.GetComponent<Line>();
        oldFocusedLineScript.OnReleased();
        mFocusedLine = null;
    }
}