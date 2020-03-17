using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


namespace Tackor.Other
{
    public class Demo : MonoBehaviour
    {
        public Button phoneBtn;
        public Button momentBtn;
        public Button cameraBtn;
        public Button screenshotBtn;
        public Image img;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Unity 打开相册 ----> ");
            IOSAlbumCamera.Instance.CallBack_PickImage_With_Base64 += GetImgDataFromAlbum;
            IOSAlbumCamera.Instance.CallBack_ImageSavedToAlbum += ScreenShotImgFinished;

            phoneBtn.onClick.AddListener(GetPictureFromPhone);
            momentBtn.onClick.AddListener(GetPictureFromMoment);
            cameraBtn.onClick.AddListener(OpenCamera);
            screenshotBtn.onClick.AddListener(ScreenShotAndSaveToAlbum);
        }

        void TODO()
        {
            /// NSPhotoLibraryAddUsageDescription
        }

        /// <summary>
        /// 从相册_照片获取图片
        /// </summary>
        void GetPictureFromPhone()
        {
            Debug.Log("Unity 打开相册_照片 --> ");
            IOSAlbumCamera.iosOpenPhotoLibrary(false);
        }

        /// <summary>
        /// 从相册_时刻获取图片
        /// </summary>
        void GetPictureFromMoment()
        {
            Debug.Log("Unity 打开相册_时刻 --> ");
            IOSAlbumCamera.iosOpenPhotoAlbums(false);
        }


        /// <summary>
        /// 从摄像头获取(图片不会保存到相册中)
        /// </summary>
        void OpenCamera()
        {
            Debug.Log("Unity 打开相机 --> ");
            IOSAlbumCamera.iosOpenCamera(false);
        }

        /// <summary>
        /// 截屏(并保存到相册中)
        /// </summary>
        void ScreenShotAndSaveToAlbum()
        {
            Debug.Log("Unity 截屏并保存到相册 --> ");
            captureScreenshot();
        }

        void captureScreenshot()
        {
            string path = Application.persistentDataPath + "/screenshot.png";
            Debug.Log("Unity 保存路径:::::: " + path);

            StartCoroutine(SaveToPhoto(path));
        }

        IEnumerator SaveToPhoto(string imgPath)
        {
            yield return new WaitForEndOfFrame();

            Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
            //Get Image from screen
            screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenImage.Apply();
            //Convert to png
            byte[] imageBytes = screenImage.EncodeToPNG();
            //Save image to file
            File.WriteAllBytes(imgPath, imageBytes);

            //给个截屏效果
            Color tempC = img.color;
            tempC.a = 0;
            img.color = tempC;
            yield return new WaitForSeconds(0.15f);
            tempC.a = 1;
            img.color = tempC;

            IOSAlbumCamera.iosSaveImageToPhotosAlbum(imgPath);

        }

        #region iOS 的回调方法
        /// <summary>
        /// 从相册_时刻/照片中获取图片成功回调
        /// </summary>
        /// <param name="imgDataStr"></param>
        void GetImgDataFromAlbum(string imgDataStr)
        {
            Debug.Log("Unity 调用相册 完成回调");

            Texture2D tex = IOSAlbumCamera.Base64StringToTexture2D(imgDataStr);
            img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            Debug.Log("tackor tex.w= " + tex.width + "; tex.h= " + tex.height + "; sprite= " + img.sprite);
        }

        /// <summary>
        /// iOS截屏完成
        /// </summary>
        void ScreenShotImgFinished(string imgDataStr)
        {
            Debug.Log("Unity 截屏 完成回调");
        }
        #endregion

    }
}

