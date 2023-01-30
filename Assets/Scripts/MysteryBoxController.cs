using System;
using System.Collections;
using UnityEngine;

public class MysteryBoxController : MonoBehaviour
{
	[SerializeField]
	Animator animator;
	[SerializeField]
	GameObject reftoplayerstats;
	PlayerStats playerstats;
	AudioSource open;
	bool IsInPogress = false;

	private void Awake()
	{
		playerstats = reftoplayerstats.GetComponent<PlayerStats>();
		animator = GetComponent<Animator>();
		open= GetComponent<AudioSource>();
	}

	private void OnDestroy()
	{
		//GameEvents.current.WhenMysteryBoxBought -= StartMysteryBox;
	}
	// Start is called before the first frame update
	void Start()
    {
        //GameEvents.current.WhenMysteryBoxBought += StartMysteryBox;
    }

    private void Buy() 
    {
		Debug.Log("Mystery");
		if (!IsInPogress)
		{
			IsInPogress = true;
			if (playerstats.score >= 950)
			{
				playerstats.PlayerLostPoints(950);
				playerstats.RefresUIScore();
				animator.Play("BoxOpen");
				open.Play();
				animator.SetBool("IsBought", true);
				StartCoroutine(BoxTimer(30.0f));
			}
			else
			{
				Debug.Log("no money");
			}
		}
        
	}

	public IEnumerator BoxTimer(float time) 
	{
		yield return new WaitForSeconds(time);
		animator.SetBool("IsFinished",true);
		animator.Play("BoxClose");
		open.Play();
		Debug.Log("closed");
		IsInPogress  =false;
	}
}
