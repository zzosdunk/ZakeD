using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    public GameObject player;
    private GameObject playerArea;
    private Rigidbody2D playerRB;

    private bool ballIsCLicked = false;
    private bool ballIsKeepClicked = false;

    private Vector3 ballPos;

    private Vector3 cursorPos;
    private Vector3 ballCursorDistance;

    public float shootPowerX;
    public float shootPowerY;
    private Vector2 shotForce;

    private float pushTime = 5f;

    public Sprite pointSprite;
    private GameObject trajectory;
    public GameObject[] points;
    public int numberOfPoints;
    public float initialPointSize;
    public float pointSeparation;
    public float pointShift;
    private float x1, y1;

    private float levelTime = 0.5f;
    public Text pushTimeText;
    
    void Start()
    {
        player = gameObject;

        playerArea = GameObject.Find("ClickArea");
        playerRB = GetComponent<Rigidbody2D>();
        trajectory = GameObject.Find("Trajectory");

        trajectory.transform.localScale = new Vector3(initialPointSize, initialPointSize, trajectory.transform.localScale.z);

        for (int k = 0; k < 40; k++)
        {
            points[k] = GameObject.Find("Dot (" + k + ")");
            if (pointSprite != null)
            {
                points[k].GetComponent<SpriteRenderer>().sprite = pointSprite;
            }
        }
        for (int k = numberOfPoints; k < 40; k++)
        {
            GameObject.Find("Dot (" + k + ")").SetActive(false);
        }
        trajectory.SetActive(false);
    }

    void Update()
    {

        ballPos = player.transform.position; //current player position

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && ballIsKeepClicked == false)
        {
            if (hit.collider.gameObject.name == playerArea.gameObject.name)
            {
                ballIsCLicked = true;

            }
            else
            {
                ballIsCLicked = false;
            }
        }
        else
        {
            ballIsCLicked = false;
        }
        if (ballIsKeepClicked == true)
        {
            ballIsCLicked = true;
        }

        if ((Input.GetKey(KeyCode.Mouse0) && ballIsCLicked == true) && (playerRB.velocity.x == 0f && playerRB.velocity.y == 0f)) ////If player has activated a shot
        {
            PushTime();

            ballIsKeepClicked = true;

            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //definding the current cursor position
            cursorPos.z = 0;
            ballCursorDistance = ballPos - cursorPos; //The distance between the cursor and the "ball" is found

            shotForce = new Vector2(ballCursorDistance.x * shootPowerX, ballCursorDistance.y * shootPowerY); //velocity of shot

            if (Mathf.Sqrt(ballCursorDistance.x * ballCursorDistance.x) + (ballCursorDistance.y * ballCursorDistance.y) >= (0.4f))
            {
                trajectory.SetActive(true);
            }
            else
            {
                trajectory.SetActive(false);
            }
            for (int k = 0; k < numberOfPoints; k++)
            {
                x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (pointSeparation * k + pointShift);
                y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (pointSeparation * k + pointShift) - (-Physics2D.gravity.y / 2f * Time.fixedDeltaTime * Time.fixedDeltaTime * (pointSeparation * k + pointShift) * (pointSeparation * k + pointShift));
                points[k].transform.position = new Vector3(x1, y1, points[k].transform.position.z);
            }

        }     
            if (Input.GetKeyUp(KeyCode.Mouse0) && ballIsCLicked == true && pushTime > 0)
            {
            KeyUp();
            }     
    }

    public void PushTime() //method which calculates "time to aim"
    {
        pushTime -= ((GameManager.score + 1) * levelTime) * Time.deltaTime;
        if (pushTime <= 0)
        {
            playerRB.velocity = new Vector2(shotForce.x, shotForce.y);

        }
        pushTime = Mathf.Clamp(pushTime, 0f, Mathf.Infinity);
        pushTimeText.text = string.Format("{0:00.00}", pushTime);
    }

    public void KeyUp()
    {
        playerArea.SetActive(false);
        ballIsKeepClicked = false;
        pushTime = 2f;
        trajectory.SetActive(false);
        playerRB.velocity = new Vector2(shotForce.x, shotForce.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GameManager.Instance.EndGame();
        }
    }
}
   
