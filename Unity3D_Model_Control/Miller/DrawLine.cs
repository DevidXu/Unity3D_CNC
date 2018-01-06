using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {
	// public variables
	public float width = 0.0002f;
	public bool draw = false, newLine = true;
	public Vector3 vertex;

	public GameObject insLine;
	// Use this for initialization
	void Start () {
		
	}

	private GameObject line;
	private LineRenderer lineRenderer;  
	int numOfVertex;
	// Update is called once per frame
	void Update () {
		if (draw) {
			if (newLine || numOfVertex>100) {
				line = Instantiate (insLine) as GameObject;
				lineRenderer = (LineRenderer)line.GetComponent ("LineRenderer");
				numOfVertex = 0;
				lineRenderer.startWidth = width;
				lineRenderer.endWidth = width;
				newLine = false;
				AnimationCurve curve = new AnimationCurve ();
				curve.AddKey (0.0f, 1.0f);
				curve.AddKey (1.0f, 1.0f);
				lineRenderer.widthCurve = curve;
			}

			numOfVertex += 1;
			lineRenderer.positionCount = numOfVertex;
			lineRenderer.SetPosition (numOfVertex - 1, vertex);

			draw = false;
		}
	}
}
