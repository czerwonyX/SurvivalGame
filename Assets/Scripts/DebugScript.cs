using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Photon.Pun;

public class DebugScript : MonoBehaviour
{
    [SerializeField] Button toggleShadowsButton;
    [SerializeField] Scrollbar shadowsDistanceScrollbar;
    [SerializeField] Button fpsButton1;
    [SerializeField] Button fpsButton2;
    [SerializeField] Button fpsButton3;
    [SerializeField] Button fpsButton4;
    [SerializeField] Button flyButton;
    [SerializeField] Button flyButtonUp;
    [SerializeField] Button flyButtonDown;
    UniversalRenderPipelineAsset urp;
    bool isFlying;
    float defaultGravity;
    private void Awake()
    {
        urp = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;

        toggleShadowsButton.onClick.AddListener(ToggleShadows);
        shadowsDistanceScrollbar.onValueChanged.AddListener(delegate { urp.shadowDistance = shadowsDistanceScrollbar.value * 200; });

        fpsButton1.onClick.AddListener(delegate { Application.targetFrameRate = -1; });
        fpsButton2.onClick.AddListener(delegate { Application.targetFrameRate = 60; });
        fpsButton3.onClick.AddListener(delegate { Application.targetFrameRate = 30; });
        fpsButton4.onClick.AddListener(delegate { Application.targetFrameRate = 5; });

        defaultGravity = Physics.gravity.y;
        flyButton.onClick.AddListener(ToggleFly);
        flyButtonUp.onClick.AddListener(FlyUp);
        flyButtonDown.onClick.AddListener(FlyDown);
    }
    private void ToggleShadows()
    {
        if (urp.shadowDistance < 0.1f)
        {
            urp.shadowDistance = shadowsDistanceScrollbar.value * 200;
            return;
        }
        urp.shadowDistance = 0;
    }
    private void ToggleFly()
    {
        var playerController = ((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerController>();
        if (isFlying)
        {
            isFlying = false;
            Physics.gravity = Vector3.up * defaultGravity;
            flyButtonUp.gameObject.SetActive(false);
            flyButtonDown.gameObject.SetActive(false);
            playerController.setMoveSpeed(playerController.getMoveSpeed() / 5);
            return;
        }
        isFlying = true;
        Physics.gravity = Vector3.zero;
        playerController.Jump();
        playerController.setMoveSpeed(playerController.getMoveSpeed() * 5);
        flyButtonUp.gameObject.SetActive(true);
        flyButtonDown.gameObject.SetActive(true);
    }
    private void FlyUp() {
        var player = ((GameObject)PhotonNetwork.LocalPlayer.TagObject);
        player.GetComponent<CharacterController>().Move(Vector3.up);
    }
    private void FlyDown() {
        var player = ((GameObject)PhotonNetwork.LocalPlayer.TagObject);
        player.GetComponent<CharacterController>().Move(Vector3.down);
    }


}
