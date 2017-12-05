using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://programmingblockchain.gitbooks.io/programmingblockchain/content/bitcoin_transfer/private_key.html

            //01.01 Bitcoin Address
            Key privateKey = new Key(); // generate a random private key
            Console.WriteLine(privateKey.ToString(Network.TestNet));    // cUsadWixPXmRFKhXypDDmyu913Cjum5PbKCewyfuYDwoR8tWyo11
            
            PubKey publicKey = privateKey.PubKey;
            Console.WriteLine(publicKey); // 0352b8fb0eb2d4b36ccc52b886c50a7795ea6d469a5c37fb736ab9bc5ab2be7daf

            Console.WriteLine(publicKey.GetAddress(Network.Main)); // 1BfAsy1ekCvyuVGE6d5U3ZmhJohsA8n585
            Console.WriteLine(publicKey.GetAddress(Network.TestNet)); // mrB8B26dZENEgbjqpC3qsUz2AoJa692yb8
            // ACK got for mrB8B26dZENEgbjqpC3qsUz2AoJa692yb8: ea99f672b770c0fc78c0126d651bc64f18a030cf251640470be75940f387c138 ea8ca76e06052a63c776e7918621390bfeab359113cb4f00b923112c6d08b893
            // send back to 2N8hwP1WmJrFF5QWABn38y63uYLhnJYJYTF

            var publicKeyHash = publicKey.Hash;
            Console.WriteLine(publicKeyHash); // d4b0f0149586597ef17388116cab40de046b6981

            //01.02 ScriptPubKey
            var mainNetAddress = publicKeyHash.GetAddress(Network.Main);
            var testNetAddress = publicKeyHash.GetAddress(Network.TestNet);

            publicKeyHash = new KeyId(publicKeyHash.ToString());

            testNetAddress = publicKeyHash.GetAddress(Network.TestNet);
            mainNetAddress = publicKeyHash.GetAddress(Network.Main);

            Console.WriteLine(mainNetAddress.ScriptPubKey); // OP_DUP OP_HASH160 d4b0f0149586597ef17388116cab40de046b6981 OP_EQUALVERIFY OP_CHECKSIG
            Console.WriteLine(testNetAddress.ScriptPubKey); // OP_DUP OP_HASH160 d4b0f0149586597ef17388116cab40de046b6981 OP_EQUALVERIFY OP_CHECKSIG

            var paymentScript = publicKeyHash.ScriptPubKey;
            Console.WriteLine(paymentScript);
            var sameMainNetAddress = paymentScript.GetDestinationAddress(Network.Main);
            Console.WriteLine(sameMainNetAddress);
            Console.WriteLine(mainNetAddress == sameMainNetAddress); // True
            var samePublicKeyHash = (KeyId)paymentScript.GetDestination();
            Console.WriteLine(publicKeyHash == samePublicKeyHash); // True
            var sameMainNetAddress2 = new BitcoinPubKeyAddress(samePublicKeyHash, Network.Main);
            Console.WriteLine(mainNetAddress == sameMainNetAddress2); // True

            //01.03 Private key
            BitcoinSecret mainNetPrivateKeySecret = privateKey.GetBitcoinSecret(Network.Main);  // generate our Bitcoin secret(also known as Wallet Import Format or simply WIF) from our private key for the mainnet
            BitcoinSecret testNetPrivateKeySecret = privateKey.GetBitcoinSecret(Network.TestNet);  // generate our Bitcoin secret(also known as Wallet Import Format or simply WIF) from our private key for the testnet
            Console.WriteLine(mainNetPrivateKeySecret); 
            Console.WriteLine(testNetPrivateKeySecret);

            bool WifIsBitcoinSecret = mainNetPrivateKeySecret == privateKey.GetWif(Network.Main);
            Console.WriteLine(WifIsBitcoinSecret); // True

            BitcoinSecret bitcoinSecret = privateKey.GetWif(Network.Main); 
            Key samePrivateKey = bitcoinSecret.PrivateKey;
            Console.WriteLine(samePrivateKey == privateKey); // True

            publicKey = privateKey.PubKey;
            BitcoinPubKeyAddress bitcoinPublicKeyAddress = publicKey.GetAddress(Network.TestNet); 
            Console.WriteLine(bitcoinPublicKeyAddress);

            bitcoinSecret = new BitcoinSecret("cVX7SpYc8yjNW8WzPpiGTqyWD4eM4BBnfqEm9nwGqJb2QiX9hhdf", Network.TestNet);
            samePrivateKey = bitcoinSecret.PrivateKey;
            Console.WriteLine(samePrivateKey.ToString(Network.TestNet));
        }
    }
}
