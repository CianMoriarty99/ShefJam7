﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateChange{

    public class Menu10 : MonoBehaviour
    {
        
        private Plantable plantable;

        void Start()
        {
            plantable = new Product("Ice Cream", 10 , TempStates.COLD);
        }

        void Update(){
                float x1 = this.transform.position.x - 15;
                float x2 = this.transform.position.x + 15;
                float y1 = this.transform.position.y - 15;
                float y2 = this.transform.position.y + 15;

                if(Input.GetButtonDown("Fire1") && Program.IsBetween(Input.mousePosition.x, Input.mousePosition.y, x1,x2,y1,y2))
                {
                    Program.oven1.PlantItem(plantable);
                    Debug.Log("Planted - " + plantable);
                }
                
            }
    }
}
