﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phoneword {
	
	[Activity(Label = "Phone Word", MainLauncher = true)]

	public class MainActivity : Activity {
		
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Our code will go here

			// Get our UI controls from the loaded layout
			EditText phoneNumberText = FindViewById<EditText>(Resource.Id.phoneNumberText);
			Button translateButton = FindViewById<Button>(Resource.Id.translateButton);
			Button callButton = FindViewById<Button>(Resource.Id.callButton);

			// Disbale to call button
			callButton.Enabled = false;

			// Add code to translate button
			string translatedNumber = string.Empty;

			translateButton.Click += (object sender, EventArgs e) => {

				// Translate user's alphanumeric phone number to numeric
				translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
				if (string.IsNullOrWhiteSpace(translatedNumber)) {
					callButton.Text = "Call";
					callButton.Enabled = false;
				} else {
					callButton.Text = "Call " + translatedNumber;
        	callButton.Enabled = true;
				}
			};

			// Add code to the call button
			callButton.Click += (object sender, EventArgs e) => {
				var callDialog = new AlertDialog.Builder(this);
				callDialog.SetMessage("Call " + translatedNumber + "?");

				callDialog.SetNeutralButton("Call", delegate {
					// Create intent to dial phone
					var callIntent = new Intent(Intent.ActionCall);
					callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
					StartActivity(callIntent);
				});

				callDialog.SetNegativeButton("Cancel", delegate { });

				// Show the alert dialog to the user and wait for a response
				callDialog.Show();
			};

		}
	}
}

