using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClimateChange
{
    class Program : MonoBehaviour
    {
        
        public static List<String> currentOrders = new List<String>();
        static Product[] allowedProducts = new Product[3];
        static OvenModifier[] modifiers = new OvenModifier[2];
        public static GameObject orderObject;
        public float timer;

        public static Sprite emptyOvenSpr;
        public static Sprite treeSpr;
        public static Sprite factorySpr;
        public static Sprite[] iceCreamSpr = new Sprite[4];
        public static Sprite[] pizzaSpr = new Sprite[4];
        public static Sprite[] coffeeSpr = new Sprite[4];

        public static Sprite[] menuSpr = new Sprite[5];

        public static Sprite[] ovenBacks = new Sprite[3];
        public float ordersCooldown;
        public static float gameOverCooldown;

        public static Oven oven0;
        public static Oven oven1;

        public static int score;
        
        
        public static void Initialise()
        {
            Product iceCream = new Product("Ice Cream", 10 , TempStates.COLD);
            Product pizza = new Product("Pizza", 20, TempStates.TEMPERATE);
            Product coffee = new Product("Coffee", 30, TempStates.HOT);
            OvenModifier tree = new OvenModifier("Tree", 20, -5);
            OvenModifier factory = new OvenModifier("Factory", 10, 10);

            allowedProducts[0] = iceCream;
            allowedProducts[1] = pizza;
            allowedProducts[2] = coffee;
            modifiers[0] = factory;
            modifiers[1] = tree;
            
            emptyOvenSpr = Resources.Load<Sprite>("emptyOvenSpr");
            treeSpr = Resources.Load<Sprite>("tree alive");
            factorySpr = Resources.Load<Sprite>("factory");
            
            iceCreamSpr[0] = Resources.Load<Sprite>("new ice");
            iceCreamSpr[1] = Resources.Load<Sprite>("mid ice");
            iceCreamSpr[2] = Resources.Load<Sprite>("end ice");
            iceCreamSpr[3] = Resources.Load<Sprite>("ready ice");
            
            pizzaSpr[0] = Resources.Load<Sprite>("new pizza");
            pizzaSpr[1] = Resources.Load<Sprite>("mid pizza");
            pizzaSpr[2] = Resources.Load<Sprite>("end pizza");
            pizzaSpr[3] = Resources.Load<Sprite>("ready pizza");
            
            coffeeSpr[0] = Resources.Load<Sprite>("new coffee");
            coffeeSpr[1] = Resources.Load<Sprite>("mid coffee");
            coffeeSpr[2] = Resources.Load<Sprite>("end coffee");
            coffeeSpr[3] = Resources.Load<Sprite>("ready coffee");

            menuSpr[0] = Resources.Load<Sprite>("ready ice");
            menuSpr[1] = Resources.Load<Sprite>("end pizza");
            menuSpr[2] = Resources.Load<Sprite>("end coffee");
            menuSpr[3] = Resources.Load<Sprite>("tree alive");
            menuSpr[4] = Resources.Load<Sprite>("factory");

            ovenBacks[0] = Resources.Load<Sprite>("cold oven");
            ovenBacks[1] = Resources.Load<Sprite>("temperate oven");
            ovenBacks[2] = Resources.Load<Sprite>("hot oven");

            score = 0;
        }


        void Start()
        {
            orderObject = GameObject.Find("OrderList");
            // orderObject.AddComponent(typeof(UnityEngine.UI.Text));
            //Instantiate(orderObject);
            gameOverCooldown = 0.4f;
            ordersCooldown = 5f;

            Initialise();
            oven0 = new Oven(0);
            // oven0.SetOvenSpace(0, 0, allowedProducts[0]);
            // oven0.SetOvenSpace(0, 1, allowedProducts[1]);
            // oven0.SetOvenSpace(0, 2, allowedProducts[2]);
            // oven0.SetOvenSpace(1, 0, modifiers[0]);
            // oven0.SetOvenSpace(1, 1, modifiers[1]);

            oven1 = new Oven(1);
            // oven1.SetOvenSpace(0, 0, allowedProducts[2]);
            // oven1.SetOvenSpace(0, 1, allowedProducts[1]);
            // oven1.SetOvenSpace(0, 2, allowedProducts[0]);
            // oven1.SetOvenSpace(1, 0, modifiers[1]);
            // oven1.SetOvenSpace(1, 1, modifiers[0]);


        }

        void Update()
        {
            timer = Time.fixedTime;
            
            //Debug.Log("Time: " + Time.fixedTime);   

            // Get any new orders
            if (ordersCooldown < 0)
            {
                GetNewOrders();
                ordersCooldown = 10f; 
            }


            // Display orders
            DisplayCurrentOrders();
            //Debug.Log("print another thing");   
            // myOven.ProcessOven();

            oven0.ProcessOven();
            oven1.ProcessOven();
            gameOverCooldown -= Time.deltaTime;
            ordersCooldown -= Time.deltaTime;
            
            if (Input.GetButtonDown("Fire1")){
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.Log(ray);

            }
        }


        public static bool IsBetween(float mouseX, float mouseY, float x1, float x2, float y1, float y2){
            if (mouseX >= x1 && mouseX <= x2 && mouseY >= y1 && mouseY <= y2){
                return true;
            }
            else {
            return false;
            }
        }
        public static void GetNewOrders()
        {
            System.Random rnd = new System.Random();

            Product randomProduct = allowedProducts[rnd.Next(3)];
            Debug.Log("Order added:     " + randomProduct);
            currentOrders.Add(randomProduct.GetName());

            int orderNumber = currentOrders.Count;
            Debug.Log("order number;   " + orderNumber);
            if (orderNumber == 6){
                Debug.Log("GAME OVER");
                // GAME OVER
                GameObject goPanel = GameObject.Find("GameOver");

                goPanel.GetComponent<UnityEngine.UI.Text>().text = "GAME OVER \n Your score: " + score;

                
                
            }
        }
        public static void DisplayCurrentOrders()
        {   
            string fullText = "Order List: \n";
            if (currentOrders != null)
            {
                foreach (string o in currentOrders)
                {
                    fullText += o + "\n";
                }
            }

            orderObject.GetComponent<UnityEngine.UI.Text>().text = fullText;
        }

        public static void giveOrder()
        {
            
        }

        public static void HandleCellClicked(string cellName){
            Debug.Log("Handle cell clicked routine. cell[4] = " + cellName[4] + " | cell[5] = " + cellName[5] + " | cell[6] = " + cellName[6]);
            int ovenName = cellName[4];
            int xPos = cellName[5];
            int yPos = cellName[6];
            
            //cellName in format "celloxy" - oven, x, y
            if (cellName[4] == '0'){
                Debug.Log("Going to oven 0");
                oven0.HandleCell(xPos, yPos);
            }
            else if (cellName[4] == '1'){
                Debug.Log("Going to oven 1");
                oven1.HandleCell(xPos, yPos);
            }
        }

    }
}
