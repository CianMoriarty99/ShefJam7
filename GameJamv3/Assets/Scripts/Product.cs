using System;
namespace ClimateChange
{
    public class Product : Plantable
    {
        private TempStates growTemp;
        private bool harvestReady;
        
        public Product(string _name, int _lifeSpan, TempStates _growTemp) : base(_name, _lifeSpan)
        {
            growTemp = _growTemp;
            harvestReady = false;
            age = 0;
        }


        public override string ToString()
        {
            return "Name:   " + name + "\n";
        }

        public bool IsHarvestReady()
        {
            if (age == lifeSpan)
            {
                harvestReady = true;
            }
            return harvestReady;
        }

        public override int GetTempChange()
        {
            return 0;
        }

        public override Plantable CheckEndLife()
        {
            harvestReady = true;
            return this;
        }

        public override bool ReadyForHarvest()
        {
            return harvestReady;
        }

        public override TempStates GetTempState(){
            return growTemp;
        }


    }
}
