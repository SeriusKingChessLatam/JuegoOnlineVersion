using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Collections;
using TMPro;
using UnityEngine;


public enum CameraAngle
{
    menu = 0,
    whiteTeam = 1,
    blackTeam = 2
}

public class GameUI : MonoBehaviour
{
    public static GameUI Instance {set; get;}

    public Server server;
    public Client client;

    [SerializeField] private Animator menuAnimator;
    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private GameObject[] cameraAngles; //instalar en el package manager cinemachine

    public Action<bool> SetLocalGame;

    private void Awake() {
        Instance = this;
        RegisterEvents();
    }

    //Cameras
    public void ChangeCamera(CameraAngle index){
        for (int i = 0; i < cameraAngles.Length; i++)
        {
            cameraAngles[i].SetActive(false);
        }

        cameraAngles[(int)index].SetActive(true);
    }

    //Buttons
    public void OnLocalGameButtom(){
        menuAnimator.SetTrigger("InGameMenu");
        SetLocalGame?.Invoke(true);
        server.Init(8007); 
        client.Init("127.0.0.1", 8007); 
        
    }
    public void OnOnlineGameButtom(){
        menuAnimator.SetTrigger("OnlineMenu");
    }

    public void OnOnlineHostButton(){
        SetLocalGame?.Invoke(false);
        server.Init(8007); //probablemente no use este puerto pero se puede cambiar
        client.Init("127.0.0.1", 8007); // el cliente debe ser el mismo para todos
        menuAnimator.SetTrigger("HostMenu");
    }
    
    //Instalar en el package manager la extension com.unity.transport 
    // com.unity.transport version 1.1.0
    public void OnOnlineConnectButton(){
        //Problemas con SetLocal
        SetLocalGame?.Invoke(false);
        client.Init(addressInput.text, 8007);
        //Debug.Log("OnOnlineConnectButton"); //$$
        

    }
    
    public void OnOnlineBackButton(){
        menuAnimator.SetTrigger("StartMenu");
    }

    public void OnHostBackButton(){
        server.ShutDown();
        client.ShutDown();
        menuAnimator.SetTrigger("OnlineMenu");
    }

    public void OnLeaveFromGameMenu(){
        ChangeCamera(CameraAngle.menu);
        menuAnimator.SetTrigger("StartMenu");
        

    }

    #region 
    private void RegisterEvents()
    {
        NetUtility.C_START_GAME += OnStartGameClient;
    }

    private void UnRegisterEvents()
    {
        NetUtility.C_START_GAME -= OnStartGameClient;
    }

    private void OnStartGameClient(NetMessage obj)
    {
        menuAnimator.SetTrigger("InGameMenu");

    }

    #endregion



}
