using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System;

public class DialogManager : MonoBehaviour
{
    

    public Queue<string> sentences;
    public Queue<string> characters;
    public Queue<int> animSequences;
    public AudioManager audioManager;
    
    public DialogTrigger[] dialogTrigger;
    public List<int> dialogChoices;
    public int curDialog = -1;
    public Animator transition;
    string sentence = "";
    bool finished = false;
    bool nextDisabled = true;
    public int nextAnim = 0;

    // scene
    public SpriteRenderer background;
    public GameObject pChar;
    public GameObject mChar;
    public GameObject ypChar;
    public GameObject fChar;
    public GameObject docChar;
    public GameObject gChar;
    public Light2D lampLight;
    public Light2D windowLight;
    public Light2D globalLight;
    public Light2D roomLight;
    public Light2D computerLight;
    public ParticleSystem rainParticles;
    public ParticleSystem rainParticles2;
    public GameObject mainCamera;
    public GameObject parisScene;
    public Animator endingTransition;

    // UI
    public TextMeshProUGUI text;
    public TextMeshProUGUI charNameText;
    public GameObject choice1;
    public GameObject choice2;
    public GameObject buttonIcon;
    public Image profileImage;
    public GameObject endGameCanvas;
    public GameObject dialogBox;
    public GameObject startButton;

    // char anim
    public float speed;
    bool moving;
    bool cameraPan = false;
    Vector3 destination;
    GameObject target;
    bool scar = true;

    public float timeScale;

    void Start()
    {
        
        sentences = new Queue<string>();
        characters = new Queue<string>();
        animSequences = new Queue<int>();
    }

