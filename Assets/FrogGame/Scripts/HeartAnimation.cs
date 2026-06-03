using UnityEngine;
using System.Collections;

public class HeartAnimation : MonoBehaviour
{
    public float scaleMultiplier = 1.2f;
    public float duration = 0.1f;

    public void Start() //pulse
    {
        StartCoroutine(PulseRoutine());
    }

    IEnumerator PulseRoutine()
    {
        Vector3 original = transform.localScale;
        Vector3 target = original * scaleMultiplier;

        float t = 0;

        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(original, target, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;

        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(target, original, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = original;
    }
}
