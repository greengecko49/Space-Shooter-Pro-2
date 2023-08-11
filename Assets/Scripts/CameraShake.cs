using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPOS = new Vector3 (0, 0, -10);

        float elaspedTime = 0f;

        while (elaspedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            transform.position =new Vector3(xOffset, yOffset, originalPOS.z);

            elaspedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPOS;
    }
}
