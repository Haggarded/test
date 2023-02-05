using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
using UnityEngine.UI;
public class luacontroller : MonoBehaviour
{
    public TextAsset luascript;
    internal static LuaEnv newlua=new LuaEnv();
    private Action luastart;
    private Action luaupdate;
    private Action luaondestory;
    private LuaTable scripttable;
    public Button AttackButton;
    public GameObject Preferb;
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
        AttackButton = GameObject.Find("attack").GetComponent<Button>();
        newlua.DoString(luascript.text, "luacontroller", scripttable);
        Action luaawake=scripttable.Get<Action>("awake");
        scripttable.Get("start",out luastart);
        scripttable.Get("update",out  luaupdate);
        
    }
    // Start is called before the first frame update
    void Start()
    {   scripttable.Set("player",GameObject.FindGameObjectWithTag("player"));
        scripttable.Set("enemy",GameObject.FindGameObjectWithTag("enemy"));
        scripttable.Set("enemyposition", GameObject.FindGameObjectWithTag("enemy").transform.position-new Vector3(1,0,0));
        scripttable.Set("playerposition", GameObject.FindGameObjectWithTag("player").transform.position);
        scripttable.Set("attackbutton", AttackButton);
        scripttable.Set("preferb", Preferb);
        scripttable.Set("enemypoint", GameObject.FindGameObjectWithTag("enemypoint").transform);
        scripttable.Set("playerpoint", GameObject.FindGameObjectWithTag("playerpoint").transform);
        if (luastart!=null)
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
