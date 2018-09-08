using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

    [SerializeField]
    private Material mDefaultMaterial;

    [SerializeField]
    private Material mFocusedMaterial;

    private Renderer mRenderer;

    public float LineWidth { get; set; }
    public float LineColliderWidthRate { get; set; }

	// Use this for initialization
	void Start ()
    {
        mRenderer = GetComponent<Renderer>();
        mRenderer.material = mDefaultMaterial;

        transform.localScale = new Vector3(LineWidth, LineWidth, transform.localScale.z);
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 colliderSize = collider.size;
        colliderSize.x *= LineColliderWidthRate;
        colliderSize.y *= LineColliderWidthRate;
        collider.size = colliderSize;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void OnReleased()
    {
        mRenderer.material = mDefaultMaterial;
    }

    public void OnFocused()
    {
        mRenderer.material = mFocusedMaterial;
    }
}
