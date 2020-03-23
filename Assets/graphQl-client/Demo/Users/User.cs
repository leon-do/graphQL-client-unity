﻿using GraphQlClient.Core;
using GraphQlClient.EventCallbacks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    public GameObject loading;
    
    [Header("API")]
    public GraphApi userApi;

    [Header("Query")]
    public Text queryDisplay;

    [Header("Mutation")]
    public InputField id;
    public InputField username;
    public Text mutationDisplay;
    
    
    [Header("Subscription")]
    public Text subscriptionDisplay;

    private void OnEnable(){
        OnSubscriptionDataReceived.RegisterListener(DisplayData);
    }

    private void OnDisable(){
        OnSubscriptionDataReceived.UnregisterListener(DisplayData);
    }

    public async void GetQuery(){
        loading.SetActive(true);
        UnityWebRequest request = await userApi.Post("GetUsers", GraphApi.Query.Type.Query);
        loading.SetActive(false);
        queryDisplay.text = HttpHandler.FormatJson(request.downloadHandler.text);
    }

    public async void CreateNewUser(){
        loading.SetActive(true);
        GraphApi.Query query = userApi.GetQueryByName("CreateNewUser", GraphApi.Query.Type.Mutation);
        query.SetArgs(new{objects = new{id = id.text, name = username.text}});
        UnityWebRequest request = await userApi.Post(query);
        loading.SetActive(false);
        mutationDisplay.text = HttpHandler.FormatJson(request.downloadHandler.text);
    }

    public void Subscribe(){
        loading.SetActive(true);
        userApi.Subscribe("SubscribeToUsers", GraphApi.Query.Type.Subscription);
        loading.SetActive(false);
    }

    public void DisplayData(OnSubscriptionDataReceived subscriptionDataReceived){
        subscriptionDisplay.text = HttpHandler.FormatJson(subscriptionDataReceived.data);
    }

    public void CancelSubscribe(){
        userApi.CancelSubscription();
    }
}