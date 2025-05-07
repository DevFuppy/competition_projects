using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace EFcoreRepoPractice.Application
{
    public static class EmailSender
    {

        public static void BuildMail(string Email, string subject, string bodyHtml)
        {
            var mail = new MimeMessage();

            BodyBuilder bb = new BodyBuilder();
            bb.HtmlBody = bodyHtml;
            var MessageBody = bb.ToMessageBody();

            mail.From.Add(new MailboxAddress("EFcore框架","suspicious@gmail.com"));
            mail.To.Add(MailboxAddress.Parse(Email));
            mail.Subject= subject;
            mail.Body= MessageBody;

            using (SmtpClient smtp = new SmtpClient()) {

                smtp.Connect("smtp.gmail.com",587,SecureSocketOptions.StartTls);
                smtp.Authenticate("iiispan.jojo@gmail.com", "smicaztagudgpvwk");
                smtp.Send(mail);
                smtp.Dispose();
                 
            }

        }



        public static void SendMail(string Email, string url)
        {
             
            string subject = "測試信來瞜~";
            string body =
                @$" <h1>這是什麼???</h1>
                     <div>
                         <p>屬於你的連結 >////< </p>
                         <a href={url}></a>   </div>";


            BuildMail(Email, subject, body);
        
        }






    }
}
