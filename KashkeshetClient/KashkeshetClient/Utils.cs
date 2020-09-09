using KashkeshetCommon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient
{
    public class Utils
    {
        public static string SerlizeObject(object obj)
        {
            string jsonString = JsonConvert.SerializeObject(obj);
            return jsonString;


        }

        public static T DeSerlizeObject<T>(string str)
        {
            var obj = JsonConvert.DeserializeObject<T>(str);
            return obj;


        }
    }
}
