using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionWhenNearBy : MonoBehaviour
{
	private bool IsInteractible = false;
	private GameObject InteractibelGameObject = null;


    // Update is called once per frame
    void Update()
    {
		
		if (IsInteractible)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				InteractibelGameObject.SendMessage("Buy");
			}
		}
	}
	//gets the collided gameobject's data and enableing the interraction with the object
	private void OnTriggerEnter(Collider other)
	{
		InteractibelGameObject = other.gameObject;
        if (InteractibelGameObject.tag == "Interactible") 
        {
			IsInteractible = true;
		}
        
	}
	//disableing the interaction with the previous gameobject
	private void OnTriggerExit(Collider other)
	{
		InteractibelGameObject = null;
		if (other.gameObject.tag == "Interactible")
		{
			IsInteractible = false;
		}
	}

}
