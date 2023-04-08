using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace App1.Services;
public class FileServices
{
    /// <summary>
    /// 各种路径
    /// </summary>
    public StorageFolder InstalledLocation = Package.Current.InstalledLocation;
    public StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
    public string DesktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    public string MyDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public string StartMenuFolder = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
    public string MyPicturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
    public string MyMusicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
    public string MyVideosFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
     public async Task<BitmapImage> GetStorageFileIcon(string path)
    {
        var bitmapimage = new BitmapImage();
        {
            try
            {

                StorageItemThumbnail thumbnail;

                StorageFile file = await StorageFile.GetFileFromPathAsync(path);
                thumbnail = await file.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem);

                InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
                await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
                randomAccessStream.Seek(0);
                bitmapimage.SetSource(randomAccessStream);
            }
            catch
            {
                bitmapimage = new BitmapImage(new Uri("ms-appx:/imgs/emptyfile.png"));

            }
            //Icons.Add(path,bitmapimage);
        }

        return bitmapimage;
    }
    /// <summary>
    /// picture Storagefile to bitmapimage
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
public async Task<BitmapImage> ReadStorageFileToBitmapImage (StorageFile f)
    {
        BitmapImage bitmap = new();
        using (var stream = await f.OpenAsync(FileAccessMode.ReadWrite))
        {
            bitmap.SetSource(stream);
        }
        return bitmap;
    }
    /// <summary>
    /// 选择文件夹 await new KSFileService().ChooseFolder(App._Window_Hamburg);
    /// </summary>
    /// <returns></returns>
    public async Task<StorageFolder> ChooseFolder(Window window)
    {
        var folderPicker = new FolderPicker();
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
        folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        folderPicker.FileTypeFilter.Add("*");
        StorageFolder folder = await folderPicker.PickSingleFolderAsync();
        return folder;
    }
    /// <summary>
    /// 选择文件 (window=App._Window_Hamburg) new string[]{".jpg",".png"}
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="filetypes"></param>
    /// <returns></returns>
    public async Task<StorageFile> ChooseFile(Window window,PickerLocationId pid, string[] filetypes)
    {
        FileOpenPicker openPicker = new FileOpenPicker();
        openPicker.ViewMode = PickerViewMode.List;
        openPicker.SuggestedStartLocation = pid;
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);
        foreach (var filetype in filetypes)
        {
            openPicker.FileTypeFilter.Add(filetype);
        }
        var file = await openPicker.PickSingleFileAsync();
        return file;
    }
    /// <summary>
    /// 保存文件(App._Window_Hamburg)
    /// </summary>
    /// <param name="info"></param>
    /// <param name="types"></param>
    /// <param name="filename"></param>
    /// <param name="txt"></param>
    async public void SaveFileWithTxt(Window window,string info, List<string> types, string filename, string txt)
    {
        var saveFile = new FileSavePicker();
        saveFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        saveFile.FileTypeChoices.Add(info, types);
        saveFile.SuggestedFileName = filename;
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(saveFile, hwnd);
        StorageFile sFile = await saveFile.PickSaveFileAsync();
        if (sFile != null)
        {
            using (StorageStreamTransaction transaction = await sFile.OpenTransactedWriteAsync())
            {
                using (DataWriter dataWriter = new DataWriter(transaction.Stream))
                {
                    dataWriter.WriteString(txt);
                    transaction.Stream.Size = await dataWriter.StoreAsync();
                    await transaction.CommitAsync();
                }
            }
        }
    }
    async public void SaveFileByStorageFile(Window window, string info, List<string> types, StorageFile file)
    {
        var saveFile = new FileSavePicker();
        saveFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        saveFile.FileTypeChoices.Add(info, types);
        saveFile.SuggestedFileName = file.DisplayName;
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(saveFile, hwnd);
        StorageFile sFile = await saveFile.PickSaveFileAsync();
        if (sFile != null)
        {
            sFile = file;
        }
    }
    /// <summary>
    /// 往LocalFolder中写文件
    /// </summary>
    /// <param name="dirname"></param>
    /// <param name="name"></param>
    /// <param name="txt"></param>
    public async void WriteFileInLocalFolder(string dirname, string name, string txt)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname == "")
            {
                storageFolder = ApplicationData.Current.LocalFolder;
            }
            else
            {
                storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(dirname, CreationCollisionOption.OpenIfExists);
            }
            StorageFile file = await storageFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, txt);
        }
        catch { }
    }
    /// <summary>
    /// 往LocalFolder中读文件
    /// </summary>
    /// <param name="dirname"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<string> ReadFileInLocalFolder(string dirname, string name)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname == "")
            {
                storageFolder = ApplicationData.Current.LocalFolder;
            }
            else
            {
                storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(dirname, CreationCollisionOption.OpenIfExists);
            }
            StorageFile file = await storageFolder.GetFileAsync(name);
            return await FileIO.ReadTextAsync(file);
        }
        catch
        {
            return string.Empty;
        }
    }
    /// <summary>
    /// 往LocalFolder中删文件
    /// </summary>
    /// <param name="dirname"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<bool> DelFileInLocalFolder(string dirname, string name)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname == "")
            {
                storageFolder = ApplicationData.Current.LocalFolder;
            }
            else
            {
                storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(dirname);
            }
            StorageFile file = await storageFolder.GetFileAsync(name);
            await file.DeleteAsync();
            return true;
        }
        catch { return false; }
    }
    /// <summary>
    /// 往LocalFolder中获得文件列表
    /// </summary>
    /// <param name="dirname"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<StorageFile>> GetFilesInLocalFolder(string dirname)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname == "")
            {
                storageFolder = ApplicationData.Current.LocalFolder;
            }
            else
            {
                storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(dirname);
            }
            return await storageFolder.GetFilesAsync();
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 判断LocalFolder中文件存在
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="dirname"></param>
    /// <returns></returns>
    public async Task<bool> DoesFileExistAsyncInLocalFolder(string fileName, string dirname)
    {
        try
        {
            StorageFolder storageFolder;
            if (dirname != "")
                storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(dirname);
            else
                storageFolder = ApplicationData.Current.LocalFolder;
            await storageFolder.GetFileAsync(fileName);
            return true;
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// 读取StorageFile的txt
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<string> ReadFileFromStorageFile(StorageFile file)
    {
        string str = "";
        try
        {
            str = await Windows.Storage.FileIO.ReadTextAsync(file);
        }
        catch (ArgumentOutOfRangeException)
        {
            //using(var stream =new StreamReader((await file.OpenReadAsync()).GetInputStreamAt(0).AsStreamForRead()))
            //{
            //    string text = stream.ReadToEnd();
            //    return text;
            //}
            IBuffer buffer = await FileIO.ReadBufferAsync(file);
            DataReader reader = DataReader.FromBuffer(buffer);
            byte[] fileContent = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(fileContent);
            string text = "";

            // Encoding.ASCII.GetString(fileContent, 0, fileContent.Length);

            //text= Encoding.GetEncoding(0).GetString(fileContent, 0, fileContent.Length);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding gbk = Encoding.GetEncoding("GBK");

            text = gbk.GetString(fileContent);
            //string text = AutoEncoding(new byte[4] { fileContent[0], fileContent[1], fileContent[2], fileContent[3] }).GetString(fileContent);

            return text;
        }
        return str;
    }
    /// <summary>
    /// path:/Assets/a.mp3
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<StorageFile> GetFileFromApplicationByPath(string path)
    {
        return await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx://" + path));
    }
}
