using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelectUIManager : MonoBehaviour
{
    public static PlayerSelectUIManager Instance;

    [SerializeField]
    private string _matchScene;

    [SerializeField]
    private GameObject _lobbyUI;

    [SerializeField]
    private GameObject _controls;

    [SerializeField]
    private GameObject _playerIcons;

    [SerializeField]
    private GameObject _startMatch;

    [SerializeField]
    private float _clearShakeTimer = 0.5f;

    [SerializeField]
    private GameObject _startMatchIcons;

    [SerializeField]
    private float _clearShakeMagnitude = 0.3f;

    private Tweener _tweener;
    private Tweener _startMatchTweener;
    private int _readyCount = 0;

    public bool ReadyToStartMatch { get { return _startMatch.active; } }

    private void Awake()
    {
        KeepOnlyOneInstance();
    }

    private void Start()
    {
        StartCoroutine(DisplayStartMatch());
    }

    public void StartMatch()
    {
        if (AudioManager.Instance.IsPlaying("Menu Theme"))
        {
            AudioManager.Instance.FadeOutSound("Menu Theme", 0.5f);
        }
        SceneManager.LoadScene(_matchScene);
    }

    public void ReadyPlayer(PlayerIndex index)
    {
        PlayerInfo player = ActivePlayers.Players.FirstOrDefault(p => p.ControllerNumber == (int)index);
        if (player == null)
        {
            return;
        }

        Material material = player.Material;

        _controls.SetActive(false);
        _playerIcons.SetActive(true);
        DisplayPlayer(index);
        SetColor(index, material);
    }

    public void RemovePlayer(PlayerIndex index)
    {
        Image[] playerImages = GetIcons(true);
        Image image = playerImages[(int)index - 1];

        playerImages[(int)index - 1].gameObject.SetActive(false);

        if (GetIcons(false).Length == 0)
        {
            _playerIcons.SetActive(false);
            _controls.SetActive(true);
        }
    }

    public void UpdateColor(PlayerIndex index, Material material)
    {
        SetColor(index, material);
    }

    private void DisplayPlayer(PlayerIndex index)
    {
        Image[] playerImages = GetIcons(true);
        Image image = playerImages[(int)index - 1];
        image.gameObject.SetActive(true);
    }

    private void SetColor(PlayerIndex index, Material material)
    {
        Image[] playerImages = GetIcons(true);
        Image image = playerImages[(int)index - 1];

        Color newColor = material.color;
        newColor.a = 1f;
        image.color = newColor;

        _tweener.Complete();
        _tweener = image.transform.DOPunchScale(Vector3.one * .5f, 0.3f);
    }

    private IEnumerator DisplayStartMatch()
    {
        bool previouslyDisabled = true;
        while (true)
        {
            if (GetIcons(false).Length == ActivePlayers.Players.Count && ActivePlayers.Players.Count >= 2)
            {
                UpdateStartMatchIcons();
                _startMatch.SetActive(true);
                _lobbyUI.SetActive(false);
                if (previouslyDisabled)
                {
                    ShakeStartMatch();
                }
                previouslyDisabled = false;
            }
            else
            {
                _startMatch.SetActive(false);
                _lobbyUI.SetActive(true);
                previouslyDisabled = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
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

    private void ShakeStartMatch()
    {
        _startMatchTweener.Complete();
        _startMatchTweener = _startMatch.transform.DOPunchScale(Vector3.one * 1.1f, 0.2f);
    }

    private Image[] GetIcons(bool getInactiveIcons)
    {
        return _playerIcons.GetComponentsInChildren<Image>(getInactiveIcons).OrderBy(i => i.gameObject.name).ToArray();
    }

    private void UpdateStartMatchIcons()
    {
        if (!_lobbyUI.active)
        {
            return;
        }

        Image[] selectedPlayers = GetIcons(true);
        Image[] newIcons = _startMatchIcons.GetComponentsInChildren<Image>(true).OrderBy(i => i.gameObject.name).ToArray();

        for (int i = 0; i < 4; i++)
        {
            newIcons[i].gameObject.SetActive(selectedPlayers[i].gameObject.active);
            newIcons[i].color = selectedPlayers[i].color;
        }
    }
}
