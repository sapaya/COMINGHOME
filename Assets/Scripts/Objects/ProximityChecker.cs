using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityChecker : MonoBehaviour {
    [SerializeField] Condition cond;
    public float minDist;
    public float maxDist;
    //public bool onlySmaller;

    public enum CheckType
    {
        Proximity,
        Angle,
        Intersection
    }

    public CheckType checkFor;

    public GameObject obj1;
    public GameObject obj2;

	
	// Update is called once per frame
	void Update () {
        switch (checkFor)
        {
            case CheckType.Angle:
                AngleCheck();
                break;

            case CheckType.Intersection:
                IntersectionTest();
                break;
                
            case CheckType.Proximity:
                ProximityTest();
                break;

            default:
                break;
        }
        
	}

    private void AngleCheck()
    {
        Vector3 targetDir = obj2.transform.position - obj1.transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (minDist < angle && angle < maxDist) // checks if z-axis of transform is almost exactly towards target
            cond.satisfied = true;
        else
            cond.satisfied = false;
    }

    private void IntersectionTest()
    {
        Bounds bounds1 = obj1.GetComponent<Renderer>().bounds;
        Bounds bounds2 = obj2.GetComponent<Renderer>().bounds;

        if (bounds1.Intersects(bounds2))
            cond.satisfied = true;
        else
            cond.satisfied = false;
    }

    private void ProximityTest()
    {

        //Debug.Log(string.Format("MinDist: {0}, MaxDist: {1}, Dist: {2}", minDist, maxDist, Vector3.Distance(obj1.transform.position, obj2.transform.position)));
        //Debug.Log(string.Format("AstPos: {0}, MasterPos: {1}", obj1.transform.position, obj2.transform.position));

        if (minDist <= Vector3.Distance(obj1.transform.position, obj2.transform.position) && Vector3.Distance(obj1.transform.position, obj2.transform.position) <= maxDist)
        {
            //Debug.Log(string.Format("AstPos: {0}, MasterPos: {1}", obj1.transform.position, obj2.transform.position));
            Debug.Log(string.Format("CLOSE ENOUGH! MinDist: {0}, MaxDist: {1}, Dist: {2}", minDist, maxDist, Vector3.Distance(obj1.transform.position, obj2.transform.position)));
            //if ((onlySmaller && obj1.transform.position.y < obj2.transform.position.y) || !onlySmaller)
            cond.satisfied = true;
        }
        else
        {
            cond.satisfied = false;
        }
            
    }
}
