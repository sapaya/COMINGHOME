using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnderwaterEffect : MonoBehaviour {
    [Header("Underwater detection Settings")]
    public GameObject waterObject;
    public ParticleSystem bubbleParticles;

    private bool isUnderwater;

    [Header("Above water Fog Settings")]
    [SerializeField] Color normalColor = new Color(0.09f, 0.1f, 0.13f, 0.25f);
    [Range(0f, 1f)]
    [SerializeField] float normalDensity = 0.005f;

    [Header("Underwater Fog Settings")]
    [SerializeField] Color underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);
    [Range(0f, 1f)]
    [SerializeField] float underwaterDensity = 0.05f;

	// Use this for initialization
	void Start () {
        bubbleParticles.Stop();
    }
	
	// Update is called once per frame
	void Update () {
        if ((transform.position.y < waterObject.transform.position.y) != isUnderwater) {
            isUnderwater = transform.position.y < waterObject.transform.position.y;

            if (isUnderwater) SetUnderwater();
            else SetNormal();
        }
	}

    private void SetNormal() {
        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = 0.005f;
        bubbleParticles.Stop();
    }

    private void SetUnderwater() {
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = 0.05f;
        bubbleParticles.Play();
    }
}
