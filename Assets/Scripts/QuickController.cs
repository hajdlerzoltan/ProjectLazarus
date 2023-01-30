using UnityEngine;
using Image = UnityEngine.UI.Image;

public class QuickController : MonoBehaviour
{
	[SerializeField]
	PlayerStats playerstats;
	GameObject quckGameObject;

	private void Awake()
	{
		playerstats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
	}
	private void Buy()
	{
		if (!playerstats.QuickBought)
		{
			if (playerstats.score >= 500)
			{
				playerstats.PlayerLostPoints(500);
				quckGameObject = new GameObject("quickrevive");

				Image perkIcon = quckGameObject.AddComponent<Image>();

				perkIcon.sprite = Resources.Load<Sprite>("Images/quick");
				perkIcon.transform.localScale = new Vector3(0.5f, 0.5f);

				quckGameObject.GetComponent<RectTransform>().SetParent(playerstats.PerkSlots.transform);

				playerstats.RefresUIScore();
				playerstats.QuickBought = true;

				Debug.Log("QuickBought");
			}
			else
			{
				Debug.Log("no money");
			}
		}

	}
}
