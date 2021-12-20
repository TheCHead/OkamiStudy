using System.Collections;
using DG.Tweening;
using StarterAssets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ThirdPersonController playerController;
    [SerializeField] private GameObject drawPanel;
    [SerializeField] private Brush brushPrefab;
    private Animator _brushAnimator;
    public static bool IsDrawing => _isDrawing;
    
    private static bool _isDrawing;

    private Demo gestureRecognizer;
    private GameObject brush;

    private void Awake()
    {
        Cursor.visible = false;
        gestureRecognizer = FindObjectOfType<Demo>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(ToggleDrawMode(true));
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            StartCoroutine(ToggleDrawMode(false));
        }

        if (_isDrawing && brush.gameObject.activeSelf)
        {
            brush.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 5));
            
            _brushAnimator.SetBool("isDrawing", Input.GetMouseButton(0));
            _brushAnimator.SetFloat("X", Mathf.Lerp(_brushAnimator.GetFloat("X"), Input.GetAxis("Mouse X") * 1, 0.1f));
            _brushAnimator.SetFloat("Y", Mathf.Lerp(_brushAnimator.GetFloat("Y"), Input.GetAxis("Mouse Y") * 1, 0.1f));
        }
    }

    IEnumerator ToggleDrawMode(bool isModeOn)
    {
        StarterAssetsInputs inputs = playerController.GetComponent<StarterAssetsInputs>();

        _isDrawing = isModeOn;

        if (isModeOn)
        {
            playerController.enabled = false;
            inputs.cursorInputForLook = false;
            inputs.cursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
            
            Vector2 warpPos = Screen.safeArea.center;
            Mouse.current.WarpCursorPosition(warpPos);
            
            if (brush != null)
            {
                foreach (Renderer renderer in brush.GetComponent<Brush>().Renderers)
                {
                    renderer.enabled = true;
                }
            }
            else
            {
                brush = Instantiate(brushPrefab.gameObject, Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 5)) ,
                    quaternion.identity, Camera.main.transform);
                _brushAnimator = brush.GetComponent<Brush>().Animator;
            }
            
            Time.timeScale = 0f;
            drawPanel.SetActive(true);
            drawPanel.transform.DOLocalRotate(new Vector3(25f, 0f, 0f), .5f).SetUpdate(true);
            drawPanel.GetComponent<MeshRenderer>().material.DOFloat(1f, "Alpha", .5f).SetUpdate(true);
            drawPanel.GetComponent<MeshRenderer>().material.DOFloat(0.75f, "SepiaAmount", .5f).SetUpdate(true);
        }
        else
        {
            
            Vector2 warpPos = Screen.safeArea.center;
            Mouse.current.WarpCursorPosition(warpPos);
            Time.timeScale = 1f;
            
            yield return new WaitForFixedUpdate();
            
            playerController.enabled = true;
            inputs.cursorInputForLook = true;
            inputs.cursorLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
            
            _brushAnimator.SetBool("isDrawing", false);
            _brushAnimator.SetFloat("X", 0f);
            _brushAnimator.SetFloat("Y", 0f);
            foreach (Renderer renderer in brush.GetComponent<Brush>().Renderers)
            {
                renderer.enabled = false;
            }
            
            gestureRecognizer.TryRecognize();
            drawPanel.transform.DOLocalRotate(new Vector3(0, 0f, 0f), .5f).SetUpdate(true);
            drawPanel.GetComponent<MeshRenderer>().material.DOFloat(0f, "Alpha", .5f).SetUpdate(true);
            drawPanel.GetComponent<MeshRenderer>().material.
                DOFloat(0f, "SepiaAmount", .5f).SetUpdate(true).OnComplete(() =>
                {
                    drawPanel.SetActive(false);
                });
        }
    }
}
