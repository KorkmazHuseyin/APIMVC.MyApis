using APIMVC.MyApis.DAL.AuthDAL.Interfaces;
using APIMVC.MyApis.DAL.Context;
using APIMVC.MyApis.DAL.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APIMVC.MyApis.DAL.AuthDAL.Concrete
{
    public class AuthDAL:IAuthDAL
    {

        private readonly AuthContext _context;
        public AuthDAL(AuthContext context)
        {
            _context = context;
        }




        // Bu metod daha önce sisteme girmiş kaydolmuş birini sisteme tekrar girmek istediğinde önceki şifresi ile griş yapmaya çalıştığı şifresini karşılaştırmaya yarıyor.

        private bool KontrolEt(string password, byte[] passwordHash, byte[] passwordSalt)// daha önce kaydolan birisi olduğu şiçin passHash ve passSalt değer zatan var kabul ediyoruz.
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var passHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < password.Length; i++)
                {
                    if (passHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }




        // bu metod daha önce sisteme kaydo gerçekleşmiş birininin KONTROLET metodu ile şifresini encoding ediyor. username lerini kontol ediyor. eğer kişi doğru username ve şifre girmişse girişe izin veriyor. Aksi halde null return ediyor.
        public async Task<User> Login(string username, string password)
        {
            /// Username ine bakıyorum böyle bir username var mı yok mu?
            var kisi = await _context.User.FirstOrDefaultAsync(a => a.UserName == username);
            if (kisi == null)
            {
                return null;
            }
            // Kişinin şifresi kontrol edilecek kişi şifresini doğru girmiş mi ? 
            if (!KontrolEt(password, kisi.PasswordHash, kisi.PasswordSalt))
            {
                return null;
            }
            return kisi;
        }










        //Register işleminin gerçekleşebilmesi için kullanıcıdan alınan şifrenin de şifrelenmesi gerekiyor. bunu daaşağıdaki metod ile yaptık. tegister metodunda da aşağıdaki metodu kullandık. Kullanıcı kaydı bu 2 metodla yapılabiliyor.

        private void KullaniciKaydetSifre(string password, out byte[] passHash, out byte[] passSalt)
        {
            //Niye using içinde kullanıyoruz. Çünki burda şifre şifrelenecek. Amaç önce güvenlik. Using ten çıktığı anda silinsin gitsin .

            using (HMACSHA512 hmac = new HMACSHA512())
            {
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));// Bu satır. ComputeHashmetodu ile içine verdiğin heğeri hashler/ yani kriptolar.Bunu yapmak için ( password ümüzün baytlerine ayrılması lazım (Getbytes metodu bunu yapıyor.) Sonrada Encoding sınıfı ile de byt[] tipine çevriliyor.)  

                passSalt = hmac.Key; // Şifrelemenin anahtarı da burda. Hash paswordün şifreli halini tutar. Salt nasıl şifrelendiğini tutar Key ile.
            }
        }


        // Kullanıcı kaydetme işlemini burada yapıyoruz.
        public async Task<User> Register(User user, string password)
        {

            byte[] passHash, passSalt;
            KullaniciKaydetSifre(password, out passHash, out passSalt);
            user.PasswordHash = passHash;
            user.PasswordSalt = passSalt;
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }








































        // Kullanıcı var mı yokmu diye bakıyoruz. AnyAsync metodu buda en uygun olanı performans açısından. İçeri verdiğimiz kullanıcı ismi varmı yok mu diye kontrol edebiliyor var yok diye dönebiliyor.

        public async Task<bool> UserExists(string username)
        {
            return !await _context.User.AnyAsync(a => a.UserName == username);
        }

    }
}
