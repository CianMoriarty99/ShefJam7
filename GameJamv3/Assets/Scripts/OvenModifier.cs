using System;
namespace ClimateChange
{
    public class OvenModifier : Plantable
    {

        private int tempChange;

        public OvenModifier(string _name, int _lifeSpan, int _tempChange) : base(_name, _lifeSpan)
        {
            tempChange = _tempChange;
            age = 0;
        }

        public override string ToString()
        {
            return "name:  " + name + " | lifeSpan " + lifeSpan + " | age:  " + age + " | tempChange: " + tempChange;
        }

        public override int GetTempChange()
        {
            return tempChange;
        }
                
        public override Plantable CheckEndLife()
        {
            return null;
        }

        public override bool ReadyForHarvest()
        {
            return false;
        }

        public override TempStates GetTempState(){
            return TempStates.INVALID;
        }

    }
}
