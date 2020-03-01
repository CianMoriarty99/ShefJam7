using System;
using UnityEngine;
using UnityEngine.UI;
namespace ClimateChange
{
    public class Oven 
    {
        private int ovenNo;
        private Plantable[,] ovenSpaces = new Plantable[4, 4];
        private TempStates tempState;
        private int temp;

        private float cooldown = 0.5f;

        public Oven(int _ovenNo)
        {
            ovenNo = _ovenNo;
            temp = 50;
            tempState = TempStates.TEMPERATE;
        }

        public void ProcessOven()
        {
            if(cooldown < 0)
            {
                UpdateTemp();
                cooldown = 0.5f;
            }
                
            IncrementOvenSpaces();
            displayOven();
            displayMenu();

            cooldown -= Time.deltaTime;
            
        }

        public void SetOvenSpace(int x, int y, Plantable item)
        {
            ovenSpaces[x, y] = item;
        }

        public Plantable GetOvenSpace(int x, int y)
        {
            return ovenSpaces[x, y];
        }

        public void PlantItem(Plantable plantable){
            Plantable newPlantable;
            if (tempState == plantable.GetTempState() || plantable.GetTempState() == TempStates.INVALID){
                int[] places = NextNull();
                Debug.Log(plantable.GetName());
                if (plantable.GetName() == "Pizza"){newPlantable = new Product("Pizza", 20, TempStates.TEMPERATE);}
                else if (plantable.GetName() == "Ice Cream"){ newPlantable = new Product("Ice Cream", 10 , TempStates.COLD);}
                else if (plantable.GetName() == "Coffee"){ newPlantable = new Product("Coffee", 30, TempStates.HOT);}
                else if (plantable.GetName() == "Tree"){ newPlantable = new OvenModifier("Tree", 20, -5);}
                else if (plantable.GetName() == "Factory"){ newPlantable = new OvenModifier("Factory", 10, 10);}
                else{newPlantable = null;}
                SetOvenSpace(places[0],places[1], newPlantable);
            }

        }

