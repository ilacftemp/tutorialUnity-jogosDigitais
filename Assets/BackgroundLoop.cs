using UnityEngine;
using System.Collections;

public class BackgroundLoop : MonoBehaviour
{
    public Sprite[] backgrounds;
    public SpriteRenderer currentRenderer;
    public SpriteRenderer nextRenderer;
    public float transitionTime = 2f;
    public float waitTime = 10f;

    private int currentIndex = 0;

    void Start()
    {
        currentRenderer.sprite = backgrounds[currentIndex];
        currentRenderer.color = new Color(1, 1, 1, 1);
        nextRenderer.color = new Color(1, 1, 1, 0);
        StartCoroutine(LoopBackgrounds());
    }

    IEnumerator LoopBackgrounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            // Define o próximo sprite
            int nextIndex = (currentIndex + 1) % backgrounds.Length;
            nextRenderer.sprite = backgrounds[nextIndex];

            // Começa o fade-in do próximo
            float timer = 0f;
            while (timer < transitionTime)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / transitionTime);

                nextRenderer.color = new Color(1, 1, 1, t); // fade-in
                currentRenderer.color = new Color(1, 1, 1, 1); // permanece 100% opaco
                yield return null;
            }

            // Troca completa: o próximo vira o atual
            currentRenderer.sprite = nextRenderer.sprite;
            currentRenderer.color = new Color(1, 1, 1, 1);
            nextRenderer.color = new Color(1, 1, 1, 0);

            currentIndex = nextIndex;
        }
    }
}
