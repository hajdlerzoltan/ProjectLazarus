using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class SpeedController : MonoBehaviour
{
	[SerializeField]
	PlayerStats playerstats;
	GameObject speedGameObject;

	private void Awake()
	{
		playerstats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
	}
	private void Buy()
	{
		if (!playerstats.SpeedBought)
		{
			if (playerstats.score >= 3000)
			{
				playerstats.PlayerLostPoints(3000);
				speedGameObject = new GameObject("quickrevive");

				Image perkIcon = speedGameObject.AddComponent<Image>();

				perkIcon.sprite = Resources.Load<Sprite>("Images/speed");
				perkIcon.transform.localScale = new Vector3(0.5f, 0.5f);

				speedGameObject.GetComponent<RectTransform>().SetParent(playerstats.PerkSlots.transform);

				playerstats.RefresUIScore();
				playerstats.SpeedBought = true;

				Debug.Log("SpeedBought");
			}
			else
			{
				Debug.Log("no money");
			}
		}

	}
}
