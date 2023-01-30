using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// experiment file... maybe delete later???
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
	private void Awake()
	{
		current = this;
	}

	public event Action WhenMysteryBoxBought;
	public void BuyMysteryBox()
	{
		if (WhenMysteryBoxBought != null)
		{
			WhenMysteryBoxBought();
		}
	}

	public event Action WhenQuickReviveBought;
	public void BuyQuickRevivePerk()
	{
		if (WhenQuickReviveBought != null)
		{
			WhenQuickReviveBought();
		}
	}

	public event Action WhenJuggernogBought;
	public void BuyJuggernogPerk()
	{
		if (WhenJuggernogBought != null)
		{
			WhenJuggernogBought();
		}
	}

	public event Action WhenDoubleTapBought;
	public void BuyDoubleTapPerk()
	{
		if (WhenDoubleTapBought != null)
		{
			WhenDoubleTapBought();
		}
	}

	public event Action WhenSpeedColaBought;
	public void BuySpeedColaPerk()
	{
		if (WhenSpeedColaBought != null)
		{
			WhenSpeedColaBought();
		}
	}
}
