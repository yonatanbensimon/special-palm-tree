using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    public Sprite[] back;
    public Sprite[] backCandle;
    public Sprite[] backIdle;
    public Sprite[] backCandleIdle;
    public Sprite[] front;
    public Sprite[] frontCandle;
    public Sprite[] frontIdle;
    public Sprite[] frontCandleIdle;

    public SpriteRenderer spriteRenderer;

    bool _isFront;
    bool _isCandle;
    bool _isIdle;
    bool _isFlip;
    public float fps;

    private float _timer;
    private float _timerLength;

    private int _frameIndex;


    private void Start()
    {
        _timerLength = 1f / fps;
        _timer = _timerLength;
        _isFront = true;
        _isCandle = true;
        _isIdle = true;
        _isFlip = false;
    }
    private void FixedUpdate()
    {
        _timer -= Time.fixedDeltaTime;
        if (_timer <= 0)
        {
            _frameIndex = _frameIndex + 1;
            _timer = _timerLength;
        }
        Sprite[] array = null;

        if (_isFront && _isCandle && _isIdle)
        {
            array = frontCandleIdle;
        }
        else if (_isFront && _isCandle && !_isIdle)
        {
            array = frontCandle;
        }
        else if (_isFront && !_isCandle && _isIdle)
        {
            array = frontIdle;
        }
        else if (_isFront && !_isCandle && !_isIdle)
        {
            array = front;
        }
        else if (!_isFront && _isCandle && _isIdle)
        {
            array = backCandleIdle;
        }
        else if (!_isFront && _isCandle && !_isIdle)
        {
            array = backCandle;
        }
        else if (!_isFront && !_isCandle && _isIdle)
        {
            array = backIdle;
        }
        else if (!_isFront && !_isCandle && !_isIdle)
        {
            array = back;
        }

        if (array == null)
        {
            Debug.LogError("Missing array of player sprites somehow defaulting to front");
            array = front;
        }

        spriteRenderer.sprite = array[_frameIndex % array.Length];
        Vector3 flippedScale = spriteRenderer.transform.localScale;
        flippedScale.x = (_isFlip ^ !_isFront) ? -1.0f : 1.0f * Mathf.Abs(flippedScale.x);
        spriteRenderer.transform.localScale = flippedScale;
    }

    public void CandleOn(bool on)
    {
        _isCandle = on; 
    }

    public void Running(bool on)
    {
        _isIdle = !on;
    }

    public void Front(bool on)
    {
        _isFront = on;
    }

    public void Flip(bool on)
    {
        _isFlip = on;
    }

}
