using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JLoader<T> where T : class
{
	public static T load(string resourcePath)
	{
		TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);

		if (jsonFile == null)
		{
			Debug.LogError($"JSON file not found at {resourcePath}");
			return null;
		}

		T data = JsonUtility.FromJson<T>(jsonFile.text);

		return data;
	}
}
