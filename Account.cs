public sealed class Account(string secret, Func<string, string, bool> cmp) {
    private readonly string _secret = secret;
    private readonly Func<string, string, bool> _cmp = cmp;

    public bool Login(string guess) => _cmp(guess, _secret);
}

