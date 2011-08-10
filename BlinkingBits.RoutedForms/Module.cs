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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

using BlinkingBits.RoutedForms.Configuration;

namespace BlinkingBits.RoutedForms
{
    class Module: IHttpModule
    {
        private UrlRoutingConfig _config = null;
        
        public void Init (HttpApplication app)
        {
            app.BeginRequest += new EventHandler(app_BeginRequest);
            _config = (UrlRoutingConfig)HttpContext.Current.GetSection("urlRouting");
        }


        void app_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            string url = context.Request.AppRelativeCurrentExecutionFilePath;

            string extension = Path.GetExtension(url);
            if (extension == "axd")
                return;

            if (_config.IgnoreExisting && File.Exists(context.Request.PhysicalPath))
                return;

            if (_config.AppendSlash && (!url.EndsWith("/")) && (Path.GetExtension (url).Length == 0))
                url += "/";

            /* First we test for ignore patterns */
            foreach (RoutingItem r in _config.IgnoreItems)
            {
                if (r.Regex.IsMatch(url))
                    return;
            }

            /* The real work here */
            foreach (RoutingItem r in _config.Items)
            {
                if (!r.Regex.IsMatch(url))
                    continue;

                Match m = r.Regex.Match(url);

                List<string> arguments = new List<string>();
                Dictionary<string, string> namedArguments = new Dictionary<string, string>();

                for (int i = 1; i < m.Groups.Count; i++)
                {
                    Group g = m.Groups[i];
                    arguments.Add(g.Value);
                    namedArguments[r.Regex.GroupNameFromNumber(i)] = g.Value;
                }
                context.Items["RoutedArguments"] = arguments;
                context.Items["RoutedNamedArguments"] = namedArguments;

                if (string.IsNullOrEmpty(r.Method))
                    context.Items["RoutedMethod"] = string.Empty;
                else
                    context.Items["RoutedMethod"] = r.Regex.Replace(url, r.Method);

                string newUrl = r.Regex.Replace(url, r.Url);
                context.RewritePath(newUrl, false);
                return;
            }
        }

        public void Dispose ()
        {
            // Nothing to see here
        }
    }
}
