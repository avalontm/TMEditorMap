using System;
using System.Collections.Generic;
using System.Text;
using TMFormat.Formats;

namespace TMEditorMap.Models
{
    public class GroupSprites
    {
        public List<TMSprite> Items { set; get; }

        public GroupSprites()
        {
            Items = new List<TMSprite>();
        }
    }

}
