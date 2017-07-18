﻿using UnityEngine;
using UnityEngine.UI;

public class RecoverPassword : MonoBehaviour
{
	
	public GameObject tokenRequestPanel, tokenInputPanel;

	public Text recoveryTokenInput;
	public Text emailInput, passwordInput, confirmPasswordInput;
	public Text recoveryTokenErrorText, emailErrorText, passwordErrorText, confirmPasswordErrorText;

	public void ForgotPasswordButton ()
	{

		MainMenuController.darkness.SetActive(true);
		this.gameObject.SetActive(true);
		tokenRequestPanel.SetActive(true);
		tokenInputPanel.SetActive(false);

		Initialize();
	}

	private void Initialize ()
	{

		emailErrorText.text = "";
		passwordErrorText.text = "";
		confirmPasswordErrorText.text = "";
	}

	public void SendRecoveryTokenRequest ()
	{

		Initialize();

		Debug.Log("Sending Recovery Token Request...");

		var data = new GameSparks.Core.GSRequestData()
			.AddString("email", emailInput.text)
			.AddString("action", "passwordRecoveryRequest");

		new GameSparks.Api.Requests.AuthenticationRequest()
			.SetUserName("")
			.SetPassword("")
			.SetScriptData(data)
			.Send((response) =>
			{

					if (response.Errors.GetString("action") == "complete")
					{

						Debug.Log (string.Format("Request Token Sent To Email : {0}", emailInput.text));
						tokenInputPanel.SetActive(true);
						tokenRequestPanel.SetActive(false);
					}
					else
					{

						Debug.Log ("Error Requesting Token... \n " + response.Errors.JSON.ToString());
						emailErrorText.text = "Invalid E-Mail. Please Try Again.";
					}
			});
	}

	public void ConfirmRecoveryToken ()
	{

		Initialize();

		bool error = false;
		if (confirmPasswordInput.text != passwordInput.text)
		{

			error = true;
		}

		if (error)
		{

			return;
		}

		var data = new GameSparks.Core.GSRequestData()
			.AddString("token", recoveryTokenInput.text)
			.AddString("password", passwordInput.text)
			.AddString("action", "resetPassword");

		Debug.Log("Contacting...");

		new GameSparks.Api.Requests.AuthenticationRequest()
			.SetUserName("")
			.SetPassword("")
			.SetScriptData(data)
			.Send((response) =>
			{

					if (response.Errors.GetString("action") == "complete")
					{

						Debug.Log ("Password change Successful!");
						this.gameObject.SetActive(false);
					}
					else
					{

						Debug.Log ("Error... \n " + response.Errors.JSON.ToString());
					}
			});
	}
}