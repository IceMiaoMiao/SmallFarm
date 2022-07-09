using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    private bool isPlant = false;
    private SpriteRenderer plant;
    private BoxCollider2D plantCollider;
    
    private int plantStage = 0;
    private float timer;

    public PlantObject selectedPlant;
    private FarmManager fm;
    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;
    private SpriteRenderer plot;

    private bool isDry = true;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unavailableSprite;
    
    public float speed = 1f;
    public bool isBought = true;
    
    private void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        fm = transform.parent.GetComponent<FarmManager>();
        plot = GetComponent<SpriteRenderer>();
        if (isBought)
        {
            plot.sprite = drySprite;
        }
        else
        {
            plot.sprite = unavailableSprite;
        }
        
    }

    private void Update()
    {
        if (isPlant && !isDry)
        {
           timer -= speed*Time.deltaTime;
            if (timer < 0 && plantStage < selectedPlant.plantStages.Length-1)
            {
                timer = selectedPlant.timeBtwStages;
                plantStage++;
                UpdatePlant();
            } 
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
    }

    private void OnMouseDown()
    {
        if (isPlant)
        {
            if (plantStage == selectedPlant.plantStages.Length-1 && !fm.isPlanting && !fm.isSelecting)
            {
              Harvest();  
            }
        }
        else if (fm.isPlanting && fm.selectPlant.plant.buyPrice <= fm.money && isBought)
        {
            Plant(fm.selectPlant.plant);
        }

        if (fm.isSelecting)
        {
            switch (fm.selectTool)
            {
                case 1:
                    if (isBought)
                    {
                        isDry = false;
                        plot.sprite = normalSprite;
                        if (isPlant)
                        {
                            UpdatePlant();
                        }
                    }
                    
                    break;
                case 2:
                    if (fm.money > 10)
                    {
                        fm.Transaction(-10);
                        if (speed < 3)
                        {
                            speed += 0.5f;
                        }
                    }
                    break;
                case 3:
                    if (fm.money >= 100 && !isBought)
                    {
                        fm.Transaction(-100);
                        isBought = true;
                        plot.sprite = drySprite;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        if (fm.isPlanting)
        {
            if (isPlant || fm.selectPlant.plant.buyPrice > fm.money || !isBought)
            {
                plot.color = unavailableColor;
            }
            else
            {
                plot.color = availableColor;
            }

            if (fm.isSelecting)
            {
                switch (fm.selectTool)
                {
                    case 1:
                    case 2:
                        if (isBought && fm.money >= (fm.selectTool-1)*10)
                        {
                            plot.color = availableColor;
                        }
                        else
                        {
                            plot.color = unavailableColor;
                        }
                        break;
                    case 3:
                        if (!isBought && fm.money>=100)
                        {
                            plot.color = availableColor;
                        }
                        else
                        {
                            plot.color = unavailableColor;
                        }
                        break;
                    default:
                        plot.color = unavailableColor;
                        break;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        plot.color = Color.white;
    }

    private void Harvest()
    {
        isPlant = false;
        plant.gameObject.SetActive(false);
        fm.Transaction(selectedPlant.sellPrice);
        isDry = true;
        plot.sprite = drySprite;
        speed = 1;
    }

    private void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlant = true;
        fm.Transaction(-selectedPlant.buyPrice);
        
        plantStage = 0;
        UpdatePlant();
        timer = selectedPlant.timeBtwStages;
        plant.gameObject.SetActive(true);
    }

    void UpdatePlant()
    {
        if (isDry)
        {
            plant.sprite = selectedPlant.dryPlanted;
        }
        else
        {
            plant.sprite = selectedPlant.plantStages[plantStage];
        }
        
        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0, plant.bounds.size.y / 2);
    }
}
