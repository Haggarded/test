using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
public class luacontroller : MonoBehaviour
{
    public TextAsset luascript;
    internal static LuaEnv newlua=new LuaEnv();
    private Action luastart;
    private Action luaupdate;
    private Action luaondestory;
    private LuaTable scripttable;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        scripttable=newlua.NewTable();
        LuaTable meta=newlua.NewTable();
        meta.Set("__index",newlua.Global);
        scripttable.SetMetaTable(meta);   
        meta.Dispose();
        scripttable.Set("player",GameObject.FindGameObjectWithTag("Player"));
        scripttable.Set("enemy",GameObject.FindGameObjectWithTag("enemy"));
        newlua.DoString(luascript.text,"luatest",scripttable);
        Action luaawake=scripttable.Get<Action>("awake");
        scripttable.Get("start",out luastart);
        scripttable.Get("update",out  luaupdate);
    }
    // Start is called before the first frame update
    void Start()
    {
         if(luastart!=null)
        {
            luastart();
        }
    }

    // Update is called once per frame
    void Update()
    {
         if(luaupdate!=null)
        {
            luaupdate();
            GameObject.FindGameObjectWithTag("Player").transform.Translate(Vector3.right);
        }
    }
     void ondestory()
    {
        if(luaondestory!=null)
        {
            luaondestory();
        }
        luaondestory=null;
        luaupdate=null;
        luastart=null;
        scripttable.Dispose();
        newlua=null;
    }
}
