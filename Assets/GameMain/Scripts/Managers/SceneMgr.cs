using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMgr : Singleton<SceneMgr>
{
    [SerializeField] private GameObject blackFade;
    [SerializeField] private GameObject fadeCanvas;
    public SceneName currentScene;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(fadeCanvas);
    }

    private void Start()
    {
        Invoke(nameof(StartBlack), 1f);
    }

    private void StartBlack() => StartCoroutine(FadeInOut(1, 0));
    
    [Obsolete("Obsolete")]
    public void ChangeToScene(string name)
    {
        FadeInOutWithCall(() =>
        {
            SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(name);
        });
    }

    public void FadeInOutWithCall(UnityAction call)
    {
        StartCoroutine(CoFadeInOutWithCall(call));
    }

    IEnumerator CoFadeInOutWithCall(UnityAction call)
    {
        StartCoroutine(FadeInOut(0, 1));
        yield return new WaitForSeconds(1f);
        call();
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeInOut(1, 0));
    }
    IEnumerator FadeInOut(float start, float end)
    {
        Image sr = blackFade.GetComponent<Image>();
        
        sr.color = new Color(1, 1, 1, start);

        float forTime = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < forTime)
        {
            float alpha = Mathf.Lerp(start, end, elapsedTime / forTime);
            sr.color = new Color(1, 1, 1, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}


public enum SceneName
{
    StartScene,
    Section_0,
    Section_1,
    Section_2
}