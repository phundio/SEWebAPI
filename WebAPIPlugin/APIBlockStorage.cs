﻿using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage.Game.Entity;

namespace WebAPIPlugin
{
    public class APIBlockCache
    {
        public static Dictionary<string, long> keyDict;

        public static IMyTextPanel Get(string key)
        {
            if (key == null)
            {
                return null;
            }

            if (keyDict.ContainsKey(key))
            {
                MyEntity entity = null;
                if (MyEntities.TryGetEntityById(keyDict[key], out entity))
                {
                    return entity as IMyTextPanel;
                }
                else
                {
                    keyDict.Remove(key);
                    return null;
                }
            }
            return null;
        }

        public static void Add(IMyTextPanel block)
        {
            string key = "";
            while (keyDict.ContainsKey(key = Generator.GenerateWeakKey(16))) { }
            block.WritePrivateText(key);
            keyDict.Add(key, block.EntityId);
        }

        public static void Save()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in keyDict)
            {
                sb.Append(pair.Key + ',' + pair.Value.ToString() + '\n');
            }

            using (var f = File.OpenWrite("api.cfg"))
            {
                byte[] data = Encoding.ASCII.GetBytes(sb.ToString());
                f.Write(data, 0, data.Length);
            }
        }

        public static void Load()
        {
            keyDict = new Dictionary<string, long>();

            if (File.Exists("api.cfg"))
            {
                FileStream f = File.OpenRead("api.cfg");
                StreamReader sr = new StreamReader(f);
                string data = sr.ReadToEnd();
                string[] lines = data.Split('\n');
                foreach (var line in lines)
                {
                    string[] kv = line.Split(',');
                    if (kv.Length == 2)
                    {
                        keyDict.Add(kv[0], long.Parse(kv[1]));
                    }
                }
                sr.Close();
            }
        }
    }
}