var charset = new char[]
{
    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
};
var SECRET = "816649";
var unsafeAccount = new Account(SECRET, CompareEarlyExit);

var attacker = new Attacker();
var unsafeResult = attacker.FindSecret(unsafeAccount, SECRET.Length, charset);
Console.WriteLine(unsafeResult);
Console.WriteLine($"Attacker can login: {unsafeAccount.Login(unsafeResult)}");
// Current secret: 8-----
// Current secret: 81----
// Current secret: 816---
// Current secret: 8166--
// Current secret: 81664-
// Current secret: 816649
// 816649
// Attacker can login: True

var safeAccount = new Account(SECRET, CompareFullscan);
var safeResult = attacker.FindSecret(safeAccount, SECRET.Length, charset);
Console.WriteLine(safeResult);
Console.WriteLine($"Attacker can login: {safeAccount.Login(safeResult)}");
// Current secret: 0-----
// Current secret: 03----
// Current secret: 032---
// Current secret: 0328--
// Current secret: 03288-
// Current secret: 032880
// 032880
// Attacker can login: False

var opensslAccount = new Account(SECRET, CompareOpenssl);
var opensslResult = attacker.FindSecret(opensslAccount, SECRET.Length, charset);
Console.WriteLine(opensslResult);
Console.WriteLine($"Attacker can login: {opensslAccount.Login(opensslResult)}");
// Current secret: 0-----
// Current secret: 05----
// Current secret: 056---
// Current secret: 0567--
// Current secret: 05678-
// Current secret: 056780
// 056780
// Attacker can login: False

bool CompareEarlyExit(string a, string b) {
    if (a.Length != b.Length) return false;
    for (int i = 0; i < a.Length; i++)
        if (a[i] != b[i]) return false;
    return true;
}

bool CompareFullscan(string a, string b) {
    var result = true;
    if (a.Length != b.Length) return false;
    for (int i = 0; i < a.Length; i++)
        if (a[i] != b[i]) result = false;
    return result;
}

bool CompareOpenssl(string a, string b) {
    if (a.Length != b.Length) return false;
    var result = 0;
    for (int i = 0; i < a.Length; i++)
        result |= a[i] ^ b[i];
    return result == 0;
}
