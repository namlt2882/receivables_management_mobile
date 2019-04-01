using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.FirebasePushNotification;
using Android.Content;
using Android.Provider;
using Android.Database;
using System.Collections.Generic;
using Xamarin.Forms;
using Android.Graphics;
using Android.Media;
using System.IO;
using Plugin.Permissions;
using Plugin.Media;
using ScanbotSDK.Xamarin.Forms;
using IO.Scanbot.Sdk;

namespace RCM.Mobile.Droid
{
    [Activity(Label = "RCM.Mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //public static int OPENGALLERYCODE = 100;
        const string licenseKey =
  "BsaBg/yi9xggKTJmLCHVqRMSXVxXrN" +
  "t3Y8Hh3orjhjoKKdm+5kMovpbk21yX" +
  "Rg89XXkn9CzuqwO5+jBmQZOYjXq9j0" +
  "62yYpFG86ftxL+zRCbTiKXC2Uo8pdA" +
  "DSogUVRPO/z52XjelfTNzEuzMOULQd" +
  "GowT1KypY6jrmTJ5Xkdv2Iu/lAopob" +
  "LgbjafzI16cZkCoe1fo25CfOOQ+U48" +
  "19LNYxOS2Z1hNhQX/DbjmKgtwv6VUx" +
  "40BziUDl5JOnNP1/8d/43Y+15IZ3ky" +
  "aZ10yhol6Ld2f+f08k8zFtEbN+007K" +
  "AJP7SBsCqLKI5HoSig2FNcC14wQ6BT" +
  "O9oP0fu1BabQ==\nU2NhbmJvdFNESw" +
  "pjb20uY29tcGFueW5hbWUuUkNNLk1v" +
  "YmlsZQoxNTU2NDA5NTk5CjU5MAoz\n";

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            FirebasePushNotificationManager.ProcessIntent(this, Intent);

            //#if DEBUG
            //            FirebasePushNotificationManager.Initialize(this, new PushNotificationHandler(), true);
            //#else
            //                  FirebasePushNotificationManager.Initialize(this,new CustomPushHandler(),false);
            //#endif
            CrossMedia.Current.Initialize();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            #region PDF
            SBSDKInitializer.Initialize(this.Application, licenseKey,
                new SBSDKConfiguration { EnableLogging = true, StorageBaseDirectory = GetStorageBaseDirectory() });
            #endregion
        }

        string GetStorageBaseDirectory()
        {
            
            //var customDocumentsFolder = System.IO.Path.Combine(
            //    System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Evidences");
            //Directory.CreateDirectory(customDocumentsFolder);
            //return customDocumentsFolder;
            var externalPublicPath = System.IO.Path.Combine(
                Android.OS.Environment.ExternalStorageDirectory.Path, "RCM-Evidences");
            Directory.CreateDirectory(externalPublicPath);
            return externalPublicPath;

            //var customDocumentsFolder = System.Environment.SpecialFolder.MyDocuments + "/Evidences";
            //Directory.CreateDirectory(customDocumentsFolder);
            //return customDocumentsFolder;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);

        //    if (requestCode == OPENGALLERYCODE && resultCode == Result.Ok)
        //    {
        //        List<string> images = new List<string>();

        //        if (data != null)
        //        {
        //            ClipData clipData = data.ClipData;
        //            if (clipData != null)
        //            {
        //                for (int i = 0; i < clipData.ItemCount; i++)
        //                {
        //                    ClipData.Item item = clipData.GetItemAt(i);
        //                    Android.Net.Uri uri = item.Uri;
        //                    var path = GetRealPathFromURI(uri);

        //                    if (path != null)
        //                    {
        //                        //Rotate Image
        //                        var imageRotated = ImageHelpers.RotateImage(path);
        //                        var newPath = ImageHelpers.SaveFile("TmpPictures", imageRotated, System.DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        //                        images.Add(newPath);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Android.Net.Uri uri = data.Data;
        //                var path = GetRealPathFromURI(uri);

        //                if (path != null)
        //                {
        //                    //Rotate Image
        //                    var imageRotated = ImageHelpers.RotateImage(path);
        //                    var newPath = ImageHelpers.SaveFile("TmpPictures", imageRotated, System.DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        //                    images.Add(newPath);
        //                }
        //            }

        //            MessagingCenter.Send<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelected", images);
        //        }
        //    }
        //}



        //public String GetRealPathFromURI(Android.Net.Uri contentURI)
        //{
        //    try
        //    {
        //        ICursor imageCursor = null;
        //        string fullPathToImage = "";

        //        imageCursor = ContentResolver.Query(contentURI, null, null, null, null);
        //        imageCursor.MoveToFirst();
        //        int idx = imageCursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);

        //        if (idx != -1)
        //        {
        //            fullPathToImage = imageCursor.GetString(idx);
        //        }
        //        else
        //        {
        //            ICursor cursor = null;
        //            var docID = DocumentsContract.GetDocumentId(contentURI);
        //            var id = docID.Split(':')[1];
        //            var whereSelect = MediaStore.Images.ImageColumns.Id + "=?";
        //            var projections = new string[] { MediaStore.Images.ImageColumns.Data };

        //            cursor = ContentResolver.Query(MediaStore.Images.Media.InternalContentUri, projections, whereSelect, new string[] { id }, null);
        //            if (cursor.Count == 0)
        //            {
        //                cursor = ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, projections, whereSelect, new string[] { id }, null);
        //            }
        //            var colData = cursor.GetColumnIndexOrThrow(MediaStore.Images.ImageColumns.Data);
        //            cursor.MoveToFirst();
        //            fullPathToImage = cursor.GetString(colData);
        //        }
        //        return fullPathToImage;
        //    }
        //    catch (Exception ex)
        //    {
        //        Toast.MakeText(Xamarin.Forms.Forms.Context, "Unable to get path", ToastLength.Long).Show();

        //    }

        //    return null;

        //}

    }
}