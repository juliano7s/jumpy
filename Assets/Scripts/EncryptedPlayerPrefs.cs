using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
 
public class EncryptedPlayerPrefs  {
 
    // Encrypted PlayerPrefs
    // Written by Sven Magnus
    // MD5 code by Matthew Wegner (from [url]http://www.unifycommunity.com/wiki/index.php?title=MD5[/url])
    
    
    // Modify this key in this file :
    private static string privateKey="i6GHF5z47vIi1B32O13kH8pyh8Zj6Z2k";
    
    // Add some values to this array before using EncryptedPlayerPrefs
    public static string[] keys = {"Jo8Su22s0b", "YSVN88kmre", "a3Ab4dt7Jh", "6KyN4opcR3", "6ziF1sdga1", "786GDdtj40", "M2iN0NLfrc", "PcnijFL69K"};
    
    
    public static string Md5(string strToEncrypt) {
        UTF8Encoding ue = new UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
 
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
 
        string hashString = "";
 
        for (int i = 0; i < hashBytes.Length; i++) {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
 
        return hashString.PadLeft(32, '0');
    }
    
    public static void SaveEncryption(string key, string type, string value) {
        int keyIndex = (int)Mathf.Floor(Random.value * keys.Length);
        string secretKey = keys[keyIndex];
        string check = Md5(type + "_" + privateKey + "_" + secretKey + "_" + value);
        string md5Key = Md5 (key);
        PlayerPrefs.SetString(md5Key + "_encryption_check", check);
        PlayerPrefs.SetInt(md5Key + "_used_key", keyIndex);
    }
    
    public static bool CheckEncryption(string key, string type, string value) {
        string md5Key = Md5 (key);
        int keyIndex = PlayerPrefs.GetInt(md5Key + "_used_key");
        string secretKey = keys[keyIndex];
        string check = Md5(type + "_" + privateKey + "_" + secretKey + "_" + value);
        if(!PlayerPrefs.HasKey(md5Key + "_encryption_check")) return false;
        string storedCheck = PlayerPrefs.GetString(md5Key + "_encryption_check");
        return storedCheck == check;
    }
    
    public static void SetInt(string key, int value) {
        string md5Key = Md5 (key + privateKey);
        PlayerPrefs.SetInt(md5Key, value);
        SaveEncryption(key, "int", value.ToString());
    }
    
    public static void SetFloat(string key, float value) {
        string md5Key = Md5 (key + privateKey);
        PlayerPrefs.SetFloat(md5Key, value);
        SaveEncryption(key, "float", Mathf.Floor(value*1000).ToString());
    }
    
    public static void SetString(string key, string value) {
        string md5Key = Md5 (key + privateKey);
        PlayerPrefs.SetString(md5Key, value);
        SaveEncryption(key, "string", value);
    }
    
    public static int GetInt(string key) {
        return GetInt(key, 0);
    }
    
    public static float GetFloat(string key) {
        return GetFloat(key, 0f);
    }
    
    public static string GetString(string key) {
        return GetString(key, "");
    }
    
    public static int GetInt(string key,int defaultValue) {
        string md5Key = Md5 (key + privateKey);
        int value = PlayerPrefs.GetInt(md5Key);
        if(!CheckEncryption(key, "int", value.ToString())) return defaultValue;
        return value;
    }
    
    public static float GetFloat(string key, float defaultValue) {
        string md5Key = Md5 (key + privateKey);
        float value = PlayerPrefs.GetFloat(md5Key);
        if(!CheckEncryption(key, "float", Mathf.Floor(value*1000).ToString())) return defaultValue;
        return value;
    }
    
    public static string GetString(string key, string defaultValue) {
        string md5Key = Md5 (key + privateKey);
        string value = PlayerPrefs.GetString(md5Key);
        if(!CheckEncryption(key, "string", value)) return defaultValue;
        return value;
    }
    
    public static bool HasKey(string key) {
        return PlayerPrefs.HasKey(key);
    }
    
    public static void DeleteKey(string key) {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.DeleteKey(key + "_encryption_check");
        PlayerPrefs.DeleteKey(key + "_used_key");
    }
    
}
