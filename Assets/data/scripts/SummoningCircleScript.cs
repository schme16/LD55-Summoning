using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCircleScript : MonoBehaviour {
	public Transform a;
	public Transform b;
	public Transform c;
	public Transform d;
	public Transform e;
	public Transform circle;
	public float min = 2.5f;
	public float max = 9f;
	public float speed = 9f;
	public bool show;

	private Material a_mat;
	private Material b_mat;
	private Material c_mat;
	private Material d_mat;
	private Material e_mat;
	private Material circle_mat;
	private float currentCircle;
	private float currentRunes;
	private float currentRunes_a;
	private float currentRunes_b;
	private float currentRunes_c;
	private float currentRunes_d;
	private float currentRunes_e;

	// Start is called before the first frame update
	void Start() {
		a_mat = a.GetComponent<MeshRenderer>().material;
		b_mat = b.GetComponent<MeshRenderer>().material;
		c_mat = c.GetComponent<MeshRenderer>().material;
		d_mat = d.GetComponent<MeshRenderer>().material;
		e_mat = e.GetComponent<MeshRenderer>().material;
		circle_mat = circle.GetComponent<MeshRenderer>().material;

		a_mat.SetFloat("_dissolveAmount", max);
		b_mat.SetFloat("_dissolveAmount", max);
		c_mat.SetFloat("_dissolveAmount", max);
		d_mat.SetFloat("_dissolveAmount", max);
		e_mat.SetFloat("_dissolveAmount", max);
		circle_mat.SetFloat("_dissolveAmount", max);
	}

	// Update is called once per frame
	void Update() {
	}
}