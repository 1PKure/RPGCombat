using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowUI : MonoBehaviour
{
    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnContinueButton()
    {
        UIManager.Instance.HideEndPanel();
        GameManager.Instance.ChangeState(GameState.Exploring);
    }
}
