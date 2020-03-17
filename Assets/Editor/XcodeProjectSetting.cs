using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
#endif


public class XcodeProjectSetting
{
    //该属性是在build完成后，被调用的callback
    [PostProcessBuildAttribute(0)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        // BuildTarget需为iOS
        if (buildTarget != BuildTarget.iOS)
            return;

        // 初始化
        var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);
        string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

        // 一、 修改参数, 添加framework, lib ---------------------------------------------------------------------------------------------
        //// 1. 添加flag
        //pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
        // 2. 设置Bitcode
        //pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
        //pbxProject.SetBuildProperty(targetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

        // 3. 添加framwrok
        //pbxProject.AddFrameworkToProject(targetGuid, "Security.framework", false);
        //pbxProject.AddFrameworkToProject(targetGuid, "CoreImage.framework", false);
        //pbxProject.AddFrameworkToProject(targetGuid, "Foundation.framework", false);
        //pbxProject.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
        //pbxProject.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", false);
        //pbxProject.AddFrameworkToProject(targetGuid, "CoreGraphics.framework", false);
        //pbxProject.AddFrameworkToProject(targetGuid, "ImageIO.framework", false);
        //pbxProject.AddFrameworkToProject(targetGuid, "CoreData.framework", false);

        // 4. 添加lib
        //AddLibToProject(pbxProject, targetGuid, "libsqlite3.tbd");
        //AddLibToProject(pbxProject, targetGuid, "libc++.tbd");
        //AddLibToProject(pbxProject, targetGuid, "libz.tbd");
        //AddLibToProject(pbxProject, targetGuid, "libresolv.tbd");

        // 应用修改
        File.WriteAllText(projectPath, pbxProject.WriteToString());


        // 二、 设置Info.plist ---------------------------------------------------------------------------------------------
        // 修改Info.plist文件
        var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        //// 插入URL Scheme到Info.plsit（理清结构）
        //var array = plist.root.CreateArray("CFBundleURLTypes");
        ////插入dict
        //var urlDict = array.AddDict();
        //urlDict.SetString("CFBundleTypeRole", "Editor");
        ////插入array
        //var urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
        //urlInnerArray.AddString("blablabla");

        //添加麦克风权限
        //plist.root.SetString("NSMicrophoneUsageDescription", "开启麦克风");
        plist.root.SetString("NSCameraUsageDescription", "请允许打开摄像头");  //相机
        plist.root.SetString("NSPhotoLibraryUsageDescription", "App需要您的同意,才能访问相册");  //相机
        plist.root.SetString("NSPhotoLibraryAddUsageDescription", "App需要您的同意,才能访问相册");  //相机
        // 应用修改
        plist.WriteToFile(plistPath);


        // 三、 插入代码 ---------------------------------------------------------------------------------------------
        ////读取UnityAppController.mm文件
        //string unityAppControllerPath = pathToBuiltProject + "/Classes/UnityAppController.mm";
        //XClass UnityAppController = new XClass(unityAppControllerPath);

        ////在指定代码后面增加一行代码
        //UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"", "#import <UMSocialCore/UMSocialCore.h>");

        //string newCode = "\n" +
        //           "    [[UMSocialManager defaultManager] openLog:YES];\n" +
        //           "    [UMSocialGlobal shareInstance].type = @\"u3d\";\n" +
        //           "    [[UMSocialManager defaultManager] setUmSocialAppkey:@\"" + "\"];\n" +
        //           "    [[UMSocialManager defaultManager] setPlaform:UMSocialPlatformType_WechatSession appKey:@\"" + "\" appSecret:@\"" + "\" redirectURL:@\"http://mobile.umeng.com/social\"];\n" +
        //           "    \n"
        //           ;
        ////在指定代码后面增加一大行代码
        //UnityAppController.WriteBelow("// if you wont use keyboard you may comment it out at save some memory", newCode);

    }

    //添加lib方法
    static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
    {
        string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }
}