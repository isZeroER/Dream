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

    public SceneName currentScene;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Invoke(nameof(StartBlack), 1f); 
        if(Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.KeypadEnter))
            Application.Quit();
    }

    private void StartBlack() => StartCoroutine(FadeInOut(1, 0));
    
    public void ChangeToScene(string name)
    {
        FadeInOutWithCall(() =>
        {
            StartCoroutine(LoadSceneWithName(name));
            if (name == SceneName.StartScene.ToString())
                UIManager.Instance.OpenPanel(UIName.BeginPanel);
        });
    }

    IEnumerator LoadSceneWithName(string name)
    {
        if (Enum.TryParse(name, true, out SceneName theCurrentScene))
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);
            while (!ao.isDone)
            {
                yield return null;
            }
            
            currentScene = theCurrentScene;
        }
        else
        {
            Debug.LogError("Invalid scene name: " + name);
        }
    }

    #region ForSmooth

    public void FadeInOutWithCall(UnityAction call, float time = 1.1f)
    {
        StartCoroutine(CoFadeInOutWithCall(call, time));
    }

    IEnumerator CoFadeInOutWithCall(UnityAction call, float time)
    {
        StartCoroutine(FadeInOut(0, 1));
        yield return new WaitForSeconds(time);
        call.Invoke();
        yield return new WaitForSeconds(time);
        StartCoroutine(FadeInOut(1, 0));
    }
    IEnumerator FadeInOut(float start, float end)
    {
        Image sr = blackFade.GetComponent<Image>();
        
        sr.color = new Color(1, 1, 1, start);

        float forTime = 0.9f;
        float elapsedTime = 0f;
        while (elapsedTime < forTime)
        {
            float alpha = Mathf.Lerp(start, end, elapsedTime / forTime);
            sr.color = new Color(1, 1, 1, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    #endregion
}


public enum SceneName
{
    StartScene,
    Section_0,
    Section_1,
    Section_2
}