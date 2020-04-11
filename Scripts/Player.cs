using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Player
{
	
	public string attempts;
	public string passed;
	
	public Player(){}
	
	public Player(string name, string pass)
	{
		attempts = name;
		passed = pass;
	}


}
