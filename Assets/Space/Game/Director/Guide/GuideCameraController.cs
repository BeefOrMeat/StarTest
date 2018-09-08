using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCameraController : MonoBehaviour
{
    [SerializeField]
    private bool mXReverse = false;

    [SerializeField]
    private bool mYReverse = false;

    [SerializeField]
    private float mLookAtDistance = 10.0f;

    [SerializeField]
    private float mMoveSpeed = 0.05f;
    
    private float mMoveX;
    private float mMoveY;

    void Start()
    {
    }

    void Update ()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (!Input.GetMouseButton(0))
        {
            return;
        }

        Vector3 move = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (mXReverse)
        {
            move.y *= -1.0f;
        }
        if (mYReverse)
        {
            move.x *= -1.0f;
        }
        mMoveX += move.x * mMoveSpeed;
        mMoveY += move.y * mMoveSpeed;

        mMoveY = Mathf.Clamp(mMoveY, -Mathf.PI, Mathf.PI);
        Vector3 lookAtPos = Vector3.zero;
        float rad = mMoveX + Mathf.PI * 0.5f;
        lookAtPos.x = Mathf.Cos(rad);
        lookAtPos.z = Mathf.Sin(rad);
        lookAtPos *= mLookAtDistance;

        transform.LookAt(lookAtPos);
        transform.Rotate(new Vector3(mMoveY * Mathf.Rad2Deg, 0.0f, 0.0f));
    }
}
