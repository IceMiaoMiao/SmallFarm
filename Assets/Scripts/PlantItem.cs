using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantItem : MonoBehaviour
{
    public PlantObject plant;
    public TextMeshProUGUI nameTex;
    public TextMeshProUGUI priceTex;
    public Image icon;

    public Image btnImage;
    public TextMeshProUGUI btnTex;
    
    private FarmManager fm;
    
    private void Start()
    {
        fm = FindObjectOfType<FarmManager>();
        InitializeUI();
    }

    public void ButPlant()
    {
        Debug.Log("Buy"+plant.name);
        fm.SelectPlant(this);
    }

    void InitializeUI()
    {
        nameTex.text = plant.name;
        priceTex.text = "$"+plant.buyPrice;
        icon.sprite = plant.icon; 
    }
}
