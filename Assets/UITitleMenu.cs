using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITitleMenu : MonoBehaviour
{
    [SerializeField]
    private Color _staticColor;

    [SerializeField]
    private Color _hoverColor;

    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private Button _quitButton;

    [SerializeField]
    private LobbyMenuInput _menuInput;

    private List<MenuAction> _actions = null;
    private MenuAction _selectedAction;
    private int _actionIndex;
    private Vector3 _startingScale;

    private void Start()
    {
        _actions = new List<MenuAction>
        {
            new MenuAction(LobbyStateManager.Instance.Play, _playButton),
            new MenuAction(LobbyStateManager.Instance.Quit, _quitButton)
        };

        _actionIndex = 0;
        _selectedAction = _actions[_actionIndex];
        _startingScale = _playButton.transform.localScale;
    }

    private void Update()
    {
        if (_menuInput.DownButtonPressed && _actionIndex < _actions.Count - 1)
        {
            AudioManager.Instance.PlayOneShot("UI Menu Toggle");
            _actionIndex++;
            _selectedAction = _actions[_actionIndex];
        }

        if (_menuInput.UpButtonPressed && _actionIndex > 0)
        {
            AudioManager.Instance.PlayOneShot("UI Menu Toggle");
            _actionIndex--;
            _selectedAction = _actions[_actionIndex];
        }

        if (_menuInput.ExecuteButtonPressed)
        {
            AudioManager.Instance.PlayOneShot("UI Menu Confirm");
            _selectedAction.action.Invoke();
        }

        _actions.ForEach(a =>
        {
            a.button.GetComponent<Image>().color = _staticColor;
            a.button.transform.localScale = _startingScale;
        });
        _selectedAction.button.GetComponent<Image>().color = _hoverColor;
        _selectedAction.button.transform.localScale *= 1.04f;
    }

    private class MenuAction
    {
        public Action action;
        public Button button;

        public MenuAction(Action a, Button b)
        {
            action = a;
            button = b;
        }
    }
}
