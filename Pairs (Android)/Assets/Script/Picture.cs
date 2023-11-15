using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour
{
    public AudioClip pressSound;
    private Material _firstMaterial;
    private Material _secondMaterial;
    private Quaternion _currentRotation;
    private new string name;

    private AudioSource _audio;
    private AudioSource _audioClip;

    [HideInInspector]
    public bool revealed = false;

    private PictureManager _pictureManager;
    private int _index;
    private bool clicked = false;

    void Start()
    {
        revealed = false;
        clicked = false;
        _pictureManager = GameObject.Find("[PictureManager]").GetComponent<PictureManager>();
        _currentRotation = gameObject.transform.rotation;
        _audio = GetComponent<AudioSource>();
        _audio.clip = pressSound;
    }

    // Update is called once per frame
    void Update() { }

    public void setIndex(int new_id)
    {
        _index = new_id;
    }

    public int getIndex()
    {
        return _index;
    }

    private void OnMouseDown()
    {
        if (!_pictureManager.waitForFlip && !_pictureManager.waitForDestroy && !clicked)
        {
            _pictureManager.currentPuzzleState = PictureManager.PuzzleState.PuzzleRotating;
            if (GameSettings.Instance.isSoundEffectMutedPermanently() == false)
            {
                _audio.Play();
            }
            _pictureManager.cardCont = _pictureManager.cardCont + 1;
            if (_pictureManager.cardCont == 2) {
                _pictureManager.cardCont = 0;
                _pictureManager.waitForFlip = true;
                Invoke("wait", 1f);
            }
            clicked = true;
            StartCoroutine(LoopRotation(45, false));
        }
    }

    public void wait()
    {
        _pictureManager.waitForFlip = false;
    }

    public void flipBack()
    {
        if (gameObject.activeSelf)
        {
            _pictureManager.currentPuzzleState = PictureManager.PuzzleState.PuzzleRotating;
            revealed = false;
            clicked = false;
            if (GameSettings.Instance.isSoundEffectMutedPermanently() == false)
            {
                _audio.Play();
            }
            StartCoroutine(LoopRotation(45, true));
        }
    }

    public void setPictureName(string newname)
    {
        name = newname;
    }

    public void setClip(string clipName)
    {
        //_audioClip.clip = Resources.Load<AudioClip>("Graphics/PuzzleCat/Animals/Pic2A");
    }

    public void playClip()
    {
        _audioClip.Play();
    }

    public string getPictureName()
    {
        return name;
    }

    IEnumerator LoopRotation(float angle, bool FirstMat)
    {
        var rotation = 0f;
        const float direction = 1f;
        const float rotationSpeed = 180.0f;
        const float rotationSpeed2 = 90.0f;

        var start_angle = angle;
        var assigned = false;
        if (FirstMat)
        {
            while (rotation < angle)
            {
                var step = Time.deltaTime * rotationSpeed2;
                gameObject
                    .GetComponent<Transform>()
                    .Rotate(new Vector3(0, 2, 0) * step * direction);
                if (rotation >= (start_angle - 3) && assigned == false)
                {
                    applyFirstMaterial();
                    assigned = true;
                }
                var adder = (1 * step * direction);
                rotation += adder;
                yield return null;
            }
        }
        else
        {
            while (angle > 0)
            {
                float step = Time.deltaTime * rotationSpeed;
                gameObject
                    .GetComponent<Transform>()
                    .Rotate(new Vector3(0, 2, 0) * step * direction);
                var adder = (1 * step * direction);
                angle -= adder;
                yield return null;
            }
        }
        gameObject.GetComponent<Transform>().rotation = _currentRotation;
        if (!FirstMat)
        {
            revealed = true;
            applySecondMaterial();
            _pictureManager.checkPicture();
        }
        else
        {
            _pictureManager.puzzleRevealedNumber = PictureManager.RevealedState.NoReavealed;
            _pictureManager.currentPuzzleState = PictureManager.PuzzleState.CanRotate;
        }
    }

    public void setFirstMaterial(Material mat, string texturePath)
    {
        _firstMaterial = mat;
        _firstMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void setSecondMaterial(Material mat, string texturePath)
    {
        _secondMaterial = mat;
        _secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void applyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _firstMaterial;
    }

    public void applySecondMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _secondMaterial;
    }

    public void Deactivate()
    {
        StartCoroutine(DeactivateCoRoutine());
    }

    private IEnumerator DeactivateCoRoutine()
    {
        revealed = false;
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    public void DeactiveNoTime()
    {
        gameObject.SetActive(false);
    }

    public void ActivatePicture()
    {
        gameObject.SetActive(true);
    }
}