        private int[] NextNull(){
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (ovenSpaces[i,j] == null){
                        int[] temp = new int[2];
                        temp[0] = i;
                        temp[1] = j;
                        return temp;
                    }
                }
            }
            return null;
        }

        public void IncrementOvenSpaces()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Plantable ovenSpace = ovenSpaces[i, j];

                    if (ovenSpace != null)
                    {
                        ovenSpace.Increment();
                        if (ovenSpace.GetAge() > ovenSpace.GetLifeSpan() )
                        {
                            ovenSpaces[i,j] = ovenSpace.CheckEndLife();
                            
                        }
                        
                    }
                }
            }
        }

        public void HandleCell(int x, int y)
        {
            x -= 48;
            y -= 48;

            if (ovenSpaces[x,y] != null){
                if (ovenSpaces[x,y].ReadyForHarvest()){
                    Debug.Log("Ready for harvest");
                    // take away from orders list and set this tile to null
                    if( Program.currentOrders.Contains(ovenSpaces[x,y].GetName())){
                        Debug.Log("Found in list!!");
                        Program.currentOrders.Remove(ovenSpaces[x,y].GetName());
                        ovenSpaces[x,y] = null;
                        Program.score++;
                    }
                    
                    
                    
                }
                else{
                    Debug.Log("Not ready for harvest");
                }
            }
            else{
                Debug.Log("This is null");
            }
        }

        // private void 
        private void UpdateTemp()
        {
            int overallTempDiff = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (ovenSpaces[i,j] != null)
                    {
                        overallTempDiff += ovenSpaces[i,j].GetTempChange();
                        if (!(tempState == ovenSpaces[i,j].GetTempState() || ovenSpaces[i,j].GetTempState() == TempStates.INVALID)){
                            ovenSpaces[i,j] = null;
                        }
                    }
                }
            }
        
            int sinValue =  (int)(Mathf.Sin(Time.fixedTime / 3)*6);
            float flSin = (Mathf.Sin(Time.fixedTime / 3)*6);
            Debug.Log("floatS: " + flSin);
            Debug.Log("Sinvalue: " + sinValue);
            Debug.Log("Total time elapsed:  " + Time.fixedTime);
            temp += overallTempDiff + sinValue;


            GameObject cell = GameObject.Find("OvenBack" + ovenNo);
            Image img = cell.GetComponent<Image>();
            

            // Check for change in tempState
            if (temp > 180)
            {
                tempState = TempStates.HOT;
                img.sprite = Program.ovenBacks[2];
            }
            else if (temp > 0)
            {
                tempState = TempStates.TEMPERATE;
                img.sprite = Program.ovenBacks[1];
            }
            else
            {
                tempState = TempStates.COLD;
                img.sprite = Program.ovenBacks[0];
            }
        }

        private void DisplayReadyForHarvest()
        {
            string harvest = "-------- Harvest ------- \n";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (ovenSpaces[i, j] != null)
                    {
                        if (ovenSpaces[i, j].ReadyForHarvest())
                        {
                            harvest += ovenSpaces[i, j] + "\n";
                        }
                    }
                }
            }
            harvest += " ------------- ";
            Debug.Log(harvest);
        }
        //private void IncrementOvenSpaces

        public override string ToString()
        {
            string output = "Oven Temp:    " + temp + "\n";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // add component to the sprit

                    GameObject cell = GameObject.Find("cell"+j+i);
                    Image img = cell.GetComponent<Image>();
                    img.sprite = Program.emptyOvenSpr;
                    
                    // if (ovenSpaces[i, j] == null) { img.sprite = emptyOvenSpr; }
                    // else if (ovenSpaces[i, j].ReadyForHarvest()) { output += " H "; }
                    // else { output += " P "; }
                }
                output += "\n";
            }

            return output;
        }

        public void displayOven(){

            // Temperature
            GameObject tempText = GameObject.Find("Temperature"+ovenNo);
            tempText.GetComponent<UnityEngine.UI.Text>().text = "Temperature: " + temp;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    float age = 10;
                    int lifeSpan = 0;
                    // add component to the sprit
                    string cellName = "cell"+j+i;
                    GameObject cell = GameObject.Find("cell"+ovenNo+j+i);
                    Image img = cell.GetComponent<Image>();
                    Plantable currOvenSpace = ovenSpaces[j,i];

                    if (currOvenSpace != null){
                        age = currOvenSpace.GetAge();
                        lifeSpan = currOvenSpace.GetLifeSpan();
                    }

                    if (currOvenSpace == null) img.sprite = Program.emptyOvenSpr;

                    else if (currOvenSpace.GetName() == "Tree") img.sprite = Program.treeSpr;
                    else if (currOvenSpace.GetName() == "Factory") img.sprite = Program.factorySpr;

                    else if (currOvenSpace.GetName() == "Ice Cream") {

                        if (age < (lifeSpan/3f)){
                            img.sprite = Program.iceCreamSpr[0];
                        }
                        else if (age < (lifeSpan/(3f/2f))){
                            img.sprite = Program.iceCreamSpr[1];
                        }
                        else if (age < lifeSpan) {
                            img.sprite = Program.iceCreamSpr[2];
                        }
                        else{
                            img.sprite = Program.iceCreamSpr[3];
                        } 
                       
                    }
                    else if (currOvenSpace.GetName() == "Pizza") {

                        if (age < (lifeSpan/(3f))){
                            img.sprite = Program.pizzaSpr[0];
                        }
                        else if (age < (lifeSpan/(3f/2f))){
                            img.sprite = Program.pizzaSpr[1];
                        }
                        else if (age < lifeSpan) {
                            img.sprite = Program.pizzaSpr[2];
                        }
                        else{
                            img.sprite = Program.pizzaSpr[3];
                        } 
                       
                    }
                    else if (currOvenSpace.GetName() == "Coffee") {

                         if (age < (lifeSpan/(3f))){
                            img.sprite = Program.coffeeSpr[0];
                        }
                        else if (age < (lifeSpan/(3f/2f))){
                            img.sprite = Program.coffeeSpr[1];
                        }
                        else if (age < lifeSpan) {
                            img.sprite = Program.coffeeSpr[2];
                        }
                        else{
                            img.sprite = Program.coffeeSpr[3];
                        } 
                       
                    }


                    
                }
            }
        }
        public void displayMenu(){
            for (int i = 0; i < 5; i++)
            {
                var j = i + 1;
                    GameObject menuItem = GameObject.Find("menuCell"+ovenNo+j);
                    Image img = menuItem.GetComponent<Image>();
                    img.sprite = Program.menuSpr[i];
            }               
    }
    }
}
