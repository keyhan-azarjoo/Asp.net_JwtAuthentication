// here You can Generate a JWT key


using System.Security.Cryptography;

var rsaKey = RSA.Create();
var privatekey = rsaKey.ExportRSAPrivateKey();
File.WriteAllBytes("key", privatekey);

