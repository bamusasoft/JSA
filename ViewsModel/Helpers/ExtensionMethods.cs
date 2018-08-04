using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Jsa.DomainModel;
using Jsa.ViewsModel.DomainEntities;

namespace Jsa.ViewsModel.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsDigit(this string str)
        {
            foreach (var chr in str.ToCharArray())
            {
                int tempTest;
                if (! int.TryParse(chr.ToString(), out tempTest))
                {
                    return false;
                }
               
            }
            return true;
        }
        public static T DeepClone<T>(this T a) where T : class
        {
            if (a == null) throw new ArgumentNullException("a");
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
        
       
    }
}
