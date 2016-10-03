﻿using System.Collections;
{

	public GameObject profileImage;
	{
	{
		{
			var rotation = Time.deltaTime * rotationSpeed;
			if (rotationLeft > rotation)
			{
				rotationLeft -= rotation;
			}
			else
			{
				rotation = rotationLeft;
				rotationLeft = 0.0f;
			}
			rectTransform.Rotate (Vector3.right, rotation);
		}
		{
			var zoom = Time.deltaTime * zoomSpeed;
			if (zoomLeft > zoom)
			{
				zoomLeft -= zoom;
			}
			else
			{
				zoom = zoomLeft;
				zoomLeft = 0.0f;
			}
			rectTransform.Translate (0.0f, 0.0f, -zoom);
		}
	{
		while (!Caching.ready)
		{
			yield return null;
		}

		var www = new WWW ("https://pbs.twimg.com/profile_images/747596515871358976/uEb5J9WP_400x400.jpg");
		yield return www;

		if (!string.IsNullOrEmpty (www.error))
		{
			Debug.Log (www.error);
			yield return null;
		}

		profileImage.GetComponent<Image>().sprite = Sprite.Create (www.texture, new Rect (0, 0, www.texture.width, www.texture.height), new Vector2 (0, 0));
		screenName.GetComponent<Text> ().text = "@sapphire_dev";

		isLoaded = true;
	}
	{
		if (isLoaded)
		{
			SceneManager.LoadScene ("main");
		}
	}