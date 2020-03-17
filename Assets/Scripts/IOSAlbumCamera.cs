
using UnityEngine;
using System.Runtime.InteropServices;


public class IOSAlbumCamera : MonoBehaviour
{
    #if UNITY_IPHONE
    
    /// <summary>
    /// 打开照片
    /// </summary>
    /// <param name="allowsEditing">true: 需要对照片进行编辑反之不需要编译</param>
    public static void iosOpenPhotoLibrary(bool allowsEditing=false)
    {
#if UNITY_EDITOR
        Debug.LogError("Editor模式下 iOSAlbumCamera 无法使用 --> ");
#elif UNITY_IOS
        if(allowsEditing)
        _iosOpenPhotoLibrary_allowsEditing();
        else
        _iosOpenPhotoLibrary();
#endif
    }


    /// <summary>
    /// 打开相册
    /// </summary>
    /// <param name="allowsEditing">true: 需要对照片进行编辑反之不需要编译</param>
    public static void iosOpenPhotoAlbums(bool allowsEditing=false)
    {
#if UNITY_EDITOR
        Debug.LogError("Editor模式下 iOSAlbumCamera 无法使用 --> ");
#elif UNITY_IOS
        if(allowsEditing)
        _iosOpenPhotoAlbums_allowsEditing();
        else
        _iosOpenPhotoAlbums();
#endif
    }


    /// <summary>
    /// 打开相机
    /// </summary>
    /// <param name="allowsEditing">true: 需要对拍得的照片进行编辑反之不需要编译</param>
    public static void iosOpenCamera(bool allowsEditing=false)
    {
#if UNITY_EDITOR
        Debug.LogError("Editor模式下 iOSAlbumCamera 无法使用 --> ");
#elif UNITY_IOS
        if(allowsEditing)
        _iosOpenCamera_allowsEditing();
        else
        _iosOpenCamera();
#endif
    }


    /// <summary>
    /// 保存图片到相册
    /// </summary>
    /// <param name="imgPath">待放入相册的照片的地址</param>
    public static void iosSaveImageToPhotosAlbum(string imgPath)
    {
#if UNITY_EDITOR
        Debug.LogError("Editor模式下 iOSAlbumCamera 无法使用 --> ");
#elif UNITY_IOS
        _iosSaveImageToPhotosAlbum(imgPath);
#endif
    }

    /// <summary>
    /// 将ios传过的string转成u3d中的texture
    /// </summary>
    /// <param name="base64">图片数据流</param>
    /// <returns></returns>
    public static Texture2D Base64StringToTexture2D(string base64)
    {
        Texture2D tex = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        try
        {
            byte[] bytes = System.Convert.FromBase64String(base64);
            tex.LoadImage(bytes);
            Debug.Log("Unity tex " + tex.width + "; height= " + tex.height);
        }
        catch(System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        return tex;
    }

    #region 从iOS回调的方法
    /// <summary>
    /// 打开相册相机后的从ios回调到unity的方法
    /// </summary>
    /// <param name="base64"></param>
    void PickImageCallBack_Base64(string base64)
    {
        if (base64.Equals("Cancel"))  //未选择任何图片,取消了
        {
            Debug.Log("Unity 取消选择 --> ");
            return;
        }

        if(CallBack_PickImage_With_Base64!=null)
        {
            CallBack_PickImage_With_Base64(base64);
        }
    }

    /// <summary>
    /// 保存图片到相册后，从ios回调到unity的方法
    /// </summary>
    /// <param name="msg"></param>
    void SaveImageToPhotosAlbumCallBack(string msg)
    {
        Debug.Log(" -- msg: " + msg);
        if(CallBack_ImageSavedToAlbum!=null)
        {
            CallBack_ImageSavedToAlbum(msg);
        }
    }
    #endregion

    [DllImport("__Internal")]
    private static extern void _iosOpenPhotoLibrary();
    [DllImport("__Internal")]
    private static extern void _iosOpenPhotoAlbums();
    [DllImport("__Internal")]
    private static extern void _iosOpenCamera();
    [DllImport("__Internal")]
    private static extern void _iosOpenPhotoLibrary_allowsEditing();
    [DllImport("__Internal")]
    private static extern void _iosOpenPhotoAlbums_allowsEditing();
    [DllImport("__Internal")]
    private static extern void _iosOpenCamera_allowsEditing();
    [DllImport("__Internal")]
    private static extern void _iosSaveImageToPhotosAlbum(string readAddr);


    private static IOSAlbumCamera _instance;
    public static IOSAlbumCamera Instance { get { return _instance; } }

    public System.Action<string> CallBack_PickImage_With_Base64;
    public System.Action<string> CallBack_ImageSavedToAlbum;

    void Awake()
    {
        if (_instance != null)
        {
            DestroyImmediate(this);
            return;
        }
        _instance = this;
    }


#endif
}
