using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using System;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using UnityEngine.Events;

public class CloudSaveManager : MonoBehaviour
{
    // Singleton
    public static CloudSaveManager _instance;
    public static CloudSaveManager Instance => _instance;

    private void Awake()
    {
        // Just a basic singleton
        if (_instance is null)
        {
            _instance = this;
            return;
        }

        Destroy(this);
    }

    // Start is called before the first frame update
    async void Start()
    {
        // Initialize unity services
        await UnityServices.InitializeAsync();

        // Setup events listeners
        SetupEvents();

        // Unity Login
        await SignInAnonymouslyAsync();
    }

    void SetupEvents()
    {
        // Testing Quantum terminal
        // Debug.Log($"Hello world!");

        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");
        }
        catch (Exception ex)
        {
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public async void SaveDataCloud()
    {
        Debug.Log("Save to cloud!");


        var data = new Dictionary<string, object> { { "key", "someValue" } };
        data.Add("Key2","3.5");
        
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async void LoadDataCloud()
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "MyKey" });

        Debug.Log("Done: " + savedData["MyKey"]);
    }

    public async void RetrieveKeys()
    {
        List<string> keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i]);
        }
    }
}
