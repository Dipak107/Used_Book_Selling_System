using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Used_Book_Selling_System.Models;
using System.IO;

namespace Used_Book_Selling_System.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult addbook()
        {
            tbl__book b = new tbl__book();
            return View(b);
        }
        [HttpPost]
        public ActionResult addbook(HttpPostedFileBase image1, String title, String edition,String author,String publisher,String language,String btype,String price,String old,String condition,String discount,String copies)
        {
            Stream stream = image1.InputStream;
            BinaryReader b = new BinaryReader(stream);
            byte[] bytes = b.ReadBytes((int)stream.Length);
            DataBase dd = new DataBase();
            dd.setimage(bytes, title,edition,author,publisher,language,btype,price,old,condition,discount,copies,Session["user_id"].ToString());


            return RedirectToAction("profile");
        }

        public List<ImageMaster> showimage()
        {
            DataBase dd = new DataBase();
            List<ImageMaster>photos=dd.getimage(Session["user_id"].ToString());
            return photos;
        }

        public ActionResult logout()
        {
            Session["last_name"] = null;

            return View("login");
        }
        
  

        public ActionResult home()
        {
            ViewBag.username = "Guest";
            ViewBag.location = "";
           if(Session["last_name"]!=null)
            {
                ViewBag.username=Session["last_name"].ToString();
                ViewBag.location = Session["city"].ToString();
            }
            return View();
        }
        public ActionResult profile()
        {
            List<ImageMaster> books=null;
            if(Session["last_name"]!=null)
            {
                if (Session["last_name"].ToString()!="Guest")
                {
                    ViewBag.email = Session["email"];
                    ViewBag.first_name = Session["first_name"];
                    ViewBag.last_name = Session["last_name"];
                    ViewBag.contact_no = Session["contact_no"];
                    ViewBag.city = Session["city"];
                    ViewBag.state = Session["state"];
                    ViewBag.country = Session["country"];
                    ViewBag.registered_date = Session["registered_date"];
                    ViewBag.user_type = Session["user_type"];
                    if(ViewBag.user_type=="seller")
                    {
                        books = showimage();
                    }
                }
            }
            
            return View(books);
        }
        public ActionResult editprofile()
        {
            if (Session["last_name"] != null)
            {
                if (Session["last_name"].ToString() != "Guest")
                {
                    ViewBag.email = Session["email"];
                    ViewBag.first_name = Session["first_name"];
                    ViewBag.last_name = Session["last_name"];
                    ViewBag.contact_no = Session["contact_no"];
                    ViewBag.city = Session["city"];
                    ViewBag.state = Session["state"];
                    ViewBag.country = Session["country"];
                    ViewBag.registered_date = Session["registered_date"];
                    ViewBag.user_type = Session["user_type"];
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult updateprofileindb(String email,String first_name,String last_name,String contact_no,String city,String state,String country,String password)
        {
            DataBase dd = new DataBase();
            int c=dd.updateprofile(Session["email"].ToString(),Session["user_type"].ToString(), email, first_name, last_name, contact_no, city, state, country, password);
            Session["email"] = email;
            fetchdata(Session["email"].ToString(), Session["user_type"].ToString());
            return RedirectToAction("profile", "Home");
        }
        
        public ActionResult login()
        {
            
            return View();
        }
        public ActionResult authenticate(String email,String password)
        {
            DataBase dd = new DataBase();
            List<String> userdetails = dd.authenticate( email, password);
            
            if(userdetails.Count>0)
            {
                Session["user_id"] = userdetails[2];
                if(userdetails[1]=="s")
                {
                    Session["user_type"] = "seller";
                }
                if (userdetails[1] == "b")
                {
                    Session["user_type"] =  "buyer";
                }
                fetchdata(Session["user_id"].ToString(),Session["user_type"].ToString());



                return RedirectToAction("home", "Home");
            }
            else
            {
                Session["loginfail"] = "yes";
                return RedirectToAction("login", "Home");
            }
        }
        public void fetchdata(String email, String type)
        {
            DataBase dd = new DataBase();
            List<String> userdetails = dd.fetchdata(Session["user_id"].ToString(), Session["user_type"].ToString());

            if (userdetails.Count > 0)
            {
                Session["email"] = userdetails[0];
                Session["first_name"] = userdetails[1];
                Session["last_name"] = userdetails[2];
                Session["contact_no"] = userdetails[3];
                Session["city"] = userdetails[4];
                Session["state"] = userdetails[5];
                Session["country"] = userdetails[6];
                Session["registered_date"] = userdetails[7];
                if (userdetails[8] == "1")
                {
                    Session["user_type"] = "seller";
                }
                if (userdetails[8] == "2")
                {
                    Session["user_type"] = "buyer";
                }
                Session["user_id"] = userdetails[9];
                
            }
        }
        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register(String first_name,String last_name,String email,String city,String state,String country,String phone ,String type,String password)
        {
            SMTP r = new SMTP();
            String otp=r.sendotp(email,last_name);
            Session["otp"] = otp;
            List<String> data = new List<String>
            {
                first_name,last_name,email,city,state,country,phone,type,password
            };
            Session["data"] = data;
           
            return RedirectToAction("getotp", "Home");
           

        }
        //ask for email to  sent otp then redirect to forgetpassword then resetpass
        public ActionResult forget()
        {
            
            
            return View();
        }
        public ActionResult forgetpassword(String email)
        {
            if (Session["onlyview"]!=null)
            {
                if(Session["onlyview"].ToString()=="yes")
                {
                    Session["onlyview"] = null;
                    return View();
                }
                
            }
            DataBase dd = new DataBase();
            String user_id=dd.userexist(email);
            if(user_id!=null)
            {
                SMTP s = new SMTP();
               String otp= s.sendotp(email, user_id);
                Session["resetemail"] = email;
            
                Session["resetotp"] = otp;
                return View();
            }
            else
            {
                
                Session["usernotexist"] = "yes";
                return RedirectToAction("forget", "home");
            }
        }
        
        
        
        public ActionResult addimage()
        {
            return View();
        }
      
        public ActionResult getotp()
        {

            return View();
        }
        [HttpPost]
        public ActionResult verifyopt(String otp)
        {
            
                if (Session["otp"].ToString() == otp)
                {
                    List<String> name = new List<string>();
                    foreach (String d in (List<String>)Session["data"])
                    {
                        name.Add(d);

                    }

                    DataBase data = new DataBase();
                    data.adduser(name[0], name[1], name[2], name[3], name[4], name[5], name[6], name[7], name[8]);
                    Session["registersuccess"] = "yes";
                    return RedirectToAction("login", "Home");


                }
                else
                {
                    
                    Session["invalidotp"] = "yes";
                    return RedirectToAction("getotp", "Home");
                }

            
           
            

        }
        [HttpPost]
        public ActionResult resetpass(String otp,String password)
        {

            if (Session["resetotp"].ToString() == otp)
            {
                DataBase dd = new DataBase();
                int c = dd.resetpass(Session["resetemail"].ToString(), password);
                Session["resetsuccess"] = "yes";
                return RedirectToAction("login", "Home");
            }
            else
            {
                Session["onlyview"] = "yes";
                Session["resetinvalidotp"] = "yes";
                return RedirectToAction("forgetpassword", "Home");
            }
        }

        public ActionResult about()
        {
            

            return View();
        }

        public ActionResult contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult bookmodification(String btn, String title, String edition, String author, String publisher, String language, String btype, String price, String old, String condition, String discount, String copies)
        {
            List<String> bookdetails = new List<string>
            {
                title,edition,author,publisher,language,btype,price,old,condition,discount,copies

            };
            DataBase dd = new DataBase();

             if (btn.Equals("Delete"))
            {

                dd.deletebook(Session["user_id"].ToString(), title, author);
                return RedirectToAction("profile");
            }
             else
            {
                return View(bookdetails);

            }

        }
        public ActionResult updatebook(String title, String edition, String author, String publisher, String language, String btype, String price, String old, String condition, String discount, String copies,String originaltitle,String originalauthor)
        {
            DataBase dd = new DataBase();
            dd.updatebook(Session["user_id"].ToString(), originaltitle, originalauthor, title, edition, author, publisher, language, btype, price, old, condition, discount, copies);
            return RedirectToAction("profile");
        }
        [HttpPost]
        public ActionResult soldbook(String title,String author,String quantity)
        {
            DataBase dd = new DataBase();
            dd.soldbook(title, author,quantity, Session["user_id"].ToString());
            return RedirectToAction("showorder");
        }
        public ActionResult showorder()
        {
            DataBase dd = new DataBase();
            
            List<BookwithOwner> books = dd.getmyorder(Session["user_id"].ToString());
            return View(books);
        }

        public ActionResult showlockedbook()
        {
            DataBase dd = new DataBase();
            List<BookwithOwner> books=dd.getmylockedbook(Session["user_id"].ToString());
            return View(books);
        }
        public ActionResult unlock(String title,String author,String seller_id,String buyer_id)
        {
            DataBase dd = new DataBase();
            dd.unlock(title, author, seller_id,buyer_id);
            return RedirectToAction("showlockedbook");
        }
        public ActionResult portfolio(String searchby ,String search)
        {
            String city="surat";
            DataBase dd = new DataBase();
            
                if (Session["city"]!=null)
              {
                city = Session["city"].ToString();
            
               }
            
            List<BookwithOwner> booklist= dd.getbooks(city,searchby,search);
            Session["booklist"] = booklist;

            return View(booklist);
        }
      [HttpPost]
        public ActionResult portfolio_details(String seller_id,String seller_email,String btn ,String title,String edition,String author,String publisher,String language,String btype,String price ,String old,String condition,String posted_date,String copies,String seller_first_name,String seller_last_name,String seller_city,String seller_contact_no)
        {
           
            if(btn.Equals("Lock"))
            {
                if (Session["last_name"] != null)
                {
                    if(seller_contact_no.Equals(Session["contact_no"].ToString()))
                    {
                        Session["sameseller"] = "yes";
                        return RedirectToAction("profile");
                    }
                    else
                    {
                        DataBase dd = new DataBase();
                        dd.lockbook(seller_id, title, author, Session["user_id"].ToString(), Session["user_type"].ToString());
                        SMTP s = new SMTP();
                        s.notify(title, author, Session["contact_no"].ToString(), seller_email, Session["email"].ToString(),price,seller_contact_no);
                        return RedirectToAction("showlockedbook");
                    }
                   
                }
                else
                {
                    return RedirectToAction("login");

                }

            }
              
            else
            {
                List<BookwithOwner> booklist = (List<BookwithOwner>)Session["booklist"];
                BookwithOwner selectedbook = null;
                foreach (BookwithOwner book in booklist)
                {
                    if (book.title.Equals(title))
                    {
                        selectedbook = book;
                        break;
                    }
                }


                return View(selectedbook);

            }
           

        }

        



    }
}