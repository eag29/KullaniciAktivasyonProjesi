using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KullaniciAktivasyonProjesi
{
    public partial class FrmMailConfirm : Form
    {
        public FrmMailConfirm()
        {
            InitializeComponent();
        }

        KullaniciAktivasyonVtEntities context = new KullaniciAktivasyonVtEntities();
        public string email;
        private void btnregister_Click(object sender, EventArgs e)
        {
            var value = context.Tbl_User.Where(x => x.Email == txtEmail.Text).Select(y => y.ConfirmCode).FirstOrDefault();

            if (txtkod.Text == value.ToString())
            {
                var value2 = context.Tbl_User.Where(x => x.Email == txtEmail.Text).FirstOrDefault();
                value2.isConfirm = true;
                context.SaveChanges();
                MessageBox.Show("Hesabınız aktif edildi");
            }
            else
            {
                MessageBox.Show("Hatalı Kod");
            }
        }
        private void FrmMailConfirm_Load(object sender, EventArgs e)
        {
            txtEmail.Text = email;
        }
    }
}
