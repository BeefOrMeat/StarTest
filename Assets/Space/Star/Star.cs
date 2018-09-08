using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField]
    private Material mDefaultMaterial;

    [SerializeField]
    private Material mSelectedMaterial;

    private Renderer mRenderer;

	// Use this for initialization
	void Start ()
    {
        mRenderer = GetComponent<Renderer>();
        mRenderer.material = mDefaultMaterial;
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
}
