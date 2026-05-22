using UnityEngine;

public class Patient : MonoBehaviour
{
    public bool isSick = false;
    public Renderer bodyRenderer;

    public enum Condition
    {
        Dehydration,
        Infection,
        Fever,
        Burn,
        Sprain,
        HeartPalpitation,
        Headache,
        FoodPoisoning,
        Cold,
        BrokenArm,
        Flu,
        ToothAche,
        StomachPain
    }

    public Condition currentCondition = Condition.Dehydration;
    public float health = 100f;

    private Color normalColor = Color.white;

    void Awake()
    {
        if (bodyRenderer == null)
            bodyRenderer = GetComponentInChildren<Renderer>();

        if (bodyRenderer != null)
            normalColor = bodyRenderer.material.color;

        ApplyConditionColor();
    }

    void Start()
    {
        ApplyConditionColor();
    }

    public void SetCondition(Condition newCondition)
    {
        currentCondition = newCondition;
        ApplyConditionColor();
    }

    public void Heal(float amount)
    {
        health += amount;

        if (health > 100f)
            health = 100f;
    }

    public void Recover()
    {
        if (bodyRenderer == null)
            bodyRenderer = GetComponentInChildren<Renderer>();

        if (bodyRenderer != null)
            bodyRenderer.material.color = normalColor;
    }

    public void AdverseReaction()
    {
        health -= 20f;

        if (health < 0f)
            health = 0f;

        if (bodyRenderer == null)
            bodyRenderer = GetComponentInChildren<Renderer>();

        if (bodyRenderer != null)
            bodyRenderer.material.color = Color.black;
    }

    void ApplyConditionColor()
    {
        if (bodyRenderer == null)
            bodyRenderer = GetComponentInChildren<Renderer>();

        if (bodyRenderer != null)
            bodyRenderer.material.color = GetConditionColor(currentCondition);
    }

    Color GetConditionColor(Condition condition)
    {
        switch (condition)
        {
            case Condition.Dehydration: return new Color(0.85f, 0.65f, 0.25f);
            case Condition.Infection: return new Color(0.15f, 0.95f, 0.15f);
            case Condition.Fever: return new Color(1f, 0.45f, 0.1f);
            case Condition.Burn: return new Color(1f, 0.15f, 0.15f);
            case Condition.Sprain: return new Color(0.95f, 0.55f, 0.15f);
            case Condition.HeartPalpitation: return new Color(1f, 0f, 1f);
            case Condition.Headache: return new Color(0.55f, 0.55f, 0.55f);
            case Condition.FoodPoisoning: return new Color(0.45f, 0.85f, 0.2f);
            case Condition.Cold: return new Color(0.35f, 0.7f, 1f);
            case Condition.BrokenArm: return new Color(0.75f, 0.75f, 1f);
            case Condition.Flu: return new Color(0.55f, 1f, 0.55f);
            case Condition.ToothAche: return new Color(1f, 0.8f, 0.8f);
            case Condition.StomachPain: return new Color(0.65f, 0.25f, 0.95f);
            default: return normalColor;
        }
    }

    public string GetSymptoms()
    {
        switch (currentCondition)
        {
            case Condition.Dehydration: return "Thirst, dizziness";
            case Condition.Infection: return "Weakness, chills";
            case Condition.Fever: return "Hot skin, sweating";
            case Condition.Burn: return "Red skin, pain";
            case Condition.Sprain: return "Swelling, pain";
            case Condition.HeartPalpitation: return "Fast heartbeat";
            case Condition.Headache: return "Head pain";
            case Condition.FoodPoisoning: return "Nausea, vomiting";
            case Condition.Cold: return "Sneezing, cough";
            case Condition.BrokenArm: return "Arm pain, cannot move";
            case Condition.Flu: return "Body aches, fever";
            case Condition.ToothAche: return "Tooth pain, jaw soreness";
            case Condition.StomachPain: return "Stomach pain, cramping";
            default: return "Unknown symptoms";
        }
    }
}