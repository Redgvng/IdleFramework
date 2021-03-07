using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.EventSystems;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;

public class AES
{
    string pw = "kkyyhka";
    string salt = "stkttnsstkttns";


    public byte[] encrypt(byte[] src)
    {
        RijndaelManaged rijndael = new RijndaelManaged();
        rijndael.KeySize = 128;
        rijndael.BlockSize = 128;

        byte[] bSalt = Encoding.UTF8.GetBytes(salt);
        Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(pw, bSalt);
        deriveBytes.IterationCount = 1000;//反復回数

        rijndael.Key = deriveBytes.GetBytes(rijndael.KeySize / 8);
        rijndael.IV = deriveBytes.GetBytes(rijndael.BlockSize / 8);

        //暗号化
        ICryptoTransform encryptor = rijndael.CreateEncryptor();
        byte[] encrypted = encryptor.TransformFinalBlock(src, 0, src.Length);

        //Debug.Log (Convert.ToBase64String (encrypted));

        encryptor.Dispose();
        return encrypted;
    }

    public byte[] dencrypt(byte[] src)
    {

        RijndaelManaged rijndael = new RijndaelManaged();
        rijndael.KeySize = 128;
        rijndael.BlockSize = 128;

        byte[] bSalt = Encoding.UTF8.GetBytes(salt);
        Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(pw, bSalt);
        deriveBytes.IterationCount = 1000;//反復回数

        rijndael.Key = deriveBytes.GetBytes(rijndael.KeySize / 8);
        rijndael.IV = deriveBytes.GetBytes(rijndael.BlockSize / 8);

        //復号化
        ICryptoTransform decryptor = rijndael.CreateDecryptor();
        byte[] plain = decryptor.TransformFinalBlock(src, 0, src.Length);

        //Debug.Log ("復号   " + System.Text.Encoding.UTF8.GetString(plain));

        decryptor.Dispose();
        return plain;
    }
}
