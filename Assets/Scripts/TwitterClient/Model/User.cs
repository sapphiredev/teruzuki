﻿using System;

namespace teruzuki.Twitter.Model
{
	public class User : ITwitterModel
	{
		public UInt64 id;
		public string name;
		public string id_str;
		public string screen_name;
		public string profile_image_url;
		public Tweet status;
	}
}
