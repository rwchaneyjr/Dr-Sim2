using UnityEngine;

public class HealthTile : MonoBehaviour
{
    [Header("Tile Colors")]
    public Color sickColor = Color.red;
    public Color recoveryColor = Color.blue;
    public Color healedColor = Color.green;

    [Header("Transparency")]
    [Range(0f, 1f)]
    public float alpha = 0.28f;

    [Header("Current State")]
    public bool isSick;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();

        if (rend == null)
        {
            Debug.LogError("HealthTile: No Renderer found on " + gameObject.name);
        }
    }

    void Start()
    {
        RandomizeTile();
    }

    public void RandomizeTile()
    {
        int randomValue = Random.Range(0, 2);

        if (randomValue == 0)
        {
            isSick = true;
            GameManager.sickTiles++;
        }
        else
        {
            isSick = false;
        }

        UpdateColor();
    }

    public void HealTile()
    {
        if (isSick)
        {
            GameManager.sickTiles--;
        }

        isSick = false;

        // FULLY visible green
        SetSolidColor(healedColor);
    }

    public void MakeRecoveryBlue()
    {
        isSick = false;

        // keep blue transparent
        SetTransparentColor(recoveryColor);
    }

    public void UpdateColor()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }

        if (rend == null)
        {
            return;
        }

        if (isSick)
        {
            SetTransparentColor(sickColor);      // transparent red
        }
        else
        {
            SetTransparentColor(recoveryColor);  // transparent blue
        }
    }

    void SetTransparentColor(Color baseColor)
    {
        if (rend == null)
        {
            return;
        }

        Color finalColor = baseColor;
        finalColor.a = alpha;
        rend.material.color = finalColor;
    }

    void SetSolidColor(Color baseColor)
    {
        if (rend == null)
        {
            return;
        }

        Color finalColor = baseColor;
        finalColor.a = 1f;
        rend.material.color = finalColor;
    }
}