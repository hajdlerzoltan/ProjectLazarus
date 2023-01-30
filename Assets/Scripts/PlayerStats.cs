using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Unity.Netcode;

public class PlayerStats : NetworkBehaviour
{
	[SerializeField]
	TextMeshProUGUI uitext;

	[SerializeField]
	public GameObject PerkSlots;

	private GameObject healthUI;
	private int _score = 10000;
	private NetworkVariable<string> _name;
	//private string _name = "player";
	private int _playerHealth = 100;
	private bool _juggBought = false;
	private bool _quickBought = false;
	private bool _speedBought = false;
	private bool _tapBought = false;

	public int score 
	{ 
		get 
		{ 
			return _score; 
		}
		set 
		{
			_score = value;
		}
	}
	public NetworkVariable<string> playername
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public int playerHealth
	{
		get
		{
			return PlayerHealth;
		}
		set
		{
			PlayerHealth = value;
		}
	}

	public int PlayerHealth { get => _playerHealth; set => _playerHealth = value; }
	public bool JuggBought { get => _juggBought; set => _juggBought = value; }
	public bool QuickBought { get => _quickBought; set => _quickBought = value; }
	public bool SpeedBought { get => _speedBought; set => _speedBought = value; }
	public bool TapBought { get => _tapBought; set => _tapBought = value; }

	private void Awake()
	{
		if (IsServer)
		{
			RefresUIScore();
		}
	}

	public void RefresUIScore()
	{
		uitext.text = $"{_name.Value}: {score}";
	}

	public void RefresUIHealth()
	{
		healthUI = GameObject.Find("HealtText");
		healthUI.GetComponent<TextMeshProUGUI>().text = $"Health: {_playerHealth}";
	}

	public int PlayerGetPoints(int amountOfPoints) 
	{
		return _score += amountOfPoints;
	}
	public int PlayerLostPoints(int amountOfPoints) 
	{
		return _score -= amountOfPoints;
	}


}