    private void Update()
    {
        Time.timeScale = timeScale;
        if (!nextDisabled && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
            DisplayNextSentence();
        }
        if (moving) {
            target.transform.position = Vector3.MoveTowards(target.transform.position, destination, speed * Time.deltaTime);
        }
        if (cameraPan)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new Vector3(-21f, 0, -10), 4f * Time.deltaTime);
        }
    }

    public void StartDialog(Dialog[] dialogs) { 
        sentences.Clear();
        foreach (Dialog dialog in dialogs) { 
            sentences.Enqueue(dialog.sentence);
            characters.Enqueue(dialog.name);
            animSequences.Enqueue(dialog.animSquence);
        }

        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
        }
        else {
            StopAllCoroutines();
            if (finished)
            {
                
                PlayAnimation(animSequences.Dequeue());
                sentence = sentences.Dequeue();
                charNameText.text = characters.Dequeue();
                profileImage.gameObject.SetActive(true);
                switch (charNameText.text) {
                    case "Peter":
                        if (scar)
                        {
                            profileImage.sprite = Resources.Load<Sprite>("peter-scarredt");
                        }
                        else {
                            profileImage.sprite = Resources.Load<Sprite>("peter");
                        }
                        break;
                    case "Grace":
                        profileImage.sprite = Resources.Load<Sprite>("grace");
                        break;
                    case "Dad":
                        profileImage.sprite = Resources.Load<Sprite>("dad");
                        break;
                    case "Messenger":
                        profileImage.sprite = Resources.Load<Sprite>("messenger");
                        break;
                    case "Young Peter":
                        profileImage.sprite = Resources.Load<Sprite>("young-peter");
                        break;
                    default:
                        profileImage.gameObject.SetActive(false);
                        break;
                }
                if (sentence.Contains("||"))
                {
                    string[] choices = sentence.Split("||");
                    choice1.GetComponent<TextMeshProUGUI>().text = choices[0];
                    choice2.GetComponent<TextMeshProUGUI>().text = choices[1];
                    Button choice1Btn = choice1.GetComponent<Button>();
                    Button choice2Btn = choice2.GetComponent<Button>();
                    choice1.SetActive(true);
                    choice2.SetActive(true);
                    text.gameObject.SetActive(false);
                    choice1Btn.onClick.RemoveAllListeners();
                    choice1Btn.onClick.AddListener(() => ChooseOption(1));
                    choice2Btn.onClick.RemoveAllListeners();
                    choice2Btn.onClick.AddListener(() => ChooseOption(2));
                    nextDisabled = true;
                    buttonIcon.SetActive(false);
                }
                else
                {
                    StartCoroutine(TypeSentence(sentence));
                }
                
            } else {
                text.text = sentence;
                finished = true;
            }
            
            
        }
    }

    IEnumerator TypeSentence(string sentence) {
        text.text = "";
        finished = false;
        foreach (char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        finished = true;
    }

    void EndDialog() {
        bool transition = true;
        if (curDialog >=0 )
        {
            transition = dialogTrigger[curDialog].transition;
        }
        switch (curDialog) {
            case 6:
                if (dialogChoices[2] == 1) {
                    curDialog += 1;
                } else if (dialogChoices[2] == 2) {
                    curDialog += 2;
                }
                break;
            case 7:
                curDialog += 2;
                break;
            case 11:
                if (dialogChoices[0] == 1)
                {
                    curDialog += 1;
                }
                else if (dialogChoices[0] == 2)
                {
                    curDialog += 2;
                }
                break;
            case 12:
                curDialog += 2;
                break;
            case 16:
                if (dialogChoices[1] == 1)
                {
                    curDialog += 1;
                }
                else if (dialogChoices[1] == 2)
                {
                    curDialog += 2;
                }
                break;
            case 17:
                curDialog += 2;
                break;
            case 21:
                if (dialogChoices[2] == 1)
                {
                    curDialog += 1;
                }
                else if (dialogChoices[2] == 2)
                {
                    curDialog += 2;
                }
                break;
            case 22:
                curDialog += 2;
                break;
            case 23:
                curDialog += 2;
                break;
            case 24:
                curDialog += 2;
                break;
            case 26:
                if (dialogChoices[3] == 1)
                {
                    curDialog += 1;
                }
                else if (dialogChoices[3] == 2)
                {
                    curDialog += 2;
                }
                break;
            case 27:
                curDialog += 2;
                break;
            case 29:
                if(dialogChoices[3] == 1)
                {
                    curDialog += 1;
                }
                else if (dialogChoices[3] == 2)
                {
                    curDialog += 2;
                }
                break;
            case 30:
                curDialog += 2;
                break;
            case 36:
                EndGame();
                break;
            default:
                curDialog += 1;
                break;
        }
        if (transition)
        {
            StartCoroutine(Transition());
        }
        else {
            dialogTrigger[curDialog].TriggerDialog();
        }
        
    }

    void EndGame() {
        endingTransition.Play("Transition");
        endGameCanvas.SetActive(true);
        nextDisabled = true;
        dialogBox.SetActive(false);
    }

    public void StartGame() { 
        curDialog = 0;
        nextDisabled = false;
        dialogTrigger[curDialog].TriggerDialog();
        dialogBox.SetActive(true);
        startButton.SetActive(false);
        SetScene();
    }

    IEnumerator Transition() {
        nextDisabled = true;
        transition.Play("Transition");
        yield return new WaitForSeconds(3f);
        SetScene();
        if (curDialog != 10 && curDialog != 33) {
            transition.Play("Transition-out");
        }
        dialogTrigger[curDialog].TriggerDialog();
        nextDisabled = false;
    }

    void SetScene() { 
        switch (curDialog)
        {
            case 0:
                background.sprite = Resources.Load<Sprite>("bedroom-night");
                pChar.transform.localScale = new Vector3(4, 4, 1);
                pChar.transform.position = new Vector3(-0.3f, 0.5f, 0);
                break;
            case 1:
                audioManager.Stop("2");
                audioManager.Play("1");
                background.sprite = Resources.Load<Sprite>("bedroom");
                globalLight.intensity = 1;
                lampLight.intensity = 0;
                mChar.SetActive(true);
                pChar.transform.localScale = new Vector3(-4, 4, 1);
                mChar.transform.localScale = new Vector3(4, 4, 1);
                pChar.transform.position = new Vector3(-3.6f, 0.5f, 0);
                mChar.transform.position = new Vector3(3.8f, 0.5f, 0);
                break;
            case 2:
                background.sprite = Resources.Load<Sprite>("bedroom-packing");
                mChar.GetComponent<SpriteRenderer>().color = new Color(64f/255f, 187f/255f, 255f/255f);
                pChar.GetComponent<SpriteRenderer>().color = new Color(64f/255f, 187/255f, 255/255f);
                mChar.transform.position = new Vector3(8, 0.5f, 0);
                pChar.transform.position = new Vector3(5, 0.5f, 0);
                pChar.transform.localScale = new Vector3(4, 4, 1);
                ypChar.SetActive(true);
                fChar.SetActive(true);
                ypChar.transform.position = new Vector3(-5f, 0.5f, 0);
                fChar.transform.position = new Vector3(13f, 0.5f, 0);
                break;
            case 3:
                background.sprite = Resources.Load<Sprite>("bedroom-cloud");
                scar = false;
                fChar.transform.localScale = new Vector3(-4, 4, 1);
                ypChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.transform.position = new Vector3(-3.5f, 0.5f, 1);
                ypChar.transform.position = new Vector3(0, 0.5f, 1);
                pChar.transform.localScale = new Vector3(-4, 4, 1);
                break;
            case 4:
                background.sprite = Resources.Load<Sprite>("bedroom");
                fChar.transform.position = new Vector3(13f, 0.5f, 0);
                pChar.transform.localScale = new Vector3(4, 4, 1);
                break;
            case 5:
                background.sprite = Resources.Load<Sprite>("bedroom-nightcomputer");
                fChar.SetActive(false);
                pChar.transform.position = new Vector3(-7f, 0.5f, 0);
                pChar.transform.localScale = new Vector3(-4, 4, 1);
                mChar.transform.position = new Vector3(-2.4f, 0.5f, 0);
                mChar.transform.localScale = new Vector3(-4, 4, 1);
                ypChar.transform.localScale = new Vector3(-4, 4, 1);
                ypChar.transform.position = new Vector3(6.8f, 0.5f, 0);
                lampLight.intensity = 0.5f;
                globalLight.intensity = 0.1f;
                roomLight.gameObject.SetActive(true);
                computerLight.gameObject.SetActive(true);
                break;
            case 6:
                audioManager.Stop("1");
                audioManager.Play("2");
                background.sprite = Resources.Load<Sprite>("bedroom-night");
                roomLight.gameObject.SetActive(false);
                windowLight.gameObject.SetActive(true);
                computerLight.gameObject.SetActive(false);
                rainParticles.gameObject.SetActive(true);
                rainParticles.Play();
                ypChar.GetComponent<Animator>().Play("YPCharPhone");
                ypChar.transform.position = new Vector3(-4.5f, 0.5f, 0);
                mChar.transform.position = new Vector3(8, 0.5f, 0);
                pChar.transform.position = new Vector3(5, 0.5f, 0);
                pChar.transform.localScale = new Vector3(4, 4, 1);
                mChar.transform.localScale = new Vector3(4, 4, 1);
                pChar.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 10:
                audioManager.Stop("2");
                rainParticles.gameObject.SetActive(false);
                rainParticles.Stop();
                mChar.SetActive(false);
                ypChar.SetActive(false);
                break;
            case 11:
                audioManager.Play("3");
                mChar.GetComponent<SpriteRenderer>().color = Color.white;
                background.sprite = Resources.Load<Sprite>("bedroom-packing");
                pChar.transform.position = new Vector3(5, 0.5f, 0);
                pChar.transform.localScale = new Vector3(4, 4, 1);
                ypChar.SetActive(true);
                fChar.SetActive(true);
                ypChar.transform.position = new Vector3(-5f, 0.5f, 0);
                fChar.transform.position = new Vector3(13f, 0.5f, 0);
                mChar.SetActive(true );
                mChar.transform.position = new Vector3(13f, 0.5f, 0);
                globalLight.intensity = 1;
                lampLight.intensity = 0;
                pChar.GetComponent<SpriteRenderer>().sortingOrder = 0;
                break;
            case 15:
                background.sprite = Resources.Load<Sprite>("bedroom-cloud");
                mChar.transform.position = new Vector3(1.5f, 0.5f, 1);
                mChar.transform.localScale = new Vector3(4, 4, 1);
                pChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.GetComponent<Animator>().Play("FCharIdle");
                fChar.transform.position = new Vector3(-3.5f, 0.5f, 1);
                break;
            case 16:
                fChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.GetComponent<Animator>().Play("FCharPhone");
                mChar.SetActive(false );
                break;
            case 21:
                background.sprite = Resources.Load<Sprite>("bedroom-cloud");
                mChar.SetActive(true);
                mChar.transform.position = new Vector3(1.5f, 0.5f, 1);
                mChar.transform.localScale = new Vector3(4, 4, 1);
                pChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.transform.position = new Vector3(-3.5f, 0.5f, 1);
                break;
            case 24:
                fChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.GetComponent<Animator>().Play("FCharPhone");
                mChar.SetActive(false );
                break;
            case 25:
                fChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.GetComponent<Animator>().Play("FCharPhone");
                mChar.SetActive(false);
                break;
            case 26:
                background.sprite = Resources.Load<Sprite>("outdoors");
                windowLight.gameObject.SetActive(false);
                mChar.SetActive(true);
                fChar.GetComponent<Animator>().Play("FCharIdle");
                mChar.transform.position = new Vector3(13f, 0.5f, 1);
                mChar.transform.localScale = new Vector3(4, 4, 1);
                pChar.transform.position = new Vector3(9.5f, 0.5f, 1);
                pChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.transform.position = new Vector3(-4f, 0.5f, 1);
                fChar.transform.localScale = new Vector3(-4, 4, 1);
                break;
            case 30:
                fChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.GetComponent<Animator>().Play("FCharPhone");
                mChar.SetActive(false);
                break;
            case 31:
                fChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.GetComponent<Animator>().Play("FCharPhone");
                mChar.SetActive(false);
                break;
            case 32:
                audioManager.Stop("3");
                audioManager.Play("2");
                background.sprite = Resources.Load<Sprite>("hospital");
                globalLight.intensity = 0.1f;
                mChar.SetActive(true);
                rainParticles2.gameObject.SetActive(true);
                rainParticles2.Play();
                roomLight.gameObject.SetActive(true);
                fChar.transform.position = new Vector3(-8f, 1.2f, 0);
                fChar.transform.Rotate(new Vector3(0, 0, 90));
                fChar.transform.localScale = new Vector3(-4, 4, 1);
                docChar.SetActive(true);
                docChar.transform.position = new Vector3(-1.2f, 0.5f, 0);
                pChar.transform.position = new Vector3(8, 0.5f, 0);
                pChar.transform.localScale = new Vector3(4, 4, 1);
                pChar.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 33:
                audioManager.Stop("2");
                break;
            case 34:
                rainParticles2.gameObject.SetActive(false);
                pChar.GetComponent<SpriteRenderer>().sortingOrder = 0;
                rainParticles2.Stop();
                mChar.SetActive(false);
                fChar.transform.Rotate(new Vector3(0, 0, -90));
                pChar.GetComponent<SpriteRenderer>().color = Color.white;
                windowLight.gameObject.SetActive(true);
                roomLight.gameObject.SetActive(false);
                globalLight.intensity = 1f;
                background.sprite = Resources.Load<Sprite>("bedroom-cloud");
                fChar.transform.localScale = new Vector3(-4, 4, 1);
                ypChar.transform.localScale = new Vector3(4, 4, 1);
                fChar.transform.position = new Vector3(-3.5f, 0.5f, 1);
                ypChar.transform.position = new Vector3(0, 0.5f, 1);
                pChar.transform.position = new Vector3(13f, 0.5f, 1);
                pChar.GetComponent<SpriteRenderer>().sortingOrder = 2;
                break;
            case 35:
                mChar.transform.position = new Vector3(-15f, 0.5f, 1);
                mChar.transform.localScale = new Vector3(-4, 4, 1);
                fChar.SetActive(false);
                mChar.SetActive(true);
                pChar.GetComponent<SpriteRenderer>().sortingOrder = 0;
                break;
            case 36:
                parisScene.SetActive(true);
                audioManager.Play("1");
                mChar.SetActive(false);
                globalLight.intensity = 0.5f;
                fChar.transform.position = new Vector3(5.2f, 0.5f, 0);
                pChar.transform.position = new Vector3(-0.8f, 0.5f, 0);
                pChar.transform.localScale = new Vector3(-4, 4, 1);
                fChar.transform.localScale = new Vector3(4, 4, 1);
                gChar.transform.localScale = new Vector3(-4, 4, 1);
                gChar.transform.position = new Vector3(-16f, 0.5f, 0);
                fChar.SetActive(true);
                gChar.SetActive(true);
                pChar.transform.position = new Vector3(-0.8f, 0.5f, 0);
                break;
            default:
                break;



        }
    }

    public void ChooseOption(int option) {
        dialogChoices.Add(option);
        nextDisabled = false;
        choice1.SetActive(false);
        choice2.SetActive(false);
        text.gameObject.SetActive(true);
        buttonIcon.SetActive(true);
        DisplayNextSentence();
    }


    void PlayAnimation(int animSequence) {
        Animator pCharAnim = pChar.GetComponent<Animator>();
        Animator fCharAnim = fChar.GetComponent<Animator>();
        Animator ypCharAnim = ypChar.GetComponent<Animator>();
        Animator mCharAnim = mChar.GetComponent<Animator>();
        Animator docCharAnim = docChar.GetComponent<Animator>();
        // stop all sound effects
        switch (animSequence)
        {
            case 1: //pChar pick up phone
                pCharAnim.Play("PCharPhone");
                break;
            case 2://pChar idle
                pCharAnim.Play("PCharIdle");
                break;
            case 3: //pChar walk into a room
                pCharAnim.Play("PCharWalk");
                pChar.transform.localScale = new Vector3(-4, 4, 1);
                pChar.transform.position = Vector3.MoveTowards(pChar.transform.position, pChar.transform.position + new Vector3(-3, 0, 0), speed * Time.deltaTime);
                break;
            case 4: //phone ring sound
                break;
            case 5: //fChar walk INTO A ROOM
                fCharAnim.Play("FCharWalk");
                fChar.transform.localScale = new Vector3(4, 4, 1);
                StartCoroutine(WaitAnim(fChar, new Vector3(-0.3f, 0.5f, 0), () => { 
                    fCharAnim.Play("FCharIdle"); 
                }));
                break;
            case 6: //knock sound

                break;
            case 7: //YPChar walk out of room
                ypCharAnim.Play("YPCharWalk");
                ypChar.transform.localScale = new Vector3(-4, 4, 1);
                StartCoroutine(WaitAnim(ypChar, new Vector3(13f, 0.5f, 0), () => {
                    ypCharAnim.Play("YPCharIdle");
                }));
                break;
            case 8://YP turn around
                ypChar.transform.localScale = new Vector3(-ypChar.transform.localScale.x, 4, 1);
                break;
            case 9: //P turns around
                pChar.transform.localScale = new Vector3(-pChar.transform.localScale.x, 4, 1);
                break;
            case 10: //fChar walks towards bed
                fCharAnim.Play("FCharWalk");
                fChar.transform.localScale = new Vector3(4, 4, 1);
                StartCoroutine(WaitAnim(fChar, new Vector3(-8f, 0.5f, 0), () => {
                    fCharAnim.Play("FCharIdle");
                }));
                break;
            case 11://mChar turn arround
                mChar.transform.localScale = new Vector3(-mChar.transform.localScale.x, 4, 1);
                break;
            case 12: //mChar walk into the room
                mCharAnim.Play("MCharWalk");
                mChar.transform.localScale = new Vector3(4, 4, 1);
                StartCoroutine(WaitAnim(mChar, new Vector3(2.5f, 0.5f, 0), () => {
                    mCharAnim.Play("MCharIdle");
                }));
                break;
            case 13: //fChar turns around
                fChar.transform.localScale = new Vector3(-fChar.transform.localScale.x, 4, 1);
                break;
            case 14: //mChar walks out of room
                mCharAnim.Play("MCharWalk");
                mChar.transform.localScale = new Vector3(-4, 4, 1);
                StartCoroutine(WaitAnim(mChar, new Vector3(13f, 0.5f, 0), () => {
                    mCharAnim.Play("MCharIdle");
                }));
                break;
            case 15: //fChar walks out of room
                fCharAnim.Play("FCharWalk");
                fChar.transform.localScale = new Vector3(-4, 4, 1);
                StartCoroutine(WaitAnim(fChar, new Vector3(13f, 0.5f, 0), () => {
                    fCharAnim.Play("FCharIdle");
                }));
                break;
            case 16: //fChar idle
                fCharAnim.Play("FCharIdle");
                break;
            case 17: //fChar walk out left
                fCharAnim.Play("FCharWalk");
                fChar.transform.localScale = new Vector3(4, 4, 1);
                StartCoroutine(WaitAnim(fChar, new Vector3(-15f, 0.5f, 0), () => {
                    fCharAnim.Play("FCharIdle");
                }));
                break;
            case 18: //doccHAR WALK OUT LEFT
                docCharAnim.Play("DocCharWalk");
                docChar.transform.localScale = new Vector3(-4, 4, 1);
                StartCoroutine(WaitAnim(docChar, new Vector3(13f, 0.5f, 0), () => {
                    docCharAnim.Play("DocCharIdle");
                }));
                break;
            case 19: //pChar walk into a room
                pCharAnim.Play("PCharWalk");
                pChar.transform.localScale = new Vector3(4, 4, 1);
                StartCoroutine(WaitAnim(pChar, new Vector3(0, 0.5f, 0), () => {
                    pCharAnim.Play("PCharIdle");
                }));
                break;
            case 20: //pChar walk halfway out
                pCharAnim.Play("PCharWalk");
                pChar.transform.localScale = new Vector3(-4, 4, 1);
                StartCoroutine(WaitAnim(pChar, new Vector3(8f, 0.5f, 0), () => {
                    pCharAnim.Play("PCharIdle");
                }));
                break;
            case 21://mChar walk in from left
                mCharAnim.Play("MCharWalk");
                mChar.transform.localScale = new Vector3(-4, 4, 1);
                StartCoroutine(WaitAnim(mChar, new Vector3(-2f, 0.5f, 0), () => {
                    mCharAnim.Play("MCharIdle");
                }));
                break;
            case 22://pChar walk to left
                pCharAnim.Play("PCharWalk");
                pChar.transform.localScale = new Vector3(4, 4, 1);
                StartCoroutine(WaitAnim(pChar, new Vector3(-26f, 0.5f, 0), () => {
                    pCharAnim.Play("PCharIdle");
                }));
                cameraPan = true;
                break;
            case 23: //GChar turns
                gChar.transform.localScale = new Vector3(-gChar.transform.localScale.x, 4, 1);
                break;
            case 24: //YPChar phone
                ypCharAnim.Play("YPCharPhone");
                break;
            case 25: //dad phone
                fCharAnim.Play("FCharPhone");
                break;
            case 26: //young peter idle
                ypCharAnim.Play("YPCharIdle");
                break;
            default:
                break;
        }
    }

    IEnumerator WaitAnim(GameObject character, Vector3 pos, Action callback) {
        nextDisabled = true;
        moving = true;
        target = character;
        destination = pos;
        yield return new WaitUntil(() => target.transform.position == pos);
        callback();
        moving = false;
        nextDisabled = false;
    }

}
