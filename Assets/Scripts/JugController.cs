using UnityEngine;
using Image = UnityEngine.UI.Image;

public class JugController : MonoBehaviour
{
    [SerializeField]
	PlayerStats playerstats;
	GameObject jugGameObject;

	private void Awake()
	{
		playerstats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
	}
	private void Buy() 
    {
		if (!playerstats.JuggBought)
		{
			if (playerstats.score >= 2500)
			{
				playerstats.PlayerLostPoints(2500);
				jugGameObject = new GameObject("juggernog");

				Image perkIcon = jugGameObject.AddComponent<Image>();

				perkIcon.sprite = Resources.Load<Sprite>("Images/jugg");
				perkIcon.transform.localScale = new Vector3(0.5f,0.5f);

				jugGameObject.GetComponent<RectTransform>().SetParent(playerstats.PerkSlots.transform);

				playerstats.RefresUIScore();
				playerstats.playerHealth += 100;
				playerstats.RefresUIHealth();
				playerstats.JuggBought = true;

				Debug.Log("juggBought");
			}
			else
			{
				Debug.Log("no money");
			}
		}
		
    }
}
