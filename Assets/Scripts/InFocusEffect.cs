using UnityEngine;
using System.Collections;

public class InFocusEffect : MonoBehaviour {

    public float scaleTime = 10;
    public float scaleUpMultiplier;

    Vector3 defaultScale;
    Vector3 maxScale;
    // Use this for initialization
    void Start () {

        defaultScale = transform.localScale;
        maxScale = defaultScale * scaleUpMultiplier;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ScaleUpObject()
    {
        StartCoroutine(scaleObject(true));
    }

    void ScaleDownObject()
    {
        StartCoroutine(scaleObject(false));
    }

    IEnumerator scaleObject(bool larger)
    {
        float t = 0;
        Vector3 currentScale = transform.localScale;
        Vector3 targetScale;
        if (larger)
        {
            targetScale = new Vector3(
                Mathf.Clamp(currentScale.x * scaleUpMultiplier, defaultScale.x, maxScale.x),
                Mathf.Clamp(currentScale.y * scaleUpMultiplier, defaultScale.y, maxScale.y),
                Mathf.Clamp(currentScale.z * scaleUpMultiplier, defaultScale.z, maxScale.z)
                );
        }
        else
        {
            targetScale = new Vector3(
                Mathf.Clamp(currentScale.x / scaleUpMultiplier, defaultScale.x, maxScale.x),
                Mathf.Clamp(currentScale.y / scaleUpMultiplier, defaultScale.y, maxScale.y),
                Mathf.Clamp(currentScale.z / scaleUpMultiplier, defaultScale.z, maxScale.z)
                );

        }
        //transform.localScale = targetScale;
        do
        {
            gameObject.transform.localScale = Vector3.Lerp(currentScale, targetScale, t / scaleTime);
            yield return null;
            t += (Time.deltaTime * 5);
        } while (t < scaleTime);
        gameObject.transform.localScale = targetScale;
        yield break;
    }
}
