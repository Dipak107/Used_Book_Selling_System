using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Used_Book_Selling_System.Models
{
    public class DataBase
    {


        String connectionstring = "Data Source=.;Initial Catalog=Book;Integrated Security=True";

        public void lockbook(String seller_id, String title, String author, String user_id, String user_type)
        {
            char user_typee = user_type[0];
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("spLock", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pseller_id = new SqlParameter()
                {
                    ParameterName = "@seller_id",
                    Value = Convert.ToInt64(seller_id)
                };
                cmd.Parameters.Add(pseller_id);
                SqlParameter ptitle = new SqlParameter()
                {
                    ParameterName = "@title",
                    Value = title
                };
                cmd.Parameters.Add(ptitle);

                SqlParameter pauthor = new SqlParameter()
                {
                    ParameterName = "@author",
                    Value = author
                };
                cmd.Parameters.Add(pauthor);

                SqlParameter puser_id = new SqlParameter()
                {
                    ParameterName = "@user_id",
                    Value = Convert.ToInt64(user_id)
                };
                cmd.Parameters.Add(puser_id);


                SqlParameter puser_type = new SqlParameter()
                {
                    ParameterName = "@user_type",
                    Value = user_typee
                };
                cmd.Parameters.Add(puser_type);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
        }
        public void soldbook(String title, String author,String q, String seller_id)
        {
            SqlConnection con = new SqlConnection(connectionstring);
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update tbl__book set islocked=0,copies=copies-@c where seller_id=@seller_id and title=@title and author=@author ";
                cmd.Parameters.AddWithValue("@c", Convert.ToInt64(q) );
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@author", author);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Open();
                cmd.CommandText = "delete from tbl_lock where seller_id=@seller_id and book_title=@title and book_author=@author";
                cmd.ExecuteNonQuery();
                con.Close();

            }
        }
        public List<BookwithOwner> getmyorder(String seller_id)
        {
            List<BookwithOwner> books = new List<BookwithOwner>();
            SqlConnection con = new SqlConnection(connectionstring);
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from tbl_lock,tbl__book,tbl__buyer where tbl_lock.seller_id=@seller_id and tbl_lock.book_title=tbl__book.title and tbl_lock.book_author=tbl__book.author and tbl__buyer.buyer_id=tbl_lock.buyer_id";
                cmd.Parameters.AddWithValue("@seller_id",seller_id );
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    BookwithOwner bo = new BookwithOwner();
                    bo.title = r["title"].ToString();
                    bo.publisher = r["publisher"].ToString();
                    bo.price = r["price"].ToString();
                    bo.posted_date = r["posted_date"].ToString();
                    bo.old = r["old"].ToString();
                    bo.language = r["language"].ToString();
                    bo.image = (byte[])r["image"];
                    bo.edittion = r["edition"].ToString();
                    bo.discount = r["discount"].ToString();
                    bo.copies = r["copies"].ToString();
                    bo.condition = r["book_condition"].ToString();
                    bo.btype = r["book_type"].ToString();
                    bo.author = r["author"].ToString();
                  
                    bo.seller_first_name = r["first_name"].ToString();
                    bo.seller_last_name = r["last_name"].ToString();
                    bo.seller_contact_no = r["contact_no"].ToString();
                    bo.lockedat = r["locked_time"].ToString();

                   
                    books.Add(bo);



                }


            }
            return books;


        }
        public List<BookwithOwner> getmylockedbook(String buyer_id)
        {
            List<BookwithOwner> books = new List<BookwithOwner>();
            SqlConnection con = new SqlConnection(connectionstring);
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select* from tbl__book,tbl_lock,tbl__seller where tbl_lock.buyer_id = @buyer_id and tbl_lock.seller_id = tbl__book.seller_id and tbl_lock.book_title = tbl__book.title and tbl_lock.book_author = tbl__book.author and tbl_lock.seller_id = tbl__seller.seller_id" ;
                cmd.Parameters.AddWithValue("@buyer_id", buyer_id);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    BookwithOwner bo = new BookwithOwner();
                    bo.title = r["title"].ToString();
                    bo.publisher = r["publisher"].ToString();
                    bo.price = r["price"].ToString();
                    bo.posted_date = r["posted_date"].ToString();
                    bo.old = r["old"].ToString();
                    bo.language = r["language"].ToString();
                    bo.image = (byte[])r["image"];
                    bo.edittion = r["edition"].ToString();
                    bo.discount = r["discount"].ToString();
                    bo.copies = r["copies"].ToString();
                    bo.condition = r["book_condition"].ToString();
                    bo.btype = r["book_type"].ToString();
                    bo.author = r["author"].ToString();
                    bo.seller_id = r["seller_id"].ToString();
                    bo.seller_first_name = r["first_name"].ToString();
                    bo.seller_last_name = r["last_name"].ToString();
                    bo.seller_contact_no = r["contact_no"].ToString();
                    bo.lockedat = r["locked_time"].ToString();
                   
                    bo.seller_id = r["seller_id"].ToString();
                  
                    bo.islocked = r["islocked"].ToString();
                    books.Add(bo);



                }


            }
            return books;

        }
        public List<BookwithOwner> getbooks(String city,String searchby,String search)
        {
            List<BookwithOwner> book = new List<BookwithOwner>();
            SqlConnection con = new SqlConnection(connectionstring);
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                if (search == null)
                {
                    cmd.CommandText = "select * from tbl__seller,tbl__book,tbl__address,tbl_login where tbl_login.user_id=tbl__seller.seller_id and tbl__seller.seller_id=tbl__book.seller_id and tbl__seller.seller_id=tbl__address.user_id  and tbl__address.city=@city and tbl__address.user_type=1 and tbl__book.copies>0";
                }
                else
                {
                    if (searchby.Equals("1")) { cmd.CommandText = "select * from tbl__seller,tbl__book,tbl__address,tbl_login where tbl_login.user_id=tbl__seller.seller_id and tbl__seller.seller_id=tbl__book.seller_id and tbl__seller.seller_id=tbl__address.user_id  and tbl__address.city=@city and tbl__address.user_type=1 and tbl__book.copies>0 and tbl__book.title like '%'+@search+'%'"; }
                    if (searchby.Equals("2")) { cmd.CommandText = "select * from tbl__seller,tbl__book,tbl__address,tbl_login where tbl_login.user_id=tbl__seller.seller_id and tbl__seller.seller_id=tbl__book.seller_id and tbl__seller.seller_id=tbl__address.user_id  and tbl__address.city=@city and tbl__address.user_type=1 and tbl__book.copies>0 and tbl__book.author like '%'+@search+'%'"; }
                    if(searchby.Equals("3")) { cmd.CommandText = "select * from tbl__seller,tbl__book,tbl__address,tbl_login where tbl_login.user_id=tbl__seller.seller_id and tbl__seller.seller_id=tbl__book.seller_id and tbl__seller.seller_id=tbl__address.user_id  and tbl__address.city=@city and tbl__address.user_type=1 and tbl__book.copies>0 and tbl__book.publisher like '%'+@search+'%'"; }
                    
                    cmd.Parameters.AddWithValue("@search", search);
                }
                cmd.Parameters.AddWithValue("@city", city);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while(r.Read())
                {
                    BookwithOwner bo = new BookwithOwner();
                    bo.title = r["title"].ToString();
                    bo.publisher = r["publisher"].ToString();
                    bo.price = r["price"].ToString();
                    bo.posted_date = r["posted_date"].ToString();
                    bo.old = r["old"].ToString();
                    bo.language = r["language"].ToString();
                    bo.image = (byte[])r["image"];
                    bo.edittion = r["edition"].ToString();
                    bo.discount = r["discount"].ToString();
                    bo.copies = r["copies"].ToString();
                    bo.condition = r["book_condition"].ToString();
                    bo.btype = r["book_type"].ToString();
                    bo.author = r["author"].ToString();
                    bo.seller_id = r["seller_id"].ToString();
                    bo.seller_first_name = r["first_name"].ToString();
                    bo.seller_last_name = r["last_name"].ToString();
                    bo.seller_contact_no = r["contact_no"].ToString();
                    bo.seller_city = r["city"].ToString();
                    bo.seller_id = r["seller_id"].ToString();
                    bo.seller_email = r["email"].ToString();
                    bo.islocked = r["islocked"].ToString();
                    book.Add(bo);
                    


                }
                

            }
            return book;
        }
        public void deletebook(String seller_id, String title, String author)
        {
            SqlConnection con = new SqlConnection(connectionstring);
            using (SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from tbl__book where seller_id=@seller_id and title=@title and author=@author";
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@seller_id", Convert.ToInt64(seller_id));
                cmd.Parameters.AddWithValue("@author", author);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void unlock(String title,String author,String seller_id,String buyer_id)
        {
            SqlConnection con = new SqlConnection(connectionstring);
            using (SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from tbl_lock where seller_id=@seller_id and book_title=@title and book_author=@author and buyer_id=@buyer_id";
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@seller_id", Convert.ToInt64(seller_id));
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@buyer_id", Convert.ToInt64(buyer_id));
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                cmd.CommandText = "update tbl__book set islocked=0 where seller_id=@seller_id and title=@title and author=@author";
               

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    
        public void updatebook(String seller_id,String orignaltitle,String orignalauthor,String title, String edition, String author, String publisher, String language, String btype, String price, String old, String condition, String discount, String copies)
        {
            SqlConnection con = new SqlConnection(connectionstring);
            using (SqlCommand cmd = new SqlCommand())
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update tbl__book set title=@title,edition=@edition,author=@author,publisher=@publisher,language=@language,book_type=@book_type,price=@price,old=@old,book_condition=@book_condition,discount=@discount,copies=@copies where seller_id=@seller_id and title=@orignaltitle and author=@orignalauthor";
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@edition",Convert.ToInt64(edition));
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@publisher", publisher);
                cmd.Parameters.AddWithValue("@language", language);
                cmd.Parameters.AddWithValue("@book_type", btype);
                cmd.Parameters.AddWithValue("@price",Convert.ToDouble(price));
                cmd.Parameters.AddWithValue("@old",Convert.ToDouble(old));
                cmd.Parameters.AddWithValue("@book_condition", condition);
                cmd.Parameters.AddWithValue("@discount",Convert.ToDouble(discount));
                cmd.Parameters.AddWithValue("@copies", Convert.ToInt64(copies));

                cmd.Parameters.AddWithValue("@seller_id",Convert.ToInt64(seller_id));
                cmd.Parameters.AddWithValue("@orignaltitle", orignaltitle);
                cmd.Parameters.AddWithValue("@orignalauthor", orignalauthor);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
        }
        public void setimage(byte[] image,String title ,String edition,String author,String publisher,String language,String btype,String price,String old,String condition,String discount,String copies,String seller_id)
        {
            using(SqlConnection con=new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("insertimage",con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter ptitle = new SqlParameter()
                {
                    ParameterName="@title",
                    Value=title
                };
                cmd.Parameters.Add(ptitle);
                SqlParameter pimage = new SqlParameter()
                {
                    ParameterName = "@image",
                    Value = image
                };
                cmd.Parameters.Add(pimage);

                SqlParameter e = new SqlParameter()
                {
                    ParameterName = "@edition",
                    Value = Convert.ToInt64(edition)
                };
                cmd.Parameters.Add(e);

                SqlParameter pauthor = new SqlParameter()
                {
                    ParameterName = "@author",
                    Value = author
                };
                cmd.Parameters.Add(pauthor);

                SqlParameter ppublisher = new SqlParameter()
                {
                    ParameterName = "@publisher",
                    Value = publisher
                };
                cmd.Parameters.Add(ppublisher);

                SqlParameter planguage = new SqlParameter()
                {
                    ParameterName = "@language",
                    Value = language
                };
                cmd.Parameters.Add(planguage);

                SqlParameter pbtype = new SqlParameter()
                {
                    ParameterName = "@btype",
                    Value = btype
                };
                cmd.Parameters.Add(pbtype);

                SqlParameter pprice = new SqlParameter()
                {
                    ParameterName = "@price",
                    Value = Convert.ToDouble(price)
                };
                cmd.Parameters.Add(pprice);

                SqlParameter pold = new SqlParameter()
                {
                    ParameterName = "@old",
                    Value = Convert.ToInt64(old)
                };
                cmd.Parameters.Add(pold);

                SqlParameter pcondition = new SqlParameter()
                {
                    ParameterName = "@condition",
                    Value = condition
                };
                cmd.Parameters.Add(pcondition);

                SqlParameter pdiscount = new SqlParameter()
                {
                    ParameterName = "@discount",
                    Value = Convert.ToDouble(discount)
                };
                cmd.Parameters.Add(pdiscount);

                SqlParameter pcopies = new SqlParameter()
                {
                    ParameterName = "@copies",
                    Value = Convert.ToInt64(copies)
                };
                cmd.Parameters.Add(pcopies);


                SqlParameter psellerid = new SqlParameter()
                {
                    ParameterName = "@seller_id",
                    Value = Convert.ToInt64(seller_id)
                };
                cmd.Parameters.Add(psellerid);


                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<ImageMaster> getimage(String seller_id)
        {
            SqlConnection con = new SqlConnection(connectionstring);
            List<ImageMaster> photogallery = new List<ImageMaster>();
            using(SqlCommand cmd=new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from tbl__book where seller_id="+Convert.ToInt64(seller_id)+"";
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while(r.Read())
                {
                    ImageMaster objImageMaster = new ImageMaster();
                    objImageMaster.seller_id = r["seller_id"].ToString();
                    objImageMaster.image = (byte[])r["image"];
                    objImageMaster.title = r["title"].ToString();
                    objImageMaster.edittion = r["edition"].ToString();
                    objImageMaster.author = r["author"].ToString();
                    objImageMaster.publisher = r["publisher"].ToString();
                    objImageMaster.language = r["language"].ToString();
                    objImageMaster.btype = r["book_type"].ToString();
                    objImageMaster.price = r["price"].ToString();
                    objImageMaster.old = r["old"].ToString();
                    objImageMaster.condition = r["book_condition"].ToString();
                    objImageMaster.discount = r["discount"].ToString();
                    objImageMaster.posted_date = r["posted_date"].ToString();
                    objImageMaster.copies = r["copies"].ToString();
                    photogallery.Add(objImageMaster);
                }
                
                r.Close();
                return photogallery.ToList();
            }
        }

        public void adduser(String first_name, String last_name, String email, String city, String state, String country, String phone, String type, String password)
        {
            

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("spInsertUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter firstname = new SqlParameter();
                firstname.ParameterName = "@first_name";
                firstname.Value = first_name;
                cmd.Parameters.Add(firstname);

                SqlParameter lastname = new SqlParameter();
                lastname.ParameterName = "@last_name";
                lastname.Value = last_name;
                cmd.Parameters.Add(lastname);

                SqlParameter pemail = new SqlParameter();
                pemail.ParameterName = "@email";
                pemail.Value = email;
                cmd.Parameters.Add(pemail);

                SqlParameter pcity = new SqlParameter();
                pcity.ParameterName = "@city";
                pcity.Value = city;
                cmd.Parameters.Add(pcity);


                SqlParameter pstate = new SqlParameter();
                pstate.ParameterName = "@state";
                pstate.Value = state;
                cmd.Parameters.Add(pstate);


                SqlParameter pcountry = new SqlParameter();
                pcountry.ParameterName = "@country";
                pcountry.Value = country;
                cmd.Parameters.Add(pcountry);

                SqlParameter pphone = new SqlParameter();
                pphone.ParameterName = "@contact_no";
                pphone.Value = phone;
                cmd.Parameters.Add(pphone);

                SqlParameter ptype = new SqlParameter();
                ptype.ParameterName = "@usertype";
                ptype.Value = type;
                cmd.Parameters.Add(ptype);


                SqlParameter ppassword = new SqlParameter();
                ppassword.ParameterName = "@password";
                ppassword.Value = password;
                cmd.Parameters.Add(ppassword);



                con.Open();
                cmd.ExecuteNonQuery();



            }
        }
        public String userexist(String email)
        {
            String user = null;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("spUserExist", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pemail = new SqlParameter();
                pemail.ParameterName = "@email";
                pemail.Value = email;
                cmd.Parameters.Add(pemail);

               

                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    user = Convert.ToString(r["user_id"]);
                }
                con.Close();

            }
            return user;
        }

        public int resetpass(String email, String password)
        {

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("spResetPass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pemail = new SqlParameter();
                pemail.ParameterName = "@email";
                pemail.Value = email;
                cmd.Parameters.Add(pemail);


                SqlParameter ppassword = new SqlParameter();
                ppassword.ParameterName = "@password";
                ppassword.Value = password;
                cmd.Parameters.Add(ppassword);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return 1;

            }
            return 0;

        }


        public List<String> authenticate( String email, String password)
        {
            List<String> userdetail = new List<String>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("spLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pemail = new SqlParameter();
                pemail.ParameterName = "@email";
                pemail.Value = email;
                cmd.Parameters.Add(pemail);

                

                SqlParameter ppassword = new SqlParameter();
                ppassword.ParameterName = "@password";
                ppassword.Value = password;
                cmd.Parameters.Add(ppassword);

                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                   
                    userdetail.Add((String)r["email"]);
                    userdetail.Add((String)r["user_type"]);
                    userdetail.Add(Convert.ToString(r["user_id"]));




                }

                con.Close();
            }
            return userdetail;
        }

        public int updateprofile(String originalemail,String type,String email, String first_name, String last_name, String contact_no, String city, String state, String country, String password)
        {
            char typee = type[0];

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("spUpdateProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter firstname = new SqlParameter();
                firstname.ParameterName = "@first_name";
                firstname.Value = first_name;
                cmd.Parameters.Add(firstname);

                SqlParameter lastname = new SqlParameter();
                lastname.ParameterName = "@last_name";
                lastname.Value = last_name;
                cmd.Parameters.Add(lastname);

                SqlParameter pemail = new SqlParameter();
                pemail.ParameterName = "@email";
                pemail.Value = email;
                cmd.Parameters.Add(pemail);

                SqlParameter pcity = new SqlParameter();
                pcity.ParameterName = "@city";
                pcity.Value = city;
                cmd.Parameters.Add(pcity);


                SqlParameter pstate = new SqlParameter();
                pstate.ParameterName = "@state";
                pstate.Value = state;
                cmd.Parameters.Add(pstate);


                SqlParameter pcountry = new SqlParameter();
                pcountry.ParameterName = "@country";
                pcountry.Value = country;
                cmd.Parameters.Add(pcountry);

                SqlParameter pphone = new SqlParameter();
                pphone.ParameterName = "@contact_no";
                pphone.Value = contact_no;
                cmd.Parameters.Add(pphone);

                SqlParameter ptype = new SqlParameter();
                ptype.ParameterName = "@usertype";
                ptype.Value = type;
                cmd.Parameters.Add(ptype);


                SqlParameter ppassword = new SqlParameter();
                ppassword.ParameterName = "@password";
                ppassword.Value = password;
                cmd.Parameters.Add(ppassword);

                SqlParameter poriginalmail = new SqlParameter();
                poriginalmail.ParameterName = "@originalemail";
                poriginalmail.Value = originalemail;
                cmd.Parameters.Add(poriginalmail);



                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return 1;



            }

        }
        public List<String> fetchdata(String user_id,String type)
        {
            List<String> userdetail = new List<String>();

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("spFetchData", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pemail = new SqlParameter();
                pemail.ParameterName = "@user_id";
                pemail.Value = user_id;
                cmd.Parameters.Add(pemail);

                SqlParameter ptype = new SqlParameter();
                ptype.ParameterName = "@type";
                ptype.Value = type;
                cmd.Parameters.Add(ptype);

               

                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                   
                    userdetail.Add((String)r["email"]);
                    userdetail.Add((String)r["first_name"]);
                    userdetail.Add((String)r["last_name"]);
                    userdetail.Add((String)r["contact_no"]);
                    userdetail.Add((String)r["city"]);
                    userdetail.Add((String)r["state"]);
                    userdetail.Add((String)r["country"]);
                    userdetail.Add(Convert.ToString(r["registered_date"]));
                    userdetail.Add((String)r["user_type"]);
                    userdetail.Add(Convert.ToString(r[0]));

                }

                con.Close();
            }
            return userdetail;
        }
    }
}