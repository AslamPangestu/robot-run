using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMeter : MonoBehaviour {

	public float energy = 0f;                 // The player's initial energy.
	public float repeatPeriod = 1.2f;       // How frequently the player can be damaged.
	public float charge = 15f;            // The amount of energy charged by moving
	public int gear = 1;                    //player gear status
    public float loss = 6f;

	private SpriteRenderer meter;           // Reference to the sprite renderer of the health bar.
	private Vector3 barScale;                // The local scale of the energy bar initially (empty first).
	private PlayerControl playerControl;        // Reference to the PlayerControl script.
	private Animator anim;                      // Reference to the Animator on the player
	private float lastGainTime;					// The time at which the player was last hit.


	void Awake()
	{
		// Setting up references.
		playerControl = GetComponent<PlayerControl>();
		meter = GameObject.Find("GearMeter").GetComponent<SpriteRenderer>();
		//anim = GetComponent<Animator>();

		// Getting the intial scale of the healthbar (whilst the player has full health).
		barScale = meter.transform.localScale;
	}

	void Update()
	{
		Debug.Log("Energy: " + energy);
		Debug.Log("Gear: " + gear);
        UpdateGearBar();
    }

	public void ShiftGear(int shift)
	{
		switch (shift)
		{
		    case 1:
				if (gear < 3 && energy >= 100)
				{
					gear++;
					energy = 10f;
					Debug.Log("case 1");
					
				}
				break;
			case 0:
			    if (gear > 1)
                {
                    gear--;
                    energy = 60f;
                    Debug.Log("case 0");
                }					
				
			    break;

		}

		switch (gear)
		{
			case 1:
				charge = 15f;
				break;
			case 2:
				charge = 8f;
				break;
			case 3:
				charge = 2f;
				break;
		}
	}

	public void GainEnergy()
	{
		if (Time.time > lastGainTime + repeatPeriod)
		{
			if(energy < 100f)
			{
				energy += charge;
				lastGainTime = Time.time;
				if (energy > 100f) energy = 100f;
				
				
			}
		}
	}

    public void LoseEnergy()
    {
        if (Time.time > lastGainTime + repeatPeriod)
        {
            if (energy > 0f)
            {
                energy -= loss;
                lastGainTime = Time.time;
                if (energy < 0f) energy = 0f;


            }
        }
    }

    public void LoseEnergy(float cost)
    {
        if (Time.time > lastGainTime + repeatPeriod)
        {
            if (energy > 0f)
            {
                energy -= cost;
                lastGainTime = Time.time;
                if (energy < 0f) energy = 0f;


            }
        }
    }

    public void UpdateGearBar()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		meter.material.color = Color.Lerp(Color.green, Color.red, 1 - energy * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.
		if (barScale.x == 0) barScale.x = 1; 
		meter.transform.localScale = new Vector3(barScale.x * energy * 0.01f, 1, 1);
	}
}
