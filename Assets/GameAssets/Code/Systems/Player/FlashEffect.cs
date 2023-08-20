using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] private float flashAlpha;
    [SerializeField] private float flashSpeed;
    private SpriteRenderer spriteRenderer;

    private Coroutine flashCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartFlash()
    {
        flashCoroutine = StartCoroutine(Flash());
    }

    public void StopFlash()
    {
        StopCoroutine(flashCoroutine);
    }

    private IEnumerator Flash()
    {
        Color color = Color.white;
        while (true)
        {
            if (spriteRenderer.color.a == 1)
            {
                color.a = flashAlpha;
            } else
            {
                color.a = 1;
            }
            spriteRenderer.color = color;
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
