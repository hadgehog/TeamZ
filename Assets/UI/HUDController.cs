﻿using Assets.Code.Helpers;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
	public Slider HealthSlider;
	public Slider ArmorSlider;

	private UnityDependency<Lizard> character;

	void Update()
	{
		if (this.character)
		{
			this.HealthSlider.value = this.character.Value.Health;
			this.ArmorSlider.value = this.character.Value.Armor;
		}
	}
}
