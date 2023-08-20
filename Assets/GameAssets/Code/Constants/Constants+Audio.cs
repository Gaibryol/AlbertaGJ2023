using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Constants
{
	public class Audio
	{
		public const string MusicVolumePP = "Music";
		public const string SFXVolumePP = "SFX";

		public const float DefaultMusicVolume = 0.25f;
		public const float DefaultSFXVolume = 0.25f;

		public const float MusicFadeSpeed = 0.25f;

		public class Music
		{
			public const string Boss = "boss";
			public const string Main = "main";
			public const string Tutorial = "tutorial-loop";
		}

		public class SFX
		{
			public const string Death = "death";
			public const string Dash = "dash";
			public const string HealthPack = "health_pack";
		}
	}
}
