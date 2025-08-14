using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GravitySimulation : MonoBehaviour
{
    [Header("Falling Objects")]
    public Rigidbody FallingObject1;
    public Rigidbody FallingObject2;

    [Header("UI Sliders")]
    public Slider MassSlider1;
    public Slider HeightSlider1;
    public Slider MassSlider2;
    public Slider HeightSlider2;

    [Header("UI Texts")]
    public TextMeshProUGUI MassText1;
    public TextMeshProUGUI HeightText1;
    public TextMeshProUGUI FallingTime;
    public TextMeshProUGUI MassText2;
    public TextMeshProUGUI HeightText2;
    public TextMeshProUGUI FallingTime2;

    [Header("Buttons")]
    public Button StartButton;
    public Button ResetButton;

    [Header("Audio")]
    public AudioSource BackgroundMusic;
    public AudioSource AudioNarration;

    private Vector3 initialPos1, initialPos2;
    private Quaternion initialRot1, initialRot2;
    private float initialMass1, initialMass2;

    private float timer1 = 0f;
    private float timer2 = 0f;
    private bool isFalling1 = false;
    private bool isFalling2 = false;

    void Start()
    {
        // Save initial state
        initialPos1 = FallingObject1.transform.position;
        initialRot1 = FallingObject1.transform.rotation;
        initialMass1 = FallingObject1.mass;

        initialPos2 = FallingObject2.transform.position;
        initialRot2 = FallingObject2.transform.rotation;
        initialMass2 = FallingObject2.mass;

        FallingObject1.isKinematic = true;
        FallingObject2.isKinematic = true;

        // Play audio narration once at the start
        if (AudioNarration != null && !AudioNarration.isPlaying)
            AudioNarration.Play();

        // Setup sliders
        MassSlider1.onValueChanged.AddListener(UpdateMass1);
        HeightSlider1.onValueChanged.AddListener(UpdateHeight1);
        MassSlider2.onValueChanged.AddListener(UpdateMass2);
        HeightSlider2.onValueChanged.AddListener(UpdateHeight2);

        StartButton.onClick.AddListener(StartSimulation);
        ResetButton.onClick.AddListener(ResetSimulation);

        // Initial values
        UpdateMass1(MassSlider1.value);
        UpdateHeight1(HeightSlider1.value);
        UpdateMass2(MassSlider2.value);
        UpdateHeight2(HeightSlider2.value);
    }

    void UpdateMass1(float value)
    {
        FallingObject1.mass = value;
        MassText1.text = "Mass: " + value.ToString("F2") + " kg";
    }

    void UpdateHeight1(float value)
    {
        Vector3 pos = FallingObject1.transform.position;
        pos.y = value;
        FallingObject1.transform.position = pos;
        HeightText1.text = "Height: " + value.ToString("F1") + " m";
    }

    void UpdateMass2(float value)
    {
        FallingObject2.mass = value;
        MassText2.text = "Mass: " + value.ToString("F2") + " kg";
    }

    void UpdateHeight2(float value)
    {
        Vector3 pos = FallingObject2.transform.position;
        pos.y = value;
        FallingObject2.transform.position = pos;
        HeightText2.text = "Height: " + value.ToString("F1") + " m";
    }

    void StartSimulation()
    {
        // Start background music if not playing
        if (BackgroundMusic != null && !BackgroundMusic.isPlaying)
            BackgroundMusic.Play();

        // Allow both objects to fall
        FallingObject1.isKinematic = false;
        FallingObject2.isKinematic = false;

        // Reset timers
        timer1 = 0f;
        timer2 = 0f;
        isFalling1 = true;
        isFalling2 = true;
    }

    void ResetSimulation()
    {
        // Stop object motion
        FallingObject1.isKinematic = true;
        FallingObject2.isKinematic = true;

        //FallingObject1.linearVelocity = Vector3.zero;
        //FallingObject1.angularVelocity = Vector3.zero;
        FallingObject1.transform.position = initialPos1;
        FallingObject1.transform.rotation = initialRot1;
        FallingObject1.mass = initialMass1;

        //FallingObject2.linearVelocity = Vector3.zero;
        //FallingObject2.angularVelocity = Vector3.zero;
        FallingObject2.transform.position = initialPos2;
        FallingObject2.transform.rotation = initialRot2;
        FallingObject2.mass = initialMass2;

        // Reset timers
        timer1 = 0f;
        timer2 = 0f;
        FallingTime.text = "Time: 0.00s";
        FallingTime2.text = "Time: 0.00s";

        // Maintain background music
        if (BackgroundMusic != null && !BackgroundMusic.isPlaying)
            BackgroundMusic.Play();
    }

    void Update()
    {
        if (isFalling1)
        {
            timer1 += Time.deltaTime;
            FallingTime.text = "Time: " + timer1.ToString("F2") + "s";

            if (FallingObject1.transform.position.y <= 0.5f)
                isFalling1 = false;
        }

        if (isFalling2)
        {
            timer2 += Time.deltaTime;
            FallingTime2.text = "Time: " + timer2.ToString("F2") + "s";

            if (FallingObject2.transform.position.y <= 0.5f)
                isFalling2 = false;
        }
    }
}
