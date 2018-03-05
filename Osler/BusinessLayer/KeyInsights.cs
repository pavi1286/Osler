using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Osler.BusinessLayer
{
    [Serializable]
    public class KeyInsights
    {
        public KeyInsights()
        {
        }

        public string Key { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string PictureUrl { get; set; }
        public string TapUrl { get; set; }
        public string Description { get; set; }
    }
}