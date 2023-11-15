using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteSoundEffectButton : MonoBehaviour
{
    public Sprite unmutedFxSprite;
    public Sprite mutedFxSprite;
    private Button _button;
    private SpriteState _state;

    void Start()
    {
        _button = GetComponent<Button>();
        if (GameSettings.Instance.isSoundEffectMutedPermanently())
        {
            _state.pressedSprite = mutedFxSprite;
            _state.highlightedSprite = mutedFxSprite;
            _button.GetComponent<Image>().sprite = mutedFxSprite;
        }
        else
        {
            _state.pressedSprite = unmutedFxSprite;
            _state.highlightedSprite = unmutedFxSprite;
            _button.GetComponent<Image>().sprite = unmutedFxSprite;
        }
    }

    private void OnGUI()
    {
        if (GameSettings.Instance.isSoundEffectMutedPermanently())
        {
            _button.GetComponent<Image>().sprite = mutedFxSprite;
        }
        else
        {
            _button.GetComponent<Image>().sprite = unmutedFxSprite;
        }
    }

    public void ToggleFxIcon()
    {
        if (GameSettings.Instance.isSoundEffectMutedPermanently())
        {
            _state.pressedSprite = unmutedFxSprite;
            _state.highlightedSprite = unmutedFxSprite;
            GameSettings.Instance.muteSoundEffectPermanently(false);
        }
        else
        {
            _state.pressedSprite = mutedFxSprite;
            _state.highlightedSprite = mutedFxSprite;
            GameSettings.Instance.muteSoundEffectPermanently(true);
        }
        _button.spriteState = _state;
    }
}
