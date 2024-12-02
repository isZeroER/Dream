using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePanel : BasePanel
{
    [SerializeField] private TextMeshProUGUI messageText;
    public void SetupMessage(string message)
    {
        StartCoroutine(TypeText(message));
    }
    private IEnumerator TypeText(string fullText)
    {
        messageText.text = "";  // 清空当前显示的文字

        foreach (char letter in fullText)
        {
            messageText.text += letter;  // 每次添加一个字符
            yield return new WaitForSeconds(.1f);  // 等待指定的时间后再打印下一个字符
        }
        
        Invoke(nameof(Close), 5f);
    }
}
