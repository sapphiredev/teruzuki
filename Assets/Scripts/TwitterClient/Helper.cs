﻿using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace teruzuki.Twitter
{
	public static class Helper
	{
		public static string BuildRESTURL (string endpoint)
		{
			return string.Format ("{0}{1}.json", Constants.URL.REST_BASE_URL, endpoint);
		}

		public static string BuildUserStreamURL (string endpoint)
		{
			return string.Format ("{0}{1}.json", Constants.URL.USERSTREAM_BASE_URL, endpoint);
		}

		public static Dictionary<string, string> ParseQueryString (string queryString)
		{
			var dict = new Dictionary<string, string> ();

			var queries = queryString.Split ('&');

			foreach (var query in queries) {
				var q = query.Split ('=');
				if (q.Length == 2) {
					dict.Add (q [0], q [1]);
				}
			}

			return dict;
		}

		public static string ComposeQueryString (Dictionary<string, string> queries)
		{
			var stringBuilder = new StringBuilder ();

			foreach (var query in queries) {
				stringBuilder.Append (string.Format ("{0}={1}&", query.Key, query.Value));
			}

			return stringBuilder.ToString ().TrimEnd ('&');
		}

		public static class JsonParser
		{
			public static T[] FromJson<T> (string json)
			{
				Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>> (json);
				return wrapper.items;
			}

			public static string ToJson<T> (T[] array)
			{
				Wrapper<T> wrapper = new Wrapper<T> ();
				wrapper.items = array;
				return UnityEngine.JsonUtility.ToJson (wrapper);
			}

			[Serializable]
			private class Wrapper<T>
			{
				public T[] items;
			}
		}
	}
}

