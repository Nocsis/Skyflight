using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(NetworkTransmitter))]
public class Paintable : NetworkBehaviour
{
    NetworkTransmitter networkTransmitter;

    [Tooltip("Needs a connected Audio Source")]
    [SerializeField]
    private bool playNotificationSound;

    [SerializeField]
    private float minNotificationDelay = 30f;

    [SerializeField]
    private AudioSource notificationAudioSource;

    private float lastNotificationTime;

    [SerializeField]
    private List<Paintable> connectedPaintables;

    [SerializeField]
    private Texture2D initialTexture;

    private Texture2D texture;
    private Texture2D newTexture;
    private Color32[] originalTexture;
    private Color32[] currentTexture;

    private int textureWidth;
    private int textureHeight;

    private bool wasModified = false;

    public StampDatabase stampDatabase;

    private void Awake()
    {
        networkTransmitter = GetComponent<NetworkTransmitter>();

        if (initialTexture == null)
            initialTexture = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;

        texture = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;

        textureWidth = texture.width;
        textureHeight = texture.height;

        originalTexture = texture.GetPixels32();

        newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false, true);
        newTexture.SetPixels32(initialTexture.GetPixels32());
        newTexture.Apply();

        currentTexture = new Color32[textureWidth * textureHeight];
        newTexture.GetPixels32().CopyTo(currentTexture, 0);

        GetComponent<MeshRenderer>().material.mainTexture = newTexture;

        lastNotificationTime = Time.realtimeSinceStartup;
        if (playNotificationSound && notificationAudioSource == null)
            Debug.LogError("[Paintable] Trying to play notification sounds. Either connect an audio source or disable them.");
    }

    private void Start()
    {
        if (!isServer)
        {
            networkTransmitter.OnDataCompletelyReceived += DataCompletelyReceivedHandler;
            CmdRequestTexture();
        }
    }

    [Client]
    private void DataCompletelyReceivedHandler(int transmissionId, byte[] data)
    {
        newTexture.LoadImage(data);
        newTexture.GetPixels32().CopyTo(currentTexture, 0);
    }

    [Command(requiresAuthority = false)]
    private void CmdRequestTexture()
    {
        byte[] sendTexture = newTexture.EncodeToJPG();

        StartCoroutine(networkTransmitter.SendBytesToClientsRoutine((int)Time.time + gameObject.GetInstanceID(), sendTexture));
    }

    private void LateUpdate()
    {
        if (wasModified)
        {
            newTexture.SetPixels32(currentTexture);
            newTexture.Apply();

            foreach(Paintable paintable in connectedPaintables)
            {
                if (paintable.playNotificationSound && Time.realtimeSinceStartup > (paintable.lastNotificationTime + paintable.minNotificationDelay))
                {
                    paintable.notificationAudioSource.PlayOneShot(notificationAudioSource.clip);
                    paintable.lastNotificationTime = Time.realtimeSinceStartup;
                }
            }

            wasModified = false;
        }
    }

    /// Paints one stamp
    public void CreateSplash(Vector2 uvPosition, int stampIndex, Color color)
    {
        CmdPaintOver(stampIndex, (Color32)color, uvPosition);
    }

    /// Paints a line that consist of stamps
    public void DrawLine(int stampIndex, Vector2 startUVPosition, Vector2 endUVPosition, Color color, float spacing)
    {
        Stamp stamp = stampDatabase.stamps[stampIndex].stamp;

        Vector2 uvDistance = endUVPosition - startUVPosition;

        Vector2 pixelDistance = new Vector2(Mathf.Abs(uvDistance.x) * textureWidth, Mathf.Abs(uvDistance.y) * textureHeight);
        float stampDistance = stamp.Width > stamp.Height ? stamp.Height : stamp.Width;

        int stampsNo = Mathf.FloorToInt((pixelDistance.magnitude / stampDistance) / spacing) + 1;

        for (int i = 0; i <= stampsNo; i++)
        {
            float lerp = i / (float)stampsNo;

            Vector2 uvPosition = Vector2.Lerp(startUVPosition, endUVPosition, lerp);

            CmdPaintOver(stampIndex, color, uvPosition);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdPaintOver(int stampIndex, Color32 color, Vector2 uvPosition)
    {
        RpcPaintOver(stampIndex, color, uvPosition);
        foreach (Paintable paintable in connectedPaintables)
            paintable.RpcPaintOver(stampIndex, color, uvPosition);
    }

    [ClientRpc]
    public void RpcPaintOver(int stampIndex, Color32 color, Vector2 uvPosition)
    {
        Stamp stamp = stampDatabase.stamps[stampIndex].stamp;

        //Debug.Log("Paint at" + uvPosition + " with stamp w/h" + stamp.Width + "/" + stamp.Height);
        int paintStartPositionX = (int)((uvPosition.x * textureWidth) - stamp.Width / 2f);
        int paintStartPositionY = (int)((uvPosition.y * textureHeight) - stamp.Height / 2f);

        // Checking manually if int is bigger than 0 is faster than using Mathf.Clamp
        int paintStartPositionXClamped = paintStartPositionX;
        if (paintStartPositionXClamped < 0)
            paintStartPositionXClamped = 0;
        int paintStartPositionYClamped = paintStartPositionY;
        if (paintStartPositionYClamped < 0)
            paintStartPositionYClamped = 0;

        // Check manually if end position doesn't exceed texture size
        int paintEndPositionXClamped = paintStartPositionX + stamp.Width;
        if (paintEndPositionXClamped >= textureWidth)
            paintEndPositionXClamped = textureWidth - 1;
        int paintEndPositionYClamped = paintStartPositionY + stamp.Height;
        if (paintEndPositionYClamped >= textureHeight)
            paintEndPositionYClamped = textureHeight - 1;

        int totalWidth = paintEndPositionXClamped - paintStartPositionXClamped;
        int totalHeight = paintEndPositionYClamped - paintStartPositionYClamped;

        Color32 newColor = Color.white;
        Color32 textureColor;
        float alpha;
        int aChannel;

        for (int x = 0; x < totalWidth; x++)
        {
            for (int y = 0; y < totalHeight; y++)
            {
                int stampX = paintStartPositionXClamped - paintStartPositionX + x;
                int stampY = paintStartPositionYClamped - paintStartPositionY + y;

                alpha = stamp.CurrentPixels[stampX + stampY * stamp.Width];

                // There is no need to do further calculations if this stamp pixel is transparent
                if (alpha < 0.001f)
                    continue;

                int texturePosition = paintStartPositionXClamped + x + (paintStartPositionYClamped + y) * textureWidth;

                if (stamp.mode == PaintMode.Erase)
                    color = originalTexture[texturePosition];

                aChannel = (int)(alpha * 255f);

                textureColor = currentTexture[texturePosition];

                newColor.r = (byte)(color.r * aChannel / 255 + textureColor.r * textureColor.a * (255 - aChannel) / (255 * 255));
                newColor.g = (byte)(color.g * aChannel / 255 + textureColor.g * textureColor.a * (255 - aChannel) / (255 * 255));
                newColor.b = (byte)(color.b * aChannel / 255 + textureColor.b * textureColor.a * (255 - aChannel) / (255 * 255));
                newColor.a = (byte)(aChannel + textureColor.a * (255 - aChannel) / 255);

                currentTexture[texturePosition] = newColor;
            }
        }

        wasModified = true;
    }
}
