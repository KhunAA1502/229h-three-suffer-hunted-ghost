using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinTrigger : MonoBehaviour
{
    [Header("End Credit Settings")]
    public GameObject endCreditPanel;
    public float delayBeforeShow = 1.5f;
    public AudioClip winSound;

    [Header("Player Settings")]
    public string playerTag = "Player";
    public GameObject playerGameObject;

    private bool hasTriggered = false;

    private void Start()
    {
        if (endCreditPanel != null)
        {
            endCreditPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("End Credit Panel is not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !hasTriggered)
        {
            hasTriggered = true;
            TriggerWin();
        }
    }

    private void TriggerWin()
    {
        // หยุดการเคลื่อนไหวของเกม
        Time.timeScale = 0f;

        // ปิดการควบคุมผู้เล่น
        if (playerGameObject != null)
        {
            playerGameObject.SetActive(false);
        }

        // เล่นเสียงชนะ
        if (winSound != null)
        {
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
        }

        // แสดง End Credit หลังจากหน่วงเวลา
        Invoke("ShowEndCredit", delayBeforeShow);
    }

    private void ShowEndCredit()
    {
        if (endCreditPanel != null)
        {
            endCreditPanel.SetActive(true);
            Debug.Log("End Credit should be visible now");
        }
    }

    // ฟังก์ชันสำหรับปุ่ม UI
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // เปลี่ยนเป็นชื่อ Scene เมนูหลักของคุณ
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}