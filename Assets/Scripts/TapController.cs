using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class TapController : MonoBehaviour
{
	[SerializeField]
	PlayerStats playerstats;
	GameObject TapGameObject;

	private void Awake()
	{
		playerstats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
	}
	private void Buy()
	{
		if (!playerstats.TapBought)
		{
			if (playerstats.score >= 2000)
			{
				playerstats.PlayerLostPoints(2000);
				TapGameObject = new GameObject("quickrevive");

				Image perkIcon = TapGameObject.AddComponent<Image>();

				perkIcon.sprite = Resources.Load<Sprite>("Images/tap");
				perkIcon.transform.localScale = new Vector3(0.5f, 0.5f);

				TapGameObject.GetComponent<RectTransform>().SetParent(playerstats.PerkSlots.transform);

				playerstats.RefresUIScore();
				playerstats.TapBought = true;

				Debug.Log("TapBought");
			}
			else
			{
				Debug.Log("no money");
			}
		}

	}
}
