using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public sealed class TimerScript : MonoBehaviour
{
    public event Action OnStarted;

    public event Action OnTimeChanged;

    public event Action OnCanceled;

    public event Action OnFinished;


    public bool IsPlaying { get; private set; }

    public float Progress
    {
        get { return this.currentTime / this.duration; }
    }

    public float Duration
    {
        get { return this.duration; }
    }

    public float CurrentTime
    {
        get { return this.currentTime; }
        set { this.currentTime = Mathf.Clamp(value, 0, this.duration); }
    }

    [SerializeField]
    private float duration = 5.0f;

    [Space]
    [SerializeField]
    private UnityEvent onStart;

    [SerializeField]
    private UnityEvent onFinish;

    [SerializeField]
    private UnityEvent onCancel;

    private float currentTime;

    private Coroutine coroutine;

    public void SetDuration(float durationLength)
    {
        this.duration = Mathf.Max(durationLength, 0);
        this.currentTime = 0;
    }

    public void Play()
    {
        if (this.IsPlaying)
        {
            return;
        }

        this.IsPlaying = true;
        this.onStart?.Invoke();
        this.OnStarted?.Invoke();
        this.coroutine = this.StartCoroutine(this.TimerRoutine());
    }

    public void Cancel()
    {
        if (this.coroutine != null)
        {
            this.StopCoroutine(this.coroutine);
        }

        if (this.IsPlaying)
        {
            this.IsPlaying = false;
            this.onCancel?.Invoke();
            this.OnCanceled?.Invoke();
        }
    }

    private IEnumerator TimerRoutine()
    {
        this.currentTime = 0;
        while (this.currentTime < this.duration)
        {
            this.currentTime += Time.deltaTime;
            this.OnTimeChanged?.Invoke();
            yield return null;
        }

        this.IsPlaying = false;
        this.onFinish?.Invoke();
        this.OnFinished?.Invoke();
    }
}