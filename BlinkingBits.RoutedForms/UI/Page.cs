/*
BlinkingBits.RoutedForms: Simple routing support for ASP.NET WebForms

Copyright (c) 2011 Luis Medel / Blining Bits Software (luis@blinkingbits.com)
http://blnkingbits.com/

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software withoutut restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Reflection;

namespace BlinkingBits.RoutedForms.UI
{
    /// <summary>
    /// Base class for routed pages
    /// </summary>
    public class Page: System.Web.UI.Page
    {
        #region Static declarations
        /// <summary>
        /// Maps between page types and the corresponding PageMethodsInfo
        /// </summary>
        private static Dictionary<Type, PageMethodsInfo> PageMapper = new Dictionary<Type, PageMethodsInfo>();
        #endregion


        #region Properties
        /// <summary>
        /// Reuqested page method
        /// </summary>
        public string Method
        {
            get { return (string)Context.Items["RoutedMethod"]; }
        }

        /// <summary>
        /// Routed arguments
        /// </summary>
        public List<string> Arguments
        {
            get { return (List<string>)Context.Items["RoutedArguments"]; }
        }

        /// <summary>
        /// Routed named arguments
        /// </summary>
        public Dictionary<string, string> NamedArguments
        {
            get { return (Dictionary<string, string>)Context.Items["RoutedNamedArguments"]; }
        }
        #endregion


        #region Protected methods
        /// <summary>
        /// Executes a page method and returns a value indicating if the call was possible
        /// </summary>
        /// <param name="name">Method name</param>
        /// <returns></returns>
        protected bool ExecMethod(string name)
        {
            MethodInfo m;
            lock (PageMapper)
            {
                Type t = this.GetType();
                if (!PageMapper.ContainsKey(t))
                    PageMapper[t] = new PageMethodsInfo(t);
                m = PageMapper[t][name];
            }

            if (m == null)
                return false;

            m.Invoke(this, null);
            return true;
        }
        #endregion


        #region Overriden methods
        protected override void OnLoad(EventArgs e)
        {
            string method = Method;
            if (!string.IsNullOrEmpty(method))
                ExecMethod(method);
        }
        #endregion
    }
}
