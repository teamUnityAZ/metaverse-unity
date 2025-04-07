using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Assets
{
    public string assetsPathandroid;
    public string assetsPathios;
    // public string assetsPath;
    public string assetname;
    public string assetpreviewlink;
    public string category;

    public Assets(string assetsPathandroid, string assetsPathIos, string assetname, string assetpreviewlink, string category)
    {
        this.assetsPathandroid = assetsPathandroid;
        this.assetname = assetname;
        this.assetpreviewlink = assetpreviewlink;
        this.category = category;
    }

}
