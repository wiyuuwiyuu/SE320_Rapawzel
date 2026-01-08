using System.Collections;
using UnityEngine;

public class timer : MonoBehaviour
{
    public float duration = 10f;

    // OnEnable runs every time the object is set to Active(true)
    private void OnEnable()
    {
        // Stop any previous timers to avoid double-triggers
        StopAllCoroutines(); 
        StartCoroutine(HideTimer());
    }

    IEnumerator HideTimer()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
