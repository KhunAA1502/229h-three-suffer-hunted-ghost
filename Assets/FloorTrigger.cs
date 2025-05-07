using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameTrigger : MonoBehaviour
{
    [Header("Credit UI Settings")]
    public GameObject creditPanel; // Panel ที่แสดงหน้า Credit
    public float delayBeforeShow = 1f; // เวลาหน่วงก่อนแสดง Credit

    [Header("Sound Effects")]
    public AudioClip endGameSound;

    private bool hasTriggered = false;

    private void Start()
    {
        // ซ่อน Credit Panel ตอนเริ่มเกม
        if (creditPanel != null)
        {
            creditPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            TriggerEndGame();
        }
    }

    private void TriggerEndGame()
    {
        // หยุดการเคลื่อนไหวของเกม
        Time.timeScale = 0f;

        // เล่นเสียงจบเกม
        if (endGameSound != null)
        {
            AudioSource.PlayClipAtPoint(endGameSound, Camera.main.transform.position);
        }

        // แสดงหน้า Credit หลังจากหน่วงเวลา
        Invoke("ShowCredit", delayBeforeShow);
    }

    private void ShowCredit()
    {
        if (creditPanel != null)
        {
            creditPanel.SetActive(true);
            
            // ซ่อนตัวผู้เล่นหรือองค์ประกอบอื่นๆ ที่ไม่ต้องการให้เห็น
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) player.SetActive(false);
        }
        else
        {
            Debug.LogError("Credit Panel is not assigned!");
        }
    }

    // ฟังก์ชันสำหรับปุ่ม UI
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // คืนค่าเวลาให้ปกติ
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