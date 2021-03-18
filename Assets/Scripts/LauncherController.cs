using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour
{
    public const float LAUNCHER_INTERVAL = 3f;

    [SerializeField]
    private GameObject trowable;
    [SerializeField]
    private Transform launchPoint;

    private IEnumerator interval = null;
    
    public void Launch()
    {
        if (interval != null)
            return;

        interval = Interval();
        StartCoroutine(interval);

        var obj = Instantiate(trowable, launchPoint.position, Quaternion.LookRotation(launchPoint.forward, launchPoint.up));
        obj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 15, ForceMode.Impulse);
    }

    IEnumerator Interval()
    {
        yield return new WaitForSeconds(LAUNCHER_INTERVAL);
        interval = null;
    }
}
