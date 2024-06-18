using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackColor : MonoBehaviour
{
    Material Material;
    float r = 255, g = 100, b = 150, value = 0.5f;
    bool rM, gM, bM;

    private void Start()
    {
        Material = GetComponent<SpriteRenderer>().material;
    }
    void Update()
    {
        if (r >= 255) rM = true; else if (r <= 0) rM = false;
        if (g >= 255) gM = true; else if (g <= 0) gM = false;
        if (b >= 255) bM = true; else if (b <= 0) bM = false;

        if (rM)
        {
            r -= value;
            g += value;
        }
        if (gM)
        {
            g -= value;
            b += value;
        }
        if (bM)
        {
            b -= value;
            r += value;
        }

        Color c = new(r / 255, g / 255, b / 255);
        Color c2 = new(b / 255, r / 255, g / 255);

        Material.SetColor("_TopColor", c);
        Material.SetColor("_ButtomColor", c2);
    }
}
