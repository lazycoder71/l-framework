﻿using UnityEngine;

namespace LFramework
{
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        public static string s_rootFolderName { get { return "ScriptableObjectSingletons"; } }

        static T s_instance = null;

        public static T instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = Resources.Load<T>(string.Format("{0}/{1}", s_rootFolderName, typeof(T)));

#if UNITY_EDITOR
                    if (s_instance == null)
                    {
                        string configPath = string.Format("Assets/Resources/{0}/", s_rootFolderName);

                        if (!System.IO.Directory.Exists(configPath))
                            System.IO.Directory.CreateDirectory(configPath);

                        s_instance = ScriptableObjectHelper.CreateAsset<T>(configPath, typeof(T).ToString());
                    }
#endif
                }

                return s_instance;
            }
        }
    }
}