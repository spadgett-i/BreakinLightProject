using UnityEngine;
using UnityEngine.UI;

public class ParryEnergy : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;
    public float energyGainPerParry = 34f;

    public Image energyFillImage;

    void Start()
    {
        currentEnergy = 0f;
        UpdateEnergyUI();
    }

    public void GainEnergy()
    {
        currentEnergy += energyGainPerParry;
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;

        UpdateEnergyUI();
        Debug.Log("CLING ⚡ Energía: " + currentEnergy);
    }

    // Método público para actualizar la barra
    public void UpdateEnergyUI()
    {
        if (energyFillImage != null)
            energyFillImage.fillAmount = currentEnergy / maxEnergy;
    }
}




