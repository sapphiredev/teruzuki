﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using teruzuki.Twitter;

namespace teruzuki
{
	public class AccountController : MonoBehaviour
	{
		public RectTransform content;
		public RectTransform accountItem;

		private IEnumerator initializeItemCoroutine;

		private Client client;
		private IEnumerator requestTokenCoroutine;
		private IEnumerator accessTokenCoroutine;

		public GameObject accountList;
		public GameObject newAccount;

		void Start ()
		{
			initializeItemCoroutine = InitializeItem ();
			StartCoroutine (initializeItemCoroutine);

			newAccount.SetActive (false);
		}

		private IEnumerator InitializeItem ()
		{
			foreach (var account in AccountManager.Instance.AccountList)
			{
				var item = Instantiate (accountItem);
				item.SetParent (content, false);
				item.GetComponent<UIAccountItem> ().Initialize (account);

				yield return new WaitForSeconds (0.1f);
			}
			yield return null;
		}

		private float opacityLeft = 0.0f;
		private float opacitySpeed = 2.0f;

		public void UIAddNewAccount ()
		{
			client = new Client ();

			newAccount.SetActive (true);

			newAccount.transform.FindChild ("PINInputField").GetComponentInChildren<Text> ().text = "";

			var color = newAccount.GetComponent<Image> ().color;
			color.a = 0.0f;
			newAccount.GetComponent<Image> ().color = color;

			opacityLeft = 1.0f;

			if (requestTokenCoroutine == null)
			{
				requestTokenCoroutine = client.AcquireRequestToken (RequestTokenCallback);
				StartCoroutine (requestTokenCoroutine);
			}
		}

		void Update ()
		{
			if (opacityLeft > 0.0f)
			{
				var opacity = opacitySpeed * Time.deltaTime;
				if (opacityLeft < opacity)
				{
					opacity = opacityLeft;
					opacityLeft = 0.0f;
				}
				else
				{
					opacityLeft -= opacity;
				}
				var color = newAccount.GetComponent<Image> ().color;
				color.a += opacity;
				newAccount.GetComponent<Image> ().color = color;
			}
		}

		public void UINewAccountSubmit ()
		{
			var pin = newAccount.transform.FindChild ("PINInputField").GetComponentInChildren<Text> ().text;

			if (accessTokenCoroutine == null)
			{
				accessTokenCoroutine = client.AcquireAccessToken (pin, AccessTokenCallback);
				StartCoroutine (accessTokenCoroutine);
			}
		}

		public void UINewAccountCancel ()
		{
			newAccount.SetActive (false);

			if (requestTokenCoroutine != null) {
				try {
					StopCoroutine (requestTokenCoroutine);
					requestTokenCoroutine = null;
				}
				catch (Exception e) {
					Debug.Log (e);
				}
			}
			if (accessTokenCoroutine != null) {
				try {
					StopCoroutine (accessTokenCoroutine);
					accessTokenCoroutine = null;
				}
				catch (Exception e) {
					Debug.Log (e);
				}
			}
		}

		private void RequestTokenCallback (string url)
		{
			Application.OpenURL (url);

			requestTokenCoroutine = null;
		}

		private void AccessTokenCallback ()
		{
			newAccount.SetActive (false);

			StartCoroutine (Twitter.API.Account.VerifyCredentials (client, new Twitter.Parameters.Account.VerifyCredentialsParameters (), VerifyCredentialsCallback));

			accessTokenCoroutine = null;
		}

		private void VerifyCredentialsCallback(Twitter.Model.User user) {
			var account = new Account (client, user);
//			AccountManager.Instance.InsertAccount (account);

			StartCoroutine (account.Initialize (AccountInitializeCallback));
		}

		private void AccountInitializeCallback(Account account) {
			var item = Instantiate (accountItem);
			item.SetParent (content, false);
			item.GetComponent<UIAccountItem> ().Initialize (account);
		}
	}
}
