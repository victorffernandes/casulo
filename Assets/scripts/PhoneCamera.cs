using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class PhoneCamera : MonoBehaviour
{

	private bool camAvailable;
	private WebCamTexture cameraTexture;
	private Texture defaultBackground;
	private bool press;

	public RawImage background;
	public AspectRatioFitter fit;

	public PhotoAction actualPhotoAction;

	private string phaseName;

	// Use this for initialization
	void Start()
	{
		phaseName = GameObject.FindObjectOfType<GameMaster>().phaseName;
		defaultBackground = background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;
		press = false;

		if (devices.Length == 0)
		{
			camAvailable = false;
			return;
		}

		for (int i = 0; i < devices.Length; i++)
		{
			if (devices[i].isFrontFacing)
			{
				cameraTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
			}
		}

		if (cameraTexture == null)
			return;

		// cameraTexture.Play(); // Start the camera		
		background.texture = cameraTexture; // Set the texture

		camAvailable = false; // Set the camAvailable for future purposes.
	}

	// Update is called once per frame
	void Update()
	{
		if (!camAvailable)
			return;

		float ratio = (float)cameraTexture.width / (float)cameraTexture.height;
		fit.aspectRatio = ratio; // Set the aspect ratio

		float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not
		background.rectTransform.localScale = new Vector3(1f, scaleY, 1f); // Swap the mirrored camera

		int orient = -cameraTexture.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
	}

	public void activateCamera(PhotoAction pc){
		this.cameraTexture.Play();
		camAvailable = true;

		actualPhotoAction = pc;
	}

	public void TakePhoto()  // Start this Coroutine on some button click
	{
		Texture2D photo = new Texture2D(cameraTexture.width, cameraTexture.height);
		photo.SetPixels(cameraTexture.GetPixels());
		photo.Apply();
		Debug.Log(photo);

		//Encode to a PNG
		byte[] bytes = photo.EncodeToPNG();

		string uuid = System.Guid.NewGuid().ToString();
		string fileName = "photo-"+phaseName+"-"+ uuid +".png";
		string subPath = Path.Combine(Application.dataPath, "selfies");
		string filePath = Path.Combine(subPath, fileName);
		
		//Write out the PNG. Of course you have to substitute your_path for something sensible
		if (!Directory.Exists(subPath))
		{
			Directory.CreateDirectory(subPath);
		}

		File.WriteAllBytes(filePath, bytes);

		actualPhotoAction.answer(null);
	}

	public Texture2D loadPhoto(string filePath){
		Texture2D tex = new Texture2D(2, 2);
		byte[] pixels = File.ReadAllBytes(filePath);

        tex.LoadImage(pixels);

		return tex;
	}
}