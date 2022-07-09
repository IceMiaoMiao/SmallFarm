using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    public PlantItem selectPlant;
    public bool isPlanting = false;
    public int money = 100;
    public TextMeshProUGUI moneyTex;
    
    public Color buyColor = Color.green;
    public Color cancelColor = Color.red;

    public bool isSelecting = false;
    public int selectTool = 0;
    public Image[] buttonImages;
    public Sprite normalBtn;
    public Sprite selectBtn;
    
    void Start()
    {
        moneyTex.text = "S" + money;
    }

    
    

    public void SelectPlant(PlantItem newPlant)
    {
        if (selectPlant == newPlant)
        {
            
            CheckSelection();
        }
        else
        {
            CheckSelection();
            
            selectPlant = newPlant;
            selectPlant.btnImage.color = cancelColor;
            selectPlant.btnTex.text = "Cancel";
            isPlanting = true;
        }
    }

    public void SelectTool(int toolNumber)
    {
        if (toolNumber == selectTool)
        {
            CheckSelection();
        }
        else
        {
            CheckSelection();
            isSelecting = true;
            selectTool = toolNumber;
            buttonImages[toolNumber - 1].sprite = selectBtn;
        }
    }

    void CheckSelection()
    {
        if (isPlanting)
        {
            isPlanting = false;
            if (selectPlant != null)
            {
                selectPlant.btnImage.color = buyColor;
                selectPlant.btnTex.text = "Buy";
                selectPlant = null;
            }
        }
        if (isSelecting)
        {
            if (selectTool > 0)
            {
                buttonImages[selectTool - 1].sprite = normalBtn;
            }
            isSelecting = false;
            selectTool = 0;
        }
    }
    public void Transaction(int value)
    {
        money += value;
        moneyTex.text = "S" + money;
    }
}
