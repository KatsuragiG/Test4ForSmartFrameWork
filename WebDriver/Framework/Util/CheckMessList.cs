using System;
using System.Collections.Generic;

namespace WebdriverFramework.Framework.Util
{
    /// <summary>
    /// list of the messages. simple wrapped over List
    /// </summary>
    public class CheckMessList : List<String>
    {
        /// <summary>
        /// add message to List and set HasWarn to true
        /// </summary>
        /// <param name="message">message with reason of the warn</param>
        public new void Add(String message)
        {
            base.Add(message);
        }
    }
}