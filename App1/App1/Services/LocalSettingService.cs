using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Foundation.Collections;

namespace App1.Services;
public class LocalSettingService
{
    ApplicationDataContainer lst = ApplicationData.Current.LocalSettings;
    public bool IsContainerExist(string container)
    {
        return lst.Containers.ContainsKey(container);
    }
    public void CreatContainer(string container)
    {
        lst.CreateContainer(container, ApplicationDataCreateDisposition.Always);
    }
    public bool IsKeyExist(string container,string key)
    {
        return lst.Containers[container].Values.ContainsKey(key);
    }
    public void SaveSetting(string container,string key,string value)
    {
        if (!IsContainerExist(container)) CreatContainer(container);
        lst.Containers[container].Values[key] = value;
    }
    public string ReadSetting(string container, string key)
    {
        if (!IsContainerExist(container)) CreatContainer(container);
        var t = lst.Containers[container].Values[key].ToString();
         return t==null?"":t;
    }
    public IPropertySet GetKeys(string container)
    {
        if (!IsContainerExist(container)) CreatContainer(container);
        return lst.Containers[container].Values;
    }
    public void DeleteSetting(string container, string key)
    {
        if (!IsContainerExist(container)) CreatContainer(container);
        lst.Containers[container].Values.Remove(key);
    }
    public void SaveSetting(string key, string value)
    {
        lst.Values[key] = value;
    }
    public string ReadSetting( string key)
    {
        return lst.Values[key].ToString();
    }
}
