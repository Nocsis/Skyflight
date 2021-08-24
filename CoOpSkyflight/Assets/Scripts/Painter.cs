using UnityEngine;

public class Painter : MonoBehaviour
{
    [Tooltip("Recommendateion: Set to the end of painter and set raycastLength to length of painter")]
    [SerializeField]
    private Transform painterOrigin;

    [SerializeField]
    private float raycastLength = 0.01f;

    [SerializeField]
    private float spacing = 1f;

    [SerializeField]
    private Color color;

    [SerializeField]
    private Paintable paintable;

    private Collider paintableCollider;

    public StampDatabase stampDatabase;

    private Vector2? lastDrawPosition = null;

    private void Awake()
    {
        if (paintable != null)
            Initialize(paintable);
        else
            Debug.LogError("[Painter]" + gameObject.name + " needs to have an assigned Paintable");
    }

    public void Initialize(Paintable newPaintable)
    {
        paintable = newPaintable;
        paintableCollider = newPaintable.GetComponent<Collider>();
    }

    private void Update()
    {
        Ray ray = new Ray(painterOrigin.position, painterOrigin.forward);
        RaycastHit hit;
        Debug.DrawRay(painterOrigin.position, painterOrigin.forward * raycastLength);

        if (paintableCollider.Raycast(ray, out hit, raycastLength))
        {
            //Debug.Log("Hit!");
            if (lastDrawPosition.HasValue && lastDrawPosition.Value != hit.textureCoord)
            {
                //Debug.Log("Drawline");
                paintable.DrawLine(0, lastDrawPosition.Value, hit.textureCoord, color, spacing);
            }
            else
            {
                //Debug.Log("Splash");
                //Debug.Log("hit.texturecoord: " + hit.textureCoord);
                paintable.CreateSplash(hit.textureCoord, 0, color);
            }

            lastDrawPosition = hit.textureCoord;
        }
        else
        {
            lastDrawPosition = null;
        }
    }

    public void ChangeColor(Color newColor)
    {
        color = newColor;
    }

    //public void ChangePaintMode(PaintMode newPaintMode)
    //{
    //    paintMode = newPaintMode;
    //    stamp.mode = paintMode;
    //}

    //public void ChangeStamp(Texture2D newBrush)
    //{
    //    stamp = new Stamp(newBrush);
    //    stamp.mode = paintMode;
    //}
}
