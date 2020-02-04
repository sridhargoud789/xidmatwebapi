using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelDVO
{
    public class ConcessionDvo
    {       
        public List<MenuCategory> MenuList { get; set; }       
    }

    public class MenuCategory
    {
        public string MenuTitle { get; set; }
        public string MenuDescription { get; set; }
        public List<ConcessionItem> MainItems { get; set; }
    }
  
    public class ConcessionItem
    {
        public string PTabs_strTitle { get; set; }

        public string Item_strHeaderTitle { get; set; }

        public string Item_strDescription { get; set; }

        public string Item_strID { get; set; }

        public string Icon_Image_URL { get; set; }

        public string DetailPage_Image_URL { get; set; }

        public string DetailPage_Description { get; set; }

        public string Item_intPrice { get; set; }
    }
}
