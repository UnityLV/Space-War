using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;



public class SwitchTransition : Transition
{
    private bool _flag;
    
    public SwitchTransition(IState fromState, IState toState) : base(fromState, toState)
    {
    }

    public override void OnEnter()
    {
        _flag = false;
    }

    public void Switch()
    {
        _flag = true;
    }

    public override bool Condition()
    {
        return _flag;
    }
}

public class StartButtonTransition : Transition
{
    private bool _isButtonPressed;
    private Button _startButton;

    public StartButtonTransition(IState fromState, IState toState) : base(fromState, toState)
    {
    }

    public override void OnEnter()
    {
        _isButtonPressed = false;
        
        GameObject canvasObject = Tools.GetCanvas().gameObject;

        GameObject buttonPrefab = Resources.Load<GameObject>("Art/Button");

        GameObject buttonObject = Object.Instantiate(buttonPrefab, canvasObject.transform);
        _startButton = buttonObject.GetComponent<Button>();

        _startButton.onClick.AddListener(OnButtonPressed);
    }

    public override void OnExit()
    {
        if (_startButton != null)
        {
            _startButton.onClick.RemoveAllListeners();
            Object.Destroy(_startButton.gameObject);
        }
    }

    public override bool Condition()
    {
        return _isButtonPressed;
    }

    private void OnButtonPressed()
    {
        _isButtonPressed = true;
    }
}

public class WatchReplayButtonTransition : Transition
{
    private bool _isButtonPressed;
    private Button _watchReplayButton;

    public WatchReplayButtonTransition(IState fromState, IState toState) : base(fromState, toState)
    {
    }

    public override void OnEnter()
    {
        _isButtonPressed = false;
        
        GameObject canvasObject = Tools.GetCanvas().gameObject;

        GameObject buttonPrefab = Resources.Load<GameObject>("Art/Button");

        GameObject buttonObject = Object.Instantiate(buttonPrefab, canvasObject.transform);
        _watchReplayButton = buttonObject.GetComponent<Button>();

        RectTransform rectTransform = _watchReplayButton.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(10, 10);
        
        _watchReplayButton.GetComponentInChildren<TextMeshProUGUI>().text = "Watch Replay";
        _watchReplayButton.onClick.AddListener(OnButtonPressed);
    }

    public override void OnExit()
    {
        if (_watchReplayButton != null)
        {
            _watchReplayButton.onClick.RemoveAllListeners();
            Object.Destroy(_watchReplayButton.gameObject);
        }
    }

    public override bool Condition()
    {
        return _isButtonPressed;
    }

    private void OnButtonPressed()
    {
        _isButtonPressed = true;
    }
}