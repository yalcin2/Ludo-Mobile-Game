using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;

namespace Common
{
    public class Encryption
    {
        public static string HashPassword(string password)
        {
            //SHA512, SHA256
            //MD5, SHA1 (weak algorithms)

            var myAlg = SHA512.Create();


            //to convert from user input to bytes always use Encoding.utf8.getbytes
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            byte[] digest = myAlg.ComputeHash(passwordAsBytes); //hashes the password

            //in order to store them in a varchar field in db
            //to convert from cryptographic input/output to string always use Convert
            return Convert.ToBase64String(digest);

        }

        public static string SymmetricEncrypt(string value)
        {
            //Rinjdael
            //TripleDES
            //DES
            //RC2
            //AES

            var myAlg = Rijndael.Create();
            string key = ConfigurationManager.AppSettings["password"];

            // getting the password from the web.config
            // using a built-in algorithm to generate the key and the iv
            // added with a salt(against dictionary attacks to guess the key)
            Rfc2898DeriveBytes myKeyGenerator = new Rfc2898DeriveBytes(key, new byte[] { 34, 54, 65, 87, 43, 123, 12, 255});

            //using the method getbytes to get a random number of bytes; divided by 8 because the Keysize/blocksize are in bits
            myAlg.Key = myKeyGenerator.GetBytes(myAlg.KeySize / 8);
            myAlg.IV = myKeyGenerator.GetBytes(myAlg.BlockSize / 8);

            //converted into an array of bytes the input of the user
            byte[] inputAsBytes = Encoding.UTF8.GetBytes(value);

            //converted the input of the user into a MemoryStream
            MemoryStream msInput = new MemoryStream(inputAsBytes);

            //the actual encryption process takes place via an object called CryptoStream
            //which as input takes:
            //1) the data you`re going to encrypt as a stream; hence it was converted into a stream
            // in the previous line
            //2) engine generated from the algorithm
            //3) read the data inside msInput

            CryptoStream cs = new CryptoStream(msInput, myAlg.CreateEncryptor(), CryptoStreamMode.Read);

            //prepare where you are going to store the cipher
            MemoryStream msOut = new MemoryStream();

            //encrypt the data by
            //while cs was copying the data, it was also encrypting the data with the parameters provided above
            cs.CopyTo(msOut);

            byte[] cipher = msOut.ToArray();

            //converting from byte to string (since the method returns a string)
            return Convert.ToBase64String(cipher);


        }

        public static string EncryptQueryString(string input)
        {
            string output = SymmetricEncrypt(input);
            //characters: a-z, 0-9 + / % & =
            output = output.Replace('+', '|');
            output = output.Replace('/', '$');
            output = output.Replace('%', ',');
            output = output.Replace('=', ';');

            return output;
        }

        public static string SymmetricDecrypt(string value)
        {
            //Rinjdael
            //TripleDES
            //DES
            //RC2
            //AES

            var myAlg = Rijndael.Create();
            string key = ConfigurationManager.AppSettings["password"];

            // getting the password from the web.config
            // using a built-in algorithm to generate the key and the iv
            // added with a salt(against dictionary attacks to guess the key)
            Rfc2898DeriveBytes myKeyGenerator = new Rfc2898DeriveBytes(key, new byte[] { 34, 54, 65, 87, 43, 123, 12, 255 });

            //using the method getbytes to get a random number of bytes; divided by 8 because the Keysize/blocksize are in bits
            myAlg.Key = myKeyGenerator.GetBytes(myAlg.KeySize / 8);
            myAlg.IV = myKeyGenerator.GetBytes(myAlg.BlockSize / 8);

            //converted into an array of bytes the input of the user
            byte[] inputAsBytes = Convert.FromBase64String(value);

            //converted the input of the user into a MemoryStream
            MemoryStream msInput = new MemoryStream(inputAsBytes);

            //the actual encryption process takes place via an object called CryptoStream
            //which as input takes:
            //1) the data you`re going to encrypt as a stream; hence it was converted into a stream
            // in the previous line
            //2) engine generated from the algorithm
            //3) read the data inside msInput
            msInput.Position = 0;
            CryptoStream cs = new CryptoStream(msInput, myAlg.CreateDecryptor(), CryptoStreamMode.Read);

            //prepare where you are going to store the cipher
            MemoryStream msOut = new MemoryStream();

            //encrypt the data by
            //while cs was copying the data, it was also encrypting the data with the parameters provided above
            cs.CopyTo(msOut);

            byte[] original = msOut.ToArray();

            //converting from byte to string (since the method returns a string)
            return Encoding.UTF8.GetString(original);


        }

