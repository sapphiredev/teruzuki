﻿using System;

namespace teruzuki.Twitter
{
	public static class Constants
	{
		public static class URL {
			private static readonly string PROTOCOL = "https";
			private static readonly string HOSTNAME = "api.twitter.com";
			private static readonly string VERSION = "1.1";

			public static string BASE_URL { get { return string.Format("{0}://{1}/{2}/", PROTOCOL, HOSTNAME, VERSION); } }

			public static readonly string REQUEST_TOKEN = string.Format("{0}://{1}/oauth/request_token", PROTOCOL, HOSTNAME);
			public static readonly string ACCESS_TOKEN = string.Format("{0}://{1}/oauth/access_token", PROTOCOL, HOSTNAME);
		}

		public static class Credentials {
			public static readonly string FILE_NAME = "credentials.teruzuki";
		}

		public static class Key {
			public static readonly string CONSUMER_KEY = "OcbDuSiWrHYWU2RFgdWyV61F8";
			public static readonly string CONSUMER_SECRET = "7fNW3QITGNFQAisvtkk8yaHdXkx5j7mxM2rEJShUeqxbwZEDHZ";
		}
	}
}
	