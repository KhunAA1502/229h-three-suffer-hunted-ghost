using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health2 : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public float invincibilityTime = 1f;

    [Header("UI References")]
    public Slider healthSlider;
    public Image healthFillImage;
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;

    [Header("Events")]
    public UnityEvent onDamageTaken;
    public UnityEvent onDeath;
    public UnityEvent onWin; // เพิ่ม Event สำหรับเมื่อชนะ

    [Header("Game Over/Win UI")]
    public GameObject gameOverUI;
    public Text gameOverText;
    public GameObject winUI; // เพิ่ม UI สำหรับเมื่อชนะ
    public Text winText;

    [Header("Win Condition")]
    public string winObjectTag = "Finish"; // Tag ของวัตถุที่ทำให้ชนะ

    private bool isInvincible = false;
    private float invincibilityTimer;
    private bool isDead = false;
    private bool hasWon = false; // เพิ่มตัวแปรตรวจสอบว่าชนะแล้วหรือไม่

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (winUI != null) winUI.SetActive(false);
    }

    private void Update()
    {
        if (isInvincible && !isDead && !hasWon)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจสอบว่าชนวัตถุที่ทำให้ชนะและยังมีเลือดเหลือ
        if (other.CompareTag(winObjectTag) && currentHealth > 0 && !hasWon)
        {
            WinGame();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || isDead || hasWon) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onDamageTaken.Invoke();
        UpdateHealthUI();

        // ตั้งค่าตัวไม่ตายชั่วคราว
        isInvincible = true;
        invincibilityTimer = invincibilityTime;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (hasWon) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }

        if (healthFillImage != null)
        {
            healthFillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, (float)currentHealth / maxHealth);
        }
    }

    private void Die()
    {
        if (isDead || hasWon) return;

        isDead = true;
        onDeath.Invoke();

        // ปิดการควบคุมผู้เล่น
        var playerController = GetComponent<PlayerMovement>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // แสดง UI เกมโอเวอร์
        ShowGameOver();

        // หยุดเกมชั่วคราว
        Time.timeScale = 0f;
    }

    private void WinGame()
    {
        hasWon = true;
        onWin.Invoke();

        // ปิดการควบคุมผู้เล่น
        var playerController = GetComponent<PlayerMovement>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // แสดง UI ชนะ
        ShowWinUI();

        // หยุดเกมชั่วคราว
        Time.timeScale = 0f;
    }

    private void ShowGameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);

            if (gameOverText != null)
            {
                gameOverText.text = "Game Over\nFinal Health: " + currentHealth;
            }
        }
    }

    private void ShowWinUI()
    {
        if (winUI != null)
        {
            winUI.SetActive(true);

            if (winText != null)
            {
                winText.text = "You Win!\nRemaining Health: " + currentHealth;
            }
        }
    }

    // ฟังก์ชันสำหรับปุ่ม Restart (เรียกจาก UI)
    public void RestartGame()
    {
        Time.timeScale = 1f; // คืนค่าเวลาให้ปกติ
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ฟังก์ชันสำหรับปุ่ม Quit (เรียกจาก UI)
    public void QuitGame()
    {
        Application.Quit();

        // สำหรับการทดสอบใน Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
