using System;

namespace MenuComponents.SaveSystem
{
   /// <summary>
   /// Class <c>DataFieldValue</c> Storage for data field value, contains data field name and value.
   /// </summary>
   [Serializable]
   public class DataFieldValue
   {
      public string dataName;
      public string dataValue;

      /// <summary>
      /// Constructor <c>DataFieldValue</c> Constructs the data field value with prams passed.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="value"></param>
      public DataFieldValue(string name, string value)
      {
         dataName = name;
         dataValue = value;
      }
   }
}
