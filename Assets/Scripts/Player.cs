using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private Vector3 playerStartPosition;

    float ejeHorizontal;
    float ejeVertical;

    public float jumpForce = 6f;
    public LayerMask groundMask;

    bool actionRun = false;
    bool gunLoaded = true;
    bool actionJump = true;

    Vector3 moveDirection;
    Vector2 facingDirection;

    [SerializeField] float fireRate = 7;
    [SerializeField] float moveSpeed;
    [SerializeField] int health = 3;
    [SerializeField] Transform aim;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform BulletPrefab;

    private Rigidbody2D playeRigidBody;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        playeRigidBody = GetComponent<Rigidbody2D>();
        playerStartPosition = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.sharedInstanceGameManager.currentGameState == GameState.inGame)
        {
        ActionRecolector();
        Debug.DrawRay(this.transform.position, Vector2.down * 1.27f, Color.red);
        Jump();
        }
        else if (GameManager.sharedInstanceGameManager.currentGameState == GameState.gameOver)
        {

            playeRigidBody.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {

        if (GameManager.sharedInstanceGameManager.currentGameState == GameState.inGame)
        {
            ToMove();
            Aim();
            ToShoot();
            //Jump();
        }
        else if (GameManager.sharedInstanceGameManager.currentGameState == GameState.gameOver)
        {
            playeRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playeRigidBody.velocity = Vector2.zero;
        }

    }

    public void StartGame()
    {
        if (GameManager.sharedInstanceGameManager.currentGameState == GameState.gameOver)
        {
            Invoke("RestartPosition", 0.2f);

        }
    }
    private void RestartPosition()
    {
        this.transform.position = playerStartPosition;
        //this.playeRigidBody.velocity = Vector2.zero;
    }

    void ActionRecolector()
    {
        //--------------------------------
        //walking.
        ejeHorizontal = Input.GetAxis("Horizontal");
        //ejeVertical = Input.GetAxis("Vertical");
        actionJump = (Input.GetButtonDown("Jump"));
        moveDirection.x = ejeHorizontal;
        moveDirection.y = 0;
        //--------------------------------
        //Running
        actionRun = Input.GetButton("Fire3");
        //--------------------------------
    }

    void ToMove()
    {
        if (actionRun)
        {
            transform.position += moveDirection * Time.deltaTime * moveSpeed * 2;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
    }

    void Jump()
    {
        if (IsTouchingTheGround() && actionJump)
        {
            playeRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool IsTouchingTheGround()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.30f, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

void ToShoot()
    {
        //  Si click izq 
        if (Input.GetMouseButton(0) && gunLoaded)
        {
            gunLoaded = false;
            float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
            Quaternion BulletDirection = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(BulletPrefab, transform.position, BulletDirection);
            StartCoroutine(ReloadGun());
        }
    }

    void Aim()
    {
        
        facingDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //En este algoritmo se coje el vector2 y se le dice a unity que lo interprete como un vector3 y se normaliza (magnitud = 1)
        aim.position = transform.position + (Vector3)facingDirection.normalized;
    }

    

            //********************************
            //Corutinas
            IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(1 / fireRate);
        gunLoaded = true;
    }
}
