using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClimateChange{
    public class ClickExample : MonoBehaviour {

	public void myClickMeth(){
		Debug.Log("Clicked button with name:  " + this.name);
		Program.HandleCellClicked(this.name);
	}
}
}