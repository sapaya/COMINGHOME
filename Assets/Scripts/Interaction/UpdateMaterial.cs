using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMaterial : MonoBehaviour {

    public Material newMaterial; // material that is set on update
    public GameObject target; // target mesh that the new material is assigned to
    public float fadeDuration; // duration of fade
    public float delay; // delay to revert
    public Condition condition; // if this condition is set, the revert will not happen

    private Material oldMaterial;
    private Material fadeStart;
    private Material fadeEnd;
    private float fadeTimeLeft;

    private void Awake() {
        this.oldMaterial = target.GetComponent<Renderer>().material;
    }

    private void Update() {
        if (this.fadeTimeLeft > Time.deltaTime) {
            target.GetComponent<Renderer>().material.color = Color.Lerp(this.fadeStart.color, this.fadeEnd.color, Time.deltaTime / this.fadeTimeLeft);
            this.fadeTimeLeft -= Time.deltaTime;
        }
    }

    public void SetNewMaterial() {
        target.GetComponent<Renderer>().material = this.newMaterial;
    }

    public void FadeNewMaterialColor() {
        this.fadeStart = oldMaterial;
        this.fadeEnd = newMaterial;
        this.fadeTimeLeft = this.fadeDuration;
    }

    public void BlinkInNewMaterial() {
        StartCoroutine(Blink(oldMaterial, newMaterial));
    }

    private IEnumerator Blink(Material current, Material blink, float delay = 0) {

        yield return new WaitForSeconds(delay);

        target.GetComponent<Renderer>().material = blink;

        yield return new WaitForSeconds(0.1f);

        target.GetComponent<Renderer>().material = current;

        yield return new WaitForSeconds(0.3f);

        target.GetComponent<Renderer>().material = blink;

        yield return new WaitForSeconds(1.0f);

        target.GetComponent<Renderer>().material = current;

        yield return new WaitForSeconds(0.1f);

        target.GetComponent<Renderer>().material = blink;

    }

    public void BlinkOutNewMaterialDelayed() {
        StartCoroutine(Blink(newMaterial, oldMaterial, this.delay));
    }

    public void RevertFade() {
        this.fadeStart = newMaterial;
        this.fadeEnd = oldMaterial;
        this.fadeTimeLeft = this.fadeDuration;
    }

    public IEnumerator RevertFadeDelayed(Condition c = null) {
        yield return new WaitForSeconds(delay);

        // if the bear is frozen the revert is triggered by an animation
        if (!AllConditions.CheckCondition(this.condition)) {
            this.RevertFade();
        }
    }

    public void Revert() {
        target.GetComponent<Renderer>().material = this.oldMaterial;
    }


    public void RevertBeerPuddle() {
        StartCoroutine(RevertFadeDelayed(this.condition));
    }

}
