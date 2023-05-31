using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace Used_Book_Selling_System.Models
{
    public class SMTP
    {
        public void notify(String title,String author,String user_contat,String seller_email,String user_email,String price,String seller_contact)
        {
            MailMessage msg = new MailMessage("rajputdevil100@gmail.com", seller_email);
            msg.Subject = "Book Locked  Notification From Used Book Selling System";
            msg.Body = "Hello  your Book " + title +"(bye:"+author+")"+ " is being locked by intrested user ...now you can contact him/her to proceed deal\n Buyer contact Number: "+user_contat+"\n Buyer contact emailid: "+user_email+"\n Thank you Have a nice day";

            SmtpClient s = new SmtpClient("smtp.gmail.com", 587);
            s.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rajputdevil100@gmail.com",
                Password = "9824048355@Ds"

            };


            s.EnableSsl = true;
            
            s.Send(msg);



            msg = new MailMessage("rajputdevil100@gmail.com", user_email);
            msg.Subject = "Book Locked  Notification From Used Book Selling System";
            msg.Body = "you have locked Book: " + title + "(bye:" + author + ") for 24 hours" + "\n price:" + price + "\nplease contact your seller to proceed deal. or login to your profile to cancel it \n seller_contact_number: " + seller_contact + "\n seller emailid :" + seller_email+"\n Thank you Have a nice day"; 

            
            s.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rajputdevil100@gmail.com",
                Password = "9824048355@Ds"

            };


            s.EnableSsl = true;
            s.Send(msg);
        }
        public String sendotp(String email,String lastname)
        {
            Random rand = new Random();
            String value = "";
            for (int i = 0; i <= 4; i++)
            {
                 value += rand.Next(1, 5).ToString();
            }
            MailMessage msg = new MailMessage("rajputdevil100@gmail.com", email);
            msg.Subject = "Email Verification From Used Book Selling System";
            msg.Body = "Hello  your OTP is "+value;

            SmtpClient s = new SmtpClient("smtp.gmail.com", 587);
            s.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rajputdevil100@gmail.com",
                Password = "9824048355@Ds"

            };
           
            
            s.EnableSsl = true;
            s.Send(msg);
            return value;
        }
    }
}