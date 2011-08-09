﻿/*
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

using System.Text.RegularExpressions;
using System.Xml;

namespace BlinkingBits.RoutedForms.Configuration
{
    /// <summary>
    /// Reader for urlRouting section in web.config
    /// </summary>
    class UrlRoutingSection : System.Configuration.IConfigurationSectionHandler
    {
        private Regex RegexSegment = new Regex (@"\{([^/]+)\}", RegexOptions.Compiled);
        private Regex RegexEscaped = new Regex(@"([.-+?])", RegexOptions.Compiled);

        public RoutingItemCollection Items { get; private set; }

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            if (Items != null)
                return Items;

            Items = new RoutingItemCollection ();

            foreach (XmlNode node in section.SelectNodes("add"))
            {
                string pattern = node.Attributes["pattern"].Value;

                if ((node.Attributes["type"] == null) || (node.Attributes["type"].Value != "regex"))
                {
                    pattern = RegexEscaped.Replace(pattern, @"\$1").Replace("*", ".*");
                    if (!pattern.StartsWith("^")) pattern = "^" + pattern;
                    if (!pattern.EndsWith("$")) pattern = pattern + "$";
                }

                pattern = RegexSegment.Replace(pattern, @"(?<$1>[^/]+)");

                Items.Add(new RoutingItem {
                    Regex = new Regex(pattern, RegexOptions.Compiled),
                    Url = node.Attributes["url"].Value,
                    Method = (node.Attributes["method"] != null) ? node.Attributes["method"].Value : string.Empty
                });
            }

            return Items;
        }
    }
}
