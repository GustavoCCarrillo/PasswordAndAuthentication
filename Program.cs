using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace PasswordAndAuthentication
{
    //collaborated wth Phill and Jack 9/22/2020


    class Program 
    {
        static void Main(string[] args)
        {
            EnterProgram();
        }
        static void EnterProgram()
        {
            bool isActive = true;
            string userinput;
            while (isActive)
            {
                Console.WriteLine("Hello there!, please enter 1 to create an account.");
                Console.WriteLine("enter 2 to authenticate the account and password");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1) Create an account type" +
                                "\n2) Authenticate a user" +
                                "\n3) Once created, enter 3 to print a list of users and passwords");
                userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "1":
                        CreateAccount();
                        isActive = false;
                        break;
                    case "2":
                        AuthenticateUser();
                        isActive = false;
                        break;
                    case "3":
                        PrintAllUser();
                        isActive = false;
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }


        static void CreateAccount()
        {
            string userName;
            string encryptedPass;

            Console.WriteLine("Username:");
            userName = Console.ReadLine();
            Console.WriteLine("Password");
            encryptedPass = Console.ReadLine();
            Console.WriteLine("Account Created");
            Console.Clear();
            encryptedPass = Encrypt(encryptedPass);
            Console.WriteLine($"Your password is Encrypted:{encryptedPass}");
            Console.WriteLine($"In Plain Text form: {Decrypt(encryptedPass)}");

            // Store User In Dictionary 
            StoreUser(userName, encryptedPass);
            Console.ReadKey();
            EnterProgram();
        }
        static void AuthenticateUser()
        {
            string userName;
            string password;

            Console.WriteLine("Username:");
            userName = Console.ReadLine();
            Console.WriteLine("Password");
            password = Console.ReadLine();
            AuthenticateUser(userName, password);
            Console.ReadKey();
            EnterProgram();

        }
        


        private static Dictionary<string, string> users = new Dictionary<string, string>();

        private static string hash = "0xld;okdfoe5r15fe@rn";
        public static string Encrypt(string userPass)
        {

            //Convert string into Bytes
            byte[] userBytePass = UTF8Encoding.UTF8.GetBytes(userPass);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDES.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(userBytePass, 0, userBytePass.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }

        }

        public static string Decrypt(string encryptedPass)
        {
            //Convert string into Bytes
            byte[] userBytePass = Convert.FromBase64String(encryptedPass);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDES.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(userBytePass, 0, userBytePass.Length);
                    return UTF8Encoding.UTF8.GetString(results);
                }
            }
        }

        public static void StoreUser(string userName, string encrytedPass)
        {
            // add user to dictionary 
            users.Add(userName, encrytedPass);
        }
        public static void AuthenticateUser(string userName, string userPass)
        {
            userPass = Encrypt(userPass);
            bool isAuthenticated = false;
            foreach (KeyValuePair<string, string> element in users)
            {
                if (element.Key == userName && element.Value == userPass)
                {

                    isAuthenticated = true;
                }
            }
            if (isAuthenticated)
            {
                Console.WriteLine("Authenticated!!!!");
            }
            else if (!isAuthenticated)
            {
                Console.WriteLine("Not Authenticated!!!!");
            }

        }
        public static void PrintAllUser()
        {
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
        }
    }
}
