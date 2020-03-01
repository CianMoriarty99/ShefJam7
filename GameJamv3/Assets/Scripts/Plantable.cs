using System;
using UnityEngine;

namespace ClimateChange
{
    public abstract class Plantable
    {
        protected string name;
        protected int lifeSpan;
        protected float age;
        protected float ageNew;

        public Plantable(string _name, int _lifeSpan)
        {
            name = _name;
            lifeSpan = _lifeSpan;
            age = 0;
            
        }

        public abstract int GetTempChange();

        public void Increment()
        {

            ageNew += Time.deltaTime;
        }

        public abstract Plantable CheckEndLife();

        public abstract bool ReadyForHarvest();

        public abstract TempStates GetTempState();


        public int GetLifeSpan()
        {
            return lifeSpan;
        }

        public string GetName(){
            return name;
        }

        public float GetAge(){
            return ageNew;
        }
        

        
    }
}
