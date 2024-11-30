using System;
using System.Collections;
using System.Collections.Generic;
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

    private void OnEnable()
    {
        Invoke(nameof(StartBlack), 1f);
    }

    private void StartBlack() => StartCoroutine(FadeInOut(1, 0));
    
    public void ChangeToScene(string name)
    {
        StartCoroutine(CoChangeToScene(name));
    }

    public void FadeInOutWithCall(UnityAction call)
    {
        StartCoroutine(CoFadeInOutWithCall(call));
    }

    IEnumerator CoChangeToScene(string name)
    {
        StartCoroutine(FadeInOut(0, 1));
        yield return new WaitForSeconds(2f);
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            yield return null;
            if (Enum.TryParse(name, out SceneName scene))
            {
                currentScene = scene;
            } 
            else
                Debug.Log("Invalid SceneName");
        }
        
        UIManager.Instance.ClosePanel(UIName.BeginPanel);
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

    IEnumerator CoFadeInOutWithCall(UnityAction call)
    {
        StartCoroutine(FadeInOut(0, 1));
        yield return new WaitForSeconds(1f);
        call();
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeInOut(1, 0));
    }

}


public enum SceneName
{
    StartScene,
    Section_0,
    Section_1,
    Section_2
}