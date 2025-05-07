using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject target;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private int damageAmount = 1;
    
    [Header("Cooldown Settings")]
    [SerializeField] private float shootCooldown = 0.5f; // เวลาคูลดาวน์ระหว่างยิง
    private float lastShootTime; // เวลาที่ยิงครั้งล่าสุด
    private bool canShoot = true; // สถานะสามารถยิงได้หรือไม่
    
    [Header("Bullet Settings")]
    [SerializeField] private float bulletLifetime = 2f; // ระยะเวลาก่อนกระสุนทำลายตัวเอง (วินาที)


    void Update()
    {
        // ตรวจสอบคูลดาวน์
        if (!canShoot && Time.time - lastShootTime >= shootCooldown)
        {
            canShoot = true;
        }

        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.magenta, 5f);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                Shoot(hit.point);
            }
        }
    }

    private void Shoot(Vector2 targetPoint)
    {
        // ตั้งค่าคูลดาวน์
        lastShootTime = Time.time;
        canShoot = false;

        target.transform.position = targetPoint;
        
        // สร้างกระสุน
        Rigidbody2D firedBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        
        // เพิ่มคอมโพเนนต์ความเสียหาย
        BulletDamage bulletDamage = firedBullet.gameObject.AddComponent<BulletDamage>();
        bulletDamage.damage = damageAmount;
        bulletDamage.lifetime = bulletLifetime; // ส่งค่าระยะเวลาที่กำหนดไปยังกระสุน
        
        // คำนวณความเร็วกระสุน
        Vector2 projectileVelocity = CalculateProjectileVelocity(shootPoint.position, targetPoint, 1f);
        firedBullet.velocity = projectileVelocity;
    }

    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
    {
        Vector2 distance = target - origin;
        float velocityX = distance.x / time;
        float velocityY = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;
        return new Vector2(velocityX, velocityY);
    }
}

public class BulletDamage : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 2f; // ระยะเวลาก่อนทำลายตัวเอง
    private float birthTime; // เวลาที่กระสุนถูกสร้าง
    
    private void Start()
    {
        birthTime = Time.time; // บันทึกเวลาที่กระสุนเกิด
        Destroy(gameObject, lifetime); // ทำลายตัวเองเมื่อเวลาผ่านไปตามที่กำหนด
    }
    
    private void Update()
    {
        // ตรวจสอบว่ากระสุนมีอายุเกินกำหนดหรือไม่
        if (Time.time - birthTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else
        {
            // ทำลายกระสุนเมื่อชนกับอะไรก็ตามที่ไม่ใช่ศัตรู (เช่น ผนัง)
            Destroy(gameObject);
        }
    }
}