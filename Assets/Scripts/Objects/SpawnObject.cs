using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpawnObject : MonoBehaviour
{
    public void SpawnCopy()
    {
        GameObject copy = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        Animator anim = copy.GetComponent<Animator>();
        anim.SetTrigger("Activate");
    }

    public void DestroyCopy()
    {
        Destroy(gameObject);
    }
}
