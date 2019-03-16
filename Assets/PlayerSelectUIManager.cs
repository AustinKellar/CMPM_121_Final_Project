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
    private GameObject _playerIconsPanel;

    [SerializeField]
    private GameObject _playerIconContainer;

    [SerializeField]
    private GameObject _startMatch;

    [SerializeField]
    private GameObject _startMatchIcons;

    [SerializeField]
    private float _playerJoinTween;

    private Tweener _tweener;
    private Tweener _startMatchTweener;

    public bool ReadyToStartMatch { get { return _startMatch.activeSelf; } }

    private void Awake()
    {
        KeepOnlyOneInstance();
        foreach (Image icon in GetIcons(true))
        {
            icon.gameObject.SetActive(false);
        }
        _playerIconsPanel.SetActive(false);
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

    public void Reset()
    {
        foreach (Image icon in GetIcons(true))
        {
            icon.gameObject.SetActive(false);
        }
        _playerIconsPanel.SetActive(false);
        _controls.SetActive(true);
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
        _playerIconsPanel.SetActive(true);
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
            _playerIconsPanel.SetActive(false);
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
        _tweener = image.transform.DOPunchScale(Vector3.one * _playerJoinTween, 0.3f, 9, 1);
    }

    private IEnumerator DisplayStartMatch()
    {
        bool previouslyDisabled = true;
        while (true)
        {
            if (LobbyStateManager.Instance.State == LobbyStateManager.LobbyState.ColorSelect)
            {
                if (ReadyForStartMatchUI())
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
            }
            else
            {
                _lobbyUI.SetActive(false);
                _startMatch.SetActive(false);
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

    private bool ReadyForStartMatchUI()
    {
        if (GetIcons(false).Length < 2)
        {
            return false;
        }

        foreach (PlayerInfo player in ActivePlayers.Players)
        {
            if (!ExistsMatchingReadyPlayer(player))
            {
                return false;
            }
        }

        return true;
    }

    private bool ExistsMatchingReadyPlayer(PlayerInfo player)
    {
        Image[] icons = GetIcons(true);
        return icons[player.ControllerNumber - 1].gameObject.activeSelf;
    }

    private Image[] GetIcons(bool getInactiveIcons)
    {
        return _playerIconContainer.GetComponentsInChildren<Image>(getInactiveIcons).OrderBy(i => i.gameObject.name).ToArray();
    }

    private void UpdateStartMatchIcons()
    {
        if (!_lobbyUI.activeSelf)
        {
            return;
        }

        Image[] selectedPlayers = GetIcons(true);
        Image[] newIcons = _startMatchIcons.GetComponentsInChildren<Image>(true).OrderBy(i => i.gameObject.name).ToArray();

        for (int i = 0; i < 4; i++)
        {
            newIcons[i].gameObject.SetActive(selectedPlayers[i].IsActive());
            newIcons[i].color = selectedPlayers[i].color;
        }
    }
}