        public static string DecryptQueryString(string input)
        {
            input = input.Replace('|', '+');
            input = input.Replace('$', '/');
            input = input.Replace(',', '%');
            input = input.Replace(';', '=');

            string output = SymmetricDecrypt(input);
            //characters: a-z, 0-9 + / % & =


            return output;
        }

        public static AssymetricKeys GenerateAsymmetricKeys()
        {
            //RSA, DSA

            RSA myAlg = RSA.Create();
            //Automatically generates the pubic and the private keys
            AssymetricKeys myKeys = new AssymetricKeys();
            myKeys.PublicKey = myAlg.ToXmlString(false);
            myKeys.Privatekey = myAlg.ToXmlString(true);

            return myKeys;
        }

        /*
        public string AsymmetricEncryptString(string input,string publickey)
        {
            //1. Declaring the alg
            RSA myAlg = RSA.Create();
            myAlg.FromXmlString(publickey);
            //2 Converting from string(non crypto input therefore use Encoding)
            byte[] inputAsBytes = Encoding.UTF8.GetBytes(input);
            //3 Encrypt
            byte[] cipher = myAlg.Encrypt(inputAsBytes, RSAEncryptionPadding.Pkcs1);
            //4 If you want to return back a string convert using base64(crypto input therefore use Convert)
            myAlg.EncryptValue(inputAsBytes);

            return Convert.ToBase64String(cipher);


        }

        public string AsymmetricDecryptString(string encryptedInput, string privateKey)
        {
            //1. Declaring the alg
            RSA myAlg = RSA.Create();
            myAlg.FromXmlString(privateKey);
            //2 Converting from string to bytes
            byte[] cipher = Convert.FromBase64String(encryptedInput);
            //3 Encrypt
            byte[] originalValue = myAlg.Decrypt(cipher, RSAEncryptionPadding.Pkcs1);
            //4 If you want to return back a string convert using base64(crypto input therefore use Convert)  
            return Convert.ToBase64String(cipher);
        }
        */

        public static byte[] AsymmetricEncrypt(byte[] input, string publickey)
        {
            //1. declaring the alg
            RSA myAlg = RSA.Create();
            myAlg.FromXmlString(publickey);

            //3 encrypt
            byte[] cipher = myAlg.Encrypt(input, RSAEncryptionPadding.Pkcs1);
            //4 if you want to return back a string convert using base64 (crypto input therefore use Convert)
            return cipher;
        }

        public static byte[] AsymmetricDecrypt(byte[] encryptedInput, string privateKey)
        {
            //1. declaring the alg
            RSA myAlg = RSA.Create();
            myAlg.FromXmlString(privateKey);

            //2 converting from string to bytes  

            //3 encrypt
            byte[] originalValue = myAlg.Decrypt(encryptedInput, RSAEncryptionPadding.Pkcs1);
            //4 if you want to return back a string convert using base64 (crypto input therefore use Convert)
            return (originalValue);
        }

        public static MemoryStream SymmetricEncrypt(Stream input, byte[] key, byte[] iv)
        {
            input.Position = 0;

            var myAlg = Rijndael.Create();
            myAlg.Key = key;
            myAlg.IV = iv;

            //the actual encryption process takes place via an object called CryptoStream
            //which as input takes:
            //1) the data you're going to encrypt as a Stream; hence it was converted into a stream
            //   in the previous line
            //2) engine generated from the algorithm
            //3) read the data inside msInput
            CryptoStream cs = new CryptoStream(input, myAlg.CreateEncryptor(), CryptoStreamMode.Read);

            //prepare where you are going to store the cipher 
            MemoryStream msOut = new MemoryStream();

            //encrypt the data by
            //while cs was copying the data , it was also encrypting the data with the parameters provided above
            cs.CopyTo(msOut);

            //converting from byte to string (since the method returns a string)
            return msOut;
        }


