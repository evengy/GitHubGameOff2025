using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineCamera gameCamera;
    [SerializeField] private CinemachineCamera menuCamera;

    Dictionary<GameState, CinemachineCamera> cameras;

    private void Start()
    {
        cameras = new Dictionary<GameState, CinemachineCamera>
        {
            { GameState.InGame, gameCamera },
            { GameState.InMenu, menuCamera },
            { GameState.InGameOver, menuCamera }
        };

        GameStateManager.Instance.StateUpdated += SetCameraPerspectiveForGameState;
    }

    private void SetCameraPerspectiveForGameState()
    {
        if (cameras.Keys.Contains(GameStateManager.Instance.GameState))
        {
            cameras[GameStateManager.Instance.GameState].Priority = 1;
            foreach (var key in cameras.Keys.Where(k => !k.Equals(GameStateManager.Instance.GameState)))
            {
                cameras[key].Priority = 0;
            }
        }
    }

}