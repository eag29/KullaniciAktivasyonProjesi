using MailKit.Net.Smtp;
using Microsoft.Win32;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace KullaniciAktivasyonProjesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*
         * Genel Açıklama:
         Bu kod, bir kullanıcı kayıt formunda çalışıyor (btnregister_Click olayı).
         Kullanıcı formu doldurup kayıt butonuna bastığında şunlar oluyor:

         Kullanıcı bilgileri veritabanına kaydediliyor.

         Kullanıcının email adresine bir onay kodu gönderiliyor.

         Email onayı için başka bir form açılıyor (FrmMailConfirm).
         */

        KullaniciAktivasyonVtEntities context = new KullaniciAktivasyonVtEntities();

        private void btnregister_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int confirmCode = random.Next(100000, 1000000);

            #region Kullanıcı bilgileri veritabanına kaydediliyor.

            Tbl_User user = new Tbl_User();
            user.KullaniciAdi = txtKullaniciAdi.Text;
            user.Email = txtEmail.Text;
            user.Sifre = txtsifre.Text;
            user.ConfirmCode = confirmCode.ToString();
            user.isConfirm = false;

            context.Tbl_User.Add(user);
            context.SaveChanges();
            MessageBox.Show("Kullanıcı Eklendi", "Ekleme İşleminin Tamamlanması için Mail Adresinize Gelen Kodu Giriniz", MessageBoxButtons.OK, MessageBoxIcon.Information);

            #endregion

            #region Kullanıcının email adresine bir onay kodu gönderiliyor.

            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress mailboxAddressfrom = new MailboxAddress("AdminRegister", "emirali19078@gmail.com");
            mimeMessage.From.Add(mailboxAddressfrom); //Emailin kimden gönderildiği ayarlanıyor (burada gönderen kişi "AdminRegister").

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", txtEmail.Text);
            mimeMessage.To.Add(mailboxAddressTo); //Emailin kime gideceği belirtiliyor (kullanıcının email adresi).

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Email Adresinizin Konfirmasyon Kodu: " + confirmCode;

            mimeMessage.Body = bodyBuilder.ToMessageBody(); //Email içeriği(body) oluşturuluyor.
            mimeMessage.Subject = "Email Konfirmasyon Kodu"; //Email konusu(subject) oluşturuluyor.

            SmtpClient smtpClient = new SmtpClient(); 
            smtpClient.Connect("smtp.gmail.com", 587, false); //Gmail SMTP sunucusuna bağlanılıyor.
            smtpClient.Authenticate("emirali19078@gmail.com", "itvohneaevgcqsub"); //Belirtilen email adresi ve şifre ile giriş yapılıyor.
            smtpClient.Send(mimeMessage); // Mail gönderiliyor.
            smtpClient.Disconnect(true); // bağlantı kapatılıyor.


            MessageBox.Show("Mail adresinize doğrulama kodu gönöderilmiştir. ");

            #endregion

            #region Email onayı için başka bir form açılıyor(FrmMailConfirm).

            FrmMailConfirm frmMailConfirm = new FrmMailConfirm();
            frmMailConfirm.email = txtEmail.Text;
            frmMailConfirm.Show();

            #endregion

            #region İşlem Özeti
            /*Formdan kullanıcı bilgilerini alıyor.
              Kullanıcıyı veritabanına ekliyor.
              Rastgele doğrulama kodu üretiyor.
              Kullanıcının mail adresine bu kodu gönderiyor.
              Kullanıcıdan gelen doğrulama koduyla aktifleşme bekleniyor.*/
            #endregion
        }
    }
}
