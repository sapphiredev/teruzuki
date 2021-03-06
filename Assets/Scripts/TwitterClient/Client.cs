﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

using teruzuki.Twitter;

namespace teruzuki.Twitter
{
	public class Client
	{
		private OAuth oAuth;

		public Token Token
		{
			get;
			private set;
		}

		public bool isReady = false;

		public Client () : this (new Token ())
		{

		}

		public Client (Token token)
		{
			this.Token = token;
			this.oAuth = new OAuth (token);
		}

		public IEnumerator AcquireRequestToken (Action<string> callback)
		{
			Dictionary<string, string> headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", oAuth.GetAuthorizationHeader (Constants.URL.REQUEST_TOKEN, "POST", null));

			WWWForm wwwForm = new WWWForm ();
			wwwForm.AddField ("", "");

			WWW www = new WWW (Constants.URL.REQUEST_TOKEN, wwwForm.data, headers);
			yield return www;

			try {
				var query = Helper.ParseQueryString (www.text);
				oAuth.OAuthToken = query["oauth_token"];

				callback (string.Format ("https://api.twitter.com/oauth/authenticate?oauth_token={0}", query["oauth_token"]));
			}
			catch (Exception e)
			{
				Debug.Log (www.text);
				Debug.Log (e);
			}
		}

		public IEnumerator AcquireAccessToken (string pin, Action callback)
		{
			oAuth.OAuthVerifier = pin;

			Dictionary<string, string> headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", oAuth.GetAuthorizationHeader (Constants.URL.ACCESS_TOKEN, "POST", null));

			WWWForm wwwForm = new WWWForm ();
			wwwForm.AddField ("", "");

			WWW www = new WWW (Constants.URL.ACCESS_TOKEN, wwwForm.data, headers);
			yield return www;

			try
			{
				var query = Helper.ParseQueryString (www.text);
				oAuth.OAuthToken = Token.AccessToken = query["oauth_token"];
				oAuth.OAuthTokenSecret = Token.AccessTokenSecret = query["oauth_token_secret"];

				isReady = true;

				callback ();
			}
			catch (Exception e)
			{
				Debug.Log (www.text);
				Debug.Log (e);
			}
		}

		public IEnumerator GET<T> (string url, Parameters.ITwitterParameters parameters, Action<T> callback)
		{
			url += parameters.ComposeQueryString ();

			Dictionary<string, string> headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", oAuth.GetAuthorizationHeader (url, "GET", parameters));

			WWW www = new WWW (url, null, headers);
			yield return www;

			try {
				callback (JsonConvert.DeserializeObject<T> (www.text));
			}
			catch (Exception e)
			{
				Debug.Log (www.text);
				Debug.Log (e);
			}
		}

		public IEnumerator POST<T> (string url, Parameters.ITwitterParameters parameters, Action<T> callback)
		{
			WWWForm wwwForm = new WWWForm ();
			parameters.Queries.ToList ().ForEach (x =>
			{
				wwwForm.AddField (x.Key, x.Value);
			});

			Dictionary<string, string> headers = new Dictionary<string, string> ();
			headers.Add ("Authorization", oAuth.GetAuthorizationHeader (url, "POST", parameters));

			WWW www = new WWW (url, wwwForm.data, headers);
			yield return www;

			try
			{
				callback (JsonConvert.DeserializeObject<T> (www.text));
			}
			catch (Exception e)
			{
				Debug.Log (www.text);
				Debug.Log (e);
			}
		}
	}
}
