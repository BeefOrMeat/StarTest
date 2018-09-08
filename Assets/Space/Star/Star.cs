using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField]
    private Material mDefaultMaterial;

    [SerializeField]
    private Material mSelectedMaterial;

    [SerializeField]
    private Material mFocusedMaterial;

    private Renderer mRenderer;

    public float Radius { get; set; }

    // Use this for initialization
    void Start ()
    {
        mRenderer = GetComponent<Renderer>();
        mRenderer.material = mDefaultMaterial;

        SphereCollider collider = GetComponent<SphereCollider>();
        collider.radius *= Radius;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnSelected()
    {
        mRenderer.material = mSelectedMaterial;
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
