using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlinkingBits.RoutedForms.Configuration
{
    class UrlRoutingConfig
    {
        public bool IgnoreExisting { get; set; }
        public bool AppendSlash { get; set; }

        public RoutingItemCollection Items { get; private set; }
        public RoutingItemCollection IgnoreItems { get; private set; }

        public UrlRoutingConfig()
        {
            Items = new RoutingItemCollection();
            IgnoreItems = new RoutingItemCollection();
        }
    }
}
