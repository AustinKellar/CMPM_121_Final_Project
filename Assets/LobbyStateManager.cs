using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStateManager : MonoBehaviour
{
    public static LobbyStateManager Instance;

    public enum LobbyState { TitleScreen, ColorSelect }
    public LobbyState State { get; set; }

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Vector3 _colorSelectCameraPosition;

    [SerializeField]
    private Vector3 _colorSelectCameraRotation;

    [SerializeField]
    private float _distanceThreshold = 0.5f;

    private Vector3 _startingPosition;
    private Vector3 _startingRotation;
    private Vector3 _destination;
    private Vector3 _destinationRotation;

    public bool CameraAtDestination
    {
        get
        {
            return Vector3.Distance(_camera.transform.position, _destination) < _distanceThreshold;
        }
    }

    private void Awake()
    {
        KeepOnlyOneInstance();
        State = LobbyState.TitleScreen;
        _destination = _startingPosition = _camera.transform.position;
        _destinationRotation = _startingRotation = _camera.transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _destination, 0.05f);
        _camera.transform.eulerAngles = Vector3.Lerp(_camera.transform.eulerAngles, _destinationRotation, 0.05f);
    }

    private void KeepOnlyOneInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Play()
    {
        _destination = _colorSelectCameraPosition;
        _destinationRotation = _colorSelectCameraRotation;
        Invoke("SetStateToColorSelect", 0.5f);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReturnToTitleScreen()
    {
        _destination = _startingPosition;
        _destinationRotation = _startingRotation;
        State = LobbyState.TitleScreen;
    }

    private void SetStateToColorSelect()
    {
        State = LobbyState.ColorSelect;
    }
}
