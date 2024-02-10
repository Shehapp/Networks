using System;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        // Define the parameters (g, n)
        BigInteger g = 2; // generator
        BigInteger n = 23; // prime modulus

        // Alice's private key
        BigInteger AlicePrivate = 6;

        // Bob's private key
        BigInteger BobPrivate = 15;

        // Calculate public keys
        BigInteger AlicePublic = BigInteger.ModPow(g, AlicePrivate, n);
        BigInteger BobPublic = BigInteger.ModPow(g, BobPrivate, n);

        // Calculate shared secrets
        BigInteger AliceSecret = BigInteger.ModPow(BobPublic, AlicePrivate, n);
        BigInteger BobSecret = BigInteger.ModPow(AlicePublic, BobPrivate, n);

        // Output the shared secrets (should be the same)
        Console.WriteLine("Alice's shared secret: " + AliceSecret);
        Console.WriteLine("Bob's shared secret:   " + BobSecret);
    }
}