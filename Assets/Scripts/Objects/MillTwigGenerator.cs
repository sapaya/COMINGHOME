using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillTwigGenerator : MonoBehaviour
{
    #region Variables
    [SerializeField] int maxTwigs = 2;
    [SerializeField] float delayInSeconds = 8.5f;

    public static List<MillTwigGenerator> millTwigs = new List<MillTwigGenerator>();
    Coroutine spawn;
    #endregion

    #region Main Methods
    // Start is called before the first frame update
    void Start()
    {
        millTwigs.Add(this);
        StartCoroutine(SpawnNewTwig());
    }
    #endregion

    #region Helper Methods
    public void StartSpawning()
    {
        spawn = StartCoroutine(SpawnNewTwig());
    }

    private IEnumerator SpawnNewTwig()
    {
        yield return new WaitForSecondsRealtime(delayInSeconds);
        if(millTwigs.Count < maxTwigs)
        {
            GameObject newTwig = Instantiate(gameObject);
            MillTwigGenerator mtg = newTwig.GetComponent<MillTwigGenerator>();
            millTwigs.Add(mtg);
            //mtg.StartSpawning();
            Debug.Log("Initialized MillTwig nr. " + millTwigs.Count);
        }
    }

    public void DestroyTwig()
    {
        millTwigs.Remove(this);
        Destroy(gameObject);
    }

    public void StopSpawning()
    {
        foreach(MillTwigGenerator mtg in millTwigs)
        {
            StopCoroutine(mtg.spawn);
            if (mtg != this)
                mtg.DestroyTwig();
        }
    }

    public void StopRestart() {
        foreach (MillTwigGenerator mtg in MillTwigGenerator.millTwigs) {
            mtg.GetComponent<Animator>().SetBool("Restart", false);
        }
    }
    #endregion
}
