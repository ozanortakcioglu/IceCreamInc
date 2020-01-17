using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{

    public float speed = 5;
    public GameObject black, white, Panel;
    public RawImage currentImage, matchImage, targetImage;
    public Slider slider, match;
    public Text levelTxt;
    public Transform left, right, faucet;

    int currentLevel = 1;
    float timeCounter = 0;
    float counter = 1;
    readonly int level1White = 100;
    readonly int level2White = 50;
    int whiteCounter = 0;  

    Vector3 currentAngle = new Vector3(30, 0, 90);
    Vector3 targetAngle = new Vector3(-10, 0, 90);
    Vector3 angle;

    bool isleft = true;
    bool playable = true;
    bool pressed = false;

    GameObject parent;
    private void Start()
    {
        parent = new GameObject();
        faucet = faucet.GetComponent<Transform>();
        angle = currentAngle;
    }

    void FixedUpdate()
    {
        if (counter == 101)
        {
            playable = false;
            UpdateUI();
            counter++;
        }
        if (playable)
        {
            if (pressed)
            {
                timeCounter += Time.deltaTime * speed;
                float x = Mathf.Cos(timeCounter) * (4.0f - (counter / 50f));
                float z = Mathf.Sin(timeCounter) * (4.0f - (counter / 50f));
                transform.position = new Vector3(x, 0, z);

                if (isleft)
                {
                    if (counter <= 100)
                    {
                        currentAngle = new Vector3(Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * speed), currentAngle.y, currentAngle.z);
                        left.transform.eulerAngles = currentAngle;
                        Instantiate(black, faucet.transform.position, Quaternion.identity, parent.transform);
                        counter++;
                    }

                }
                else
                {
                    if (counter <= 100)
                    {
                        currentAngle = new Vector3(Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * speed), currentAngle.y, currentAngle.z);
                        right.transform.eulerAngles = currentAngle;
                        Instantiate(white, faucet.transform.position, Quaternion.identity, parent.transform);
                        counter++;
                        whiteCounter++;
                    }

                }
                slider.value = counter;
            }
            else
            {
                right.transform.eulerAngles = currentAngle;
                left.transform.eulerAngles = currentAngle;
            }
        } 
    }

    public void LeftButtonDown()
    {
        isleft = true;
        pressed = true;
    }
    public void LeftButtonUp()
    {
        currentAngle = angle;
        pressed = false;
    }
    public void RightButtonDown()
    {
        isleft = false;
        pressed = true;
    }
    public void RightButtonUp()
    {
        currentAngle = angle;
        pressed = false;
        
    }

    void UpdateUI()
    {
        StartCoroutine(WaitAndUpdate());
        if (currentLevel == 1)
        {
            match.value = 100 - Mathf.Abs(whiteCounter - level1White);
        }
        if (currentLevel == 2)
        {
            match.value = 100 - Mathf.Abs(whiteCounter - level2White) * 2;
            matchImage.texture = Resources.Load("Level" + currentLevel, typeof(Texture)) as Texture;
        }
    }

    public void OnClick()
    {
        whiteCounter = 0;
        currentLevel++;
        levelTxt.text = "Level  " + currentLevel;
        Panel.SetActive(false);
        playable = true;
        counter = 0;
        targetImage.texture = Resources.Load("Level2", typeof(Texture)) as Texture;
        slider.value = 0;
        Destroy(parent);
        parent = new GameObject();
    }

    IEnumerator WaitAndUpdate()
    {
        yield return new WaitForSeconds(5f);
        ScreenCapture.CaptureScreenshot("Assets/Resources/current.jpg");
        yield return null;
        UnityEditor.AssetDatabase.Refresh();
        yield return null;
        currentImage.texture = Resources.Load("current", typeof(Texture)) as Texture;
        Panel.SetActive(true);
        yield return null;
    }


}
