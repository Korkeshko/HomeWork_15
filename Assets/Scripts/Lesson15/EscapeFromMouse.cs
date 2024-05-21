using UnityEngine;

public class EscapeFromMouse : MonoBehaviour
{
    private Vector3 startMousePosition;
    private Vector3 currentMousePosition;
    private new Camera camera;
    public float speed = 0.05f;
    
    public void Awake()
    {   
        camera = Camera.main; 
    }

    public void OnMouseEnter()
    {
        Vector3 mousePosition = Input.mousePosition;   
           
        // Чтобы убедиться, что точка мирового пространства является частью объема обзора камеры, 
        // координата z должна находиться между координатами камеры nearClipPlane и farClipPlane
        mousePosition.z = camera.nearClipPlane; // Установить координату z на ближнюю распознающую камерой плоскость  
        startMousePosition = camera.ScreenToWorldPoint(mousePosition);
    }

    public void OnMouseOver()
    {     
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = camera.nearClipPlane;
        currentMousePosition = camera.ScreenToWorldPoint(mousePosition);      
        Vector3 direction = (currentMousePosition - startMousePosition).normalized;
        transform.position += direction * speed;
    }
}