        public static MemoryStream HybridEncrypt(Stream inputFile, string publicKey)
        {
            inputFile.Position = 0;
            //1. make sure that the publicKey variable is not empty

            //2. 
            //i.  declaring the symmetric algorithm that you will use
            //ii. call the GenerateIV() and GenerateKey()
            Rijndael myAlg = Rijndael.Create();
            myAlg.GenerateKey();
            myAlg.GenerateIV();

            //3. 
            //i. extract the key and the iv from the symmetric algorithm
            var iv = myAlg.IV;
            var key = myAlg.Key;



            //ii.  call the method  AsymmetricEncrypt(key, publicKey)
            byte[] encKey = AsymmetricEncrypt(key, publicKey);
            //check the size of the encrypted key ******
            //iii. call the method  AsymmetricEncrypt(iv, publicKey)

            //4. symmetrically encrypt using the iv and key in step 3. the inputFile

            MemoryStream msEncryptedAudioFile = SymmetricEncrypt(inputFile, key, iv);



            MemoryStream msOut = new MemoryStream();
            //5.save the encrypted key from 3.2
            msOut.Write(key, 0, key.Length);
            //6.save the encrypted iv from 3.3
            msOut.Write(iv, 0, iv.Length);
            //7.save the encrypted file contents
            //copy the output of the CryptoStream into msOut by calling CopyTo
            msEncryptedAudioFile.CopyTo(msOut);

            //8. return msOut
            return msOut;

        }

        public static MemoryStream HybridDecrypt(Stream encryptedFile, string privateKey)
        {
            encryptedFile.Position = 0;

            //1. retrieve the private key of the file owner

            //2. retrieve the encrypted secret key and the encrypted iv
            byte[] encryptedSecretKey = new byte[128]; //**** note: check the size of the encrypted key
            encryptedFile.Read(encryptedSecretKey, 0, 128);

            byte[] encryptedIv = new byte[128]; //**** note: check the size of the encrypted iv
            encryptedFile.Read(encryptedIv, 0, 128);

            //3. asymmetrically decrypt the secret key and the iv from no. 2 with the private key from no.1
            //i.  call the method byte[] decryptedKey = AsymmetricDecrypt(encryptedSecretKey, privateKey)
            //ii. call the method  AsymmetricDecrypt(encryptedIv, privateKey)


            //4. create the algorithm instance used in encryption, load it with the decrypted secret key and iv
            // myAlg.Key = decryptedKey
            // myAlg.IV = decryptedIV  ...from step no 3.

            //5. symmetrically decrypt the remaining file content using the parameters in step no 4
            //i.
            MemoryStream remainingFileContent = new MemoryStream();
            encryptedFile.CopyTo(remainingFileContent);

            //ii. decrypt the remainingFileContent using the symmetric with parameters from step 4.
            //MemoryStream decryptedFileContent = SymmetricDecrypt(remainingFileContent, decryptedKey, decryptedIv);



            MemoryStream msOut = new MemoryStream();


            //6. return the decryptedFileContent
            return msOut;

        }


        public static string SignFile(byte[] input, string privateKey)
        {
            //sign a file with the private key

            var rsa = RSA.Create();
            rsa.FromXmlString(privateKey);

            byte[] signature = rsa.SignData(input, new HashAlgorithmName("SHA512"), RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signature);

        }

        public static bool VerifyFile(byte[] input, string publicKey, string signature)
        {
            //verify a file with the public key
            var rsa = RSA.Create();
            rsa.FromXmlString(publicKey);

            bool result = rsa.VerifyData(input, Convert.FromBase64String(signature), new HashAlgorithmName("SHA512"), RSASignaturePadding.Pkcs1);

            //true means that the file is still the same
            //false means the file was somehow changed that it does not match with the signature passed

            return result;
        }

    }
}
