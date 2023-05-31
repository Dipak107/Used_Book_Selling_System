using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Used_Book_Selling_System.Models
{
    public class BookwithOwner
    {
        public String seller_id { get; set; }
        public String title { get; set; }
        public String edittion { get; set; }
        public String author { get; set; }
        public String publisher { get; set; }
        public String language { get; set; }
        public String btype { get; set; }
        public String price { get; set; }
        public byte[] image { get; set; }
        public String old { get; set; }
        public String condition { get; set; }
        public String discount { get; set; }
        public String posted_date { get; set; }
        public String copies { get; set; }
        public String seller_first_name { get; set; }
        public String seller_last_name { get; set; }
        public String seller_city { get; set; }
        public String seller_contact_no { get; set; }
        public String seller_email { get; set; }
        public String islocked { get; set; }
        public String lockedat { get; set; }




    }
}