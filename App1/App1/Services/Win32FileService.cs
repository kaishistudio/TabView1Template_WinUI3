using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace App1.Services;
//需引入System.Management包
public class Win32FileService
{
    /*
    /// <summary>
    /// 获取本地电脑所有的盘符
    /// </summary>
    /// <returns></returns>
    public List<string> GetRemovableDeviceID()
    {
        List<string> deviceIDs = new List<string>();
        ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT  *  From  Win32_LogicalDisk ");
        ManagementObjectCollection queryCollection = query.Get();
        foreach (ManagementObject mo in queryCollection)
        {
            switch (int.Parse(mo["DriveType"].ToString()))
            {
                case (int)DriveType.Removable:   //可以移动磁盘     
                    {
                        //可以移动磁盘
                        deviceIDs.Add(mo["DeviceID"].ToString());
                        break;
                    }
                case (int)DriveType.Fixed:   //本地磁盘     
                    {
                        //本地磁盘
                        deviceIDs.Add(mo["DeviceID"].ToString());
                        break;
                    }
                case (int)DriveType.CDRom:   //CD   rom   drives     
                    {
                        //CD   rom   drives 
                        break;
                    }
                case (int)DriveType.Network:   //网络驱动   
                    {
                        //网络驱动器 
                        break;
                    }
                case (int)DriveType.Ram:
                    {
                        //驱动器是一个 RAM 磁盘 
                        break;
                    }
                case (int)DriveType.NoRootDirectory:
                    {
                        //驱动器没有根目录 
                        break;
                    }
                default:   //defalut   to   folder     
                    {
                        //驱动器类型未知
                        break;
                    }
            }
        }
        return deviceIDs;
    }
    /// <summary>
    /// 获取本地库
    /// </summary>
    /// <returns></returns>
    public List<string> GetSystemFolders()
    {
        List<string> _StartUserList = new List<string>();
        var n1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        _StartUserList.Add(n1);
        var n2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        _StartUserList.Add(n2);
        var n3 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        _StartUserList.Add(n3);
        var n4 = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        _StartUserList.Add(n4);
        var n5 = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        _StartUserList.Add(n5);
        return _StartUserList;
    }
    /// <summary>
    /// 获得文件图标
    /// </summary>
    /// <returns></returns>
    public async Task<BitmapImage> GetFileIcon(string path)
    {
        var bitmapimage = new BitmapImage();
        try
        {
            StorageItemThumbnail thumbnail;
            if (Directory.Exists(path))
            {
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
                thumbnail = await folder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem);
            }
            else
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(path);
                thumbnail = await file.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem);
            }
            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
            await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
            randomAccessStream.Seek(0);
            bitmapimage.SetSource(randomAccessStream);
        }
        catch
        {
            if (Directory.Exists(path))
            {
                bitmapimage = new BitmapImage(new Uri("ms-appx:/imgs/dir.png"));
            }
            else
            {
                bitmapimage = new BitmapImage(new Uri("ms-appx:/imgs/emptyfile.png"));
            }
        }
        return bitmapimage;
    }
    #region 调用系统的文件复制移动删除
    [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool SHFileOperation([In, Out] SHFILEOPSTRUCT str);
    private const int FO_MOVE = 0x1;
    private const int FO_COPY = 0x2;
    private const int FO_DELETE = 0x3;
    private const ushort FOF_NOCONFIRMATION = 0x10;
    private const ushort FOF_ALLOWUNDO = 0x40;
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class SHFILEOPSTRUCT
    {

        public IntPtr hwnd;

        /// <summary> 

        /// 设置操作方式，移动：FO_MOVE，复制：FO_COPY，删除：FO_DELETE 

        /// </summary> 

        public UInt32 wFunc;

        /// <summary> 

        /// 源文件路径 

        /// </summary> 

        public string pFrom;

        /// <summary> 

        /// 目标文件路径 

        /// </summary> 

        public string pTo;

        /// <summary> 

        /// 允许恢复 

        /// </summary> 

        public UInt16 fFlags;

        /// <summary> 

        /// 监测有无中止 

        /// </summary> 

        public Int32 fAnyOperationsAborted;

        public IntPtr hNameMappings;

        /// <summary> 

        /// 设置标题 

        /// </summary> 

        public string lpszProgressTitle;

    }
    #endregion
    /// <summary>
    /// 调用系统复制功能
    /// </summary>
    /// <param name="SourceFileName">文件源</param>
    /// <param name="DestFileName">目标路径</param>
    /// <returns>是否成功复制</returns>
    public bool CopyFile(string SourceFileName, string DestFileName)
    {

        SHFILEOPSTRUCT pm = new SHFILEOPSTRUCT();

        pm.wFunc = FO_COPY;

        pm.pFrom = SourceFileName + "\0";

        pm.pTo = DestFileName + "\0";

        pm.fFlags = FOF_ALLOWUNDO;//允许恢复 

        pm.lpszProgressTitle = "Copy";

        return !SHFileOperation(pm);
    }
    /// <summary>
    /// 调用系统移动功能
    /// </summary>
    /// <param name="SourceFileName">文件源</param>
    /// <param name="DestFileName">目标路径</param>
    /// <returns>是否成功复制</returns>
    public bool MoveFile(string SourceFileName, string DestFileName)
    {

        SHFILEOPSTRUCT pm = new SHFILEOPSTRUCT();

        pm.wFunc = FO_MOVE;

        pm.pFrom = SourceFileName + "\0";

        pm.pTo = DestFileName + "\0";

        pm.fFlags = FOF_ALLOWUNDO;//允许恢复 

        pm.lpszProgressTitle = "Move";

        return !SHFileOperation(pm);
    }
    public enum FileFuncFlags : uint
    {
        FO_MOVE = 0x1,
        FO_COPY = 0x2,
        FO_DELETE = 0x3,
        FO_RENAME = 0x4
    }
    [Flags]
    public enum FILEOP_FLAGS : ushort
    {
        FOF_MULTIDESTFILES = 0x1,
        FOF_CONFIRMMOUSE = 0x2,
        FOF_SILENT = 0x4,
        FOF_RENAMEONCOLLISION = 0x8,
        FOF_NOCONFIRMATION = 0x10,
        FOF_WANTMAPPINGHANDLE = 0x20,
        FOF_ALLOWUNDO = 0x40,
        FOF_FILESONLY = 0x80,
        FOF_SIMPLEPROGRESS = 0x100,
        FOF_NOCONFIRMMKDIR = 0x200,
        FOF_NOERRORUI = 0x400,
        FOF_NOCOPYSECURITYATTRIBS = 0x800,
        FOF_NORECURSION = 0x1000,
        FOF_NO_CONNECTED_ELEMENTS = 0x2000,
        FOF_WANTNUKEWARNING = 0x4000,
        FOF_NORECURSEREPARSE = 0x8000
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Win32SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        public FileFuncFlags wFunc;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pFrom;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pTo;
        public FILEOP_FLAGS fFlags;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fAnyOperationsAborted;
        public IntPtr hNameMappings;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszProgressTitle;
    }
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int SHFileOperation([In] ref Win32SHFILEOPSTRUCT lpFileOp);
    /// <summary>
    /// 调用Win32的复制功能
    /// </summary>
    /// <returns></returns>
    public bool Win32CopyFile(string SourceFileName, string DestFileName)
    {
        Win32SHFILEOPSTRUCT op = new Win32SHFILEOPSTRUCT();
        op.hwnd = IntPtr.Zero;
        op.wFunc = FileFuncFlags.FO_COPY;
        op.pFrom = SourceFileName + "\0";// 需要注意，最后需要加入"\0"表示字符串结束，如果需要拷贝多个文件，则 file1 + "\0" + file2 + "\0"...
        op.pTo = DestFileName + "\0";// 需要注意，最后需要加入"\0"表示字符串结束
        op.hNameMappings = IntPtr.Zero;
        op.fFlags = FILEOP_FLAGS.FOF_NOCONFIRMMKDIR;
        op.fAnyOperationsAborted = false;
        int ret = SHFileOperation(ref op);
        return ret == 0;
    }
    /// <summary>
    /// 调用Win32的复制功能
    /// </summary>
    /// <returns></returns>
    public bool Win32MoveFile(string SourceFileName, string DestFileName)
    {
        Win32SHFILEOPSTRUCT op = new Win32SHFILEOPSTRUCT();
        op.hwnd = IntPtr.Zero;
        op.wFunc = FileFuncFlags.FO_MOVE;
        op.pFrom = SourceFileName + "\0";// 需要注意，最后需要加入"\0"表示字符串结束，如果需要拷贝多个文件，则 file1 + "\0" + file2 + "\0"...
        op.pTo = DestFileName + "\0";// 需要注意，最后需要加入"\0"表示字符串结束
        op.hNameMappings = IntPtr.Zero;
        op.fFlags = FILEOP_FLAGS.FOF_NOCONFIRMMKDIR;
        op.fAnyOperationsAborted = false;
        int ret = SHFileOperation(ref op);
        return ret == 0;
    }
    [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int SHFileOperation([In, Out] _SHFILEOPSTRUCT str);
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class _SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        public UInt32 wFunc;
        public string pFrom;
        public string pTo;
        public UInt16 fFlags;
        public Int32 fAnyOperationsAborted;
        public IntPtr hNameMappings;
        public string lpszProgressTitle;
    }
    /// <summary>
    /// 调用Win32系统的复制功能
    /// </summary>
    /// <returns></returns>
    public int Win32DeleteFile(string path)
    {
        _SHFILEOPSTRUCT pm = new _SHFILEOPSTRUCT();
        pm.wFunc = FO_DELETE;
        pm.pFrom = path + '\0';
        pm.pTo = null;
        pm.fFlags = FOF_ALLOWUNDO;
        return SHFileOperation(pm);
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct SHELLEXECUTEINFO
    {
        public int cbSize;
        public uint fMask;
        public IntPtr hwnd;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpVerb;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpFile;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpParameters;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpDirectory;
        public int nShow;
        public IntPtr hInstApp;
        public IntPtr lpIDList;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpClass;
        public IntPtr hkeyClass;
        public uint dwHotKey;
        public IntPtr hIcon;
        public IntPtr hProcess;
    }
    private const int SW_SHOW = 5;
    private const uint SEE_MASK_INVOKEIDLIST = 12;
    [DllImport("shell32.dll")]
    static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);
    /// <summary>
    /// 调用系统的属性功能
    /// </summary>
    /// <returns></returns>
    public void GetSystemAttribute(string path)
    {
        try
        {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = path;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            ShellExecuteEx(ref info);
        }
        catch { }
    }
    /// <summary>
    /// 调用系统打开方式
    /// </summary>
    /// <returns></returns>
    public void GetSystemOpenAs(string path)
    {
        try
        {
            System.Diagnostics.Process.Start("rundll32.exe", "shell32.dll, OpenAs_RunDLL " + path);
        }
        catch { }
    }*/
}
