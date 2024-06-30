using ImprovedTimers;
using UnityEngine;

public class Flasher
{
    private SpriteRenderer spriteRenderer;

    private Material originalMaterial;

    private Material flashMaterial;

    private CountdownTimer flashTimer;

    public Flasher(ref SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;

        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(Shader.Find("Shader Graphs/Flash"));

        flashTimer = new CountdownTimer(0);
        flashTimer.OnTimerStop += ResetMaterial;
    }

    public void Flash(Color col, float duration)
    {
        flashMaterial.SetColor("_Color", col);

        spriteRenderer.material = flashMaterial;

        flashTimer.Reset(duration);
        flashTimer.Start();
    }

    private void ResetMaterial()
    {
        spriteRenderer.material = originalMaterial;
    }
}
