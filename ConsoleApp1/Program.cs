using NBitcoin;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBitcoin.Crypto;
using NBitcoin.Stealth;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://programmingblockchain.gitbooks.io/programmingblockchain/content/bitcoin_transfer/private_key.html

            //01.01 Bitcoin Address
            BitcoinAddressSample();

            //01.02 ScriptPubKey
            ScriptPubKey();

            //01.03 Private key
            PrivateKey();

            //01.04 Transaction
            // Transaction();

            ////01.05 Blockchain
            // Blockchain();

            ////01.06 Proof of ownership
            ProofOfOwnership();

            //KeyGenerationAndEncryption();

            Pay2Sample();
        }

        private static void Pay2Sample()
        {
            var key = new Key();
            var publicKeyHash = key.PubKey.Hash;
            var bitcoinAddress = publicKeyHash.GetAddress(Network.TestNet);
            Console.WriteLine(key.ToString(Network.TestNet)); // 171LGoEKyVzgQstGwnTHVh3TFTgo5PsqiY
            Console.WriteLine(publicKeyHash); // 41e0d7ab8af1ba5452b824116a31357dc931cf28
            Console.WriteLine(bitcoinAddress); // 171LGoEKyVzgQstGwnTHVh3TFTgo5PsqiY

            var scriptPubKey = bitcoinAddress.ScriptPubKey;
            Console.WriteLine(scriptPubKey);
            var sameBitcoinAddress = scriptPubKey.GetDestinationAddress(Network.TestNet);
            Console.WriteLine(sameBitcoinAddress);

            //not all ScriptPubKey represent a Bitcoin Address
            Block genesisBlock = Network.Main.GetGenesis();
            Transaction firstTransactionEver = genesisBlock.Transactions.First();
            var firstInputEver = firstTransactionEver.Inputs.First();
            var firstOutputEver = firstTransactionEver.Outputs.First();
            var firstScriptPubKeyEver = firstOutputEver.ScriptPubKey;
            var firstBitcoinAddressEver = firstScriptPubKeyEver.GetDestinationAddress(Network.Main);
            Console.WriteLine(firstBitcoinAddressEver == null); // True

            Console.WriteLine(firstTransactionEver);

            var firstPubKeyEver = firstScriptPubKeyEver.GetDestinationPublicKeys().First();
            Console.WriteLine(firstPubKeyEver);

            key = new Key();
            Console.WriteLine("Pay to public key : " + key.PubKey.ScriptPubKey);//P2PK (pay to public key) 
            Console.WriteLine();
            Console.WriteLine("Pay to public key hash : " + key.PubKey.Hash.ScriptPubKey);//P2PKH (pay to public key hash).

            Console.WriteLine(key.PubKey.WitHash.ScriptPubKey); //P2WPKH
            Console.WriteLine(key.PubKey.WitHash);

        }

        private static void BitcoinAddressSample()
        {
            Key privateKey = new Key(); // generate a random private key
            Console.WriteLine(privateKey.ToString(Network.TestNet));
            // cUFZgMBEfhBi5R7PDL7itSbjUZ8DGPiPrCBPZa4Za9Q5hJq6Xmqo // basically is the secret

            PubKey publicKey = privateKey.PubKey;
            Console.WriteLine(publicKey); // 0324cfe85eb54d01e0d5d16177868f9cf9fd968a63e47fcce61acac778b8060731

            Console.WriteLine(publicKey.GetAddress(Network.Main)); // 18Av7Pm5knLGDu2CCAwVs6fPoy7UhyaC3Z
            Console.WriteLine(publicKey.GetAddress(Network.TestNet)); // mngsQSr4ZomX11Voujush1sifxiBadXvFK

            var publicKeyHash = publicKey.Hash;
            Console.WriteLine(publicKeyHash); // 18Av7Pm5knLGDu2CCAwVs6fPoy7UhyaC3Z
        }

        private static void ScriptPubKey()
        {
            Key privateKey = new Key();
            KeyId publicKeyHash = privateKey.PubKey.Hash;

            var mainNetAddress = publicKeyHash.GetAddress(Network.Main);
            var testNetAddress = publicKeyHash.GetAddress(Network.TestNet);

            publicKeyHash = new KeyId(publicKeyHash.ToString());

            testNetAddress = publicKeyHash.GetAddress(Network.TestNet);
            mainNetAddress = publicKeyHash.GetAddress(Network.Main);

            Console.WriteLine(mainNetAddress.ScriptPubKey);
            // OP_DUP OP_HASH160 18Av7Pm5knLGDu2CCAwVs6fPoy7UhyaC3Z OP_EQUALVERIFY OP_CHECKSIG
            Console.WriteLine(testNetAddress.ScriptPubKey);
            // OP_DUP OP_HASH160 18Av7Pm5knLGDu2CCAwVs6fPoy7UhyaC3Z OP_EQUALVERIFY OP_CHECKSIG

            var paymentScript = publicKeyHash.ScriptPubKey;
            Console.WriteLine(paymentScript);
            var sameMainNetAddress = paymentScript.GetDestinationAddress(Network.Main);
            Console.WriteLine(sameMainNetAddress);
            Console.WriteLine(mainNetAddress == sameMainNetAddress); // True
            var samePublicKeyHash = (KeyId)paymentScript.GetDestination();
            Console.WriteLine(publicKeyHash == samePublicKeyHash); // True
            var sameMainNetAddress2 = new BitcoinPubKeyAddress(samePublicKeyHash, Network.Main);
            Console.WriteLine(mainNetAddress == sameMainNetAddress2); // True
        }

        private static void PrivateKey()
        {
            Key privateKey = new Key(); // generate a random private key

            // generate our Bitcoin secret(also known as Wallet Import Format or simply WIF) from our private key for the mainnet
            BitcoinSecret mainNetPrivateKeySecret = privateKey.GetBitcoinSecret(Network.Main);

            // generate our Bitcoin secret(also known as Wallet Import Format or simply WIF) from our private key for the testnet
            BitcoinSecret testNetPrivateKeySecret = privateKey.GetBitcoinSecret(Network.TestNet);

            Console.WriteLine(mainNetPrivateKeySecret);
            Console.WriteLine(testNetPrivateKeySecret);

            bool WifIsBitcoinSecret = mainNetPrivateKeySecret == privateKey.GetWif(Network.Main);
            Console.WriteLine(WifIsBitcoinSecret); // True

            BitcoinSecret bitcoinSecret = privateKey.GetWif(Network.Main);
            Key samePrivateKey = bitcoinSecret.PrivateKey;
            Console.WriteLine(samePrivateKey == privateKey); // True

            var publicKey = privateKey.PubKey;
            BitcoinPubKeyAddress bitcoinPublicKeyAddress = publicKey.GetAddress(Network.TestNet);
            Console.WriteLine(bitcoinPublicKeyAddress);

            bitcoinSecret = new BitcoinSecret("cVX7SpYc8yjNW8WzPpiGTqyWD4eM4BBnfqEm9nwGqJb2QiX9hhdf", Network.TestNet);
            samePrivateKey = bitcoinSecret.PrivateKey;
            Console.WriteLine(samePrivateKey.ToString(Network.TestNet));
        }

        private static void ProofOfOwnership()
        {
            var message = "I am Craig Wright";
            var signature = "IN5v9+3HGW1q71OqQ1boSZTm0/DCiMpI8E4JB1nD67TCbIVMRk/e3KrTT9GvOuu3NGN0w8R2lWOV2cxnBp+Of8c=";

            var address = new BitcoinPubKeyAddress("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa");
            bool isCraigWrightSatoshi = address.VerifyMessage(message, signature);

            Console.WriteLine("Is Craig Wright Satoshi? " + isCraigWrightSatoshi);

            message = "Nicolas Dorier Book Funding Address";
            signature = "H1jiXPzun3rXi0N9v9R5fAWrfEae9WPmlL5DJBj1eTStSvpKdRR8Io6/uT9tGH/3OnzG6ym5yytuWoA9ahkC3dQ=";

            address = new BitcoinPubKeyAddress("1KF8kUVHK42XzgcmJF4Lxz4wcL5WDL97PB");
            var isNicDorier = address.VerifyMessage(message, signature);

            Console.WriteLine("Is Nic Dorier? " + isNicDorier);
        }

        private static void KeyGenerationAndEncryption()
        {
            // 02.01. Key Generation
            RandomUtils.AddEntropy("hello");
            RandomUtils.AddEntropy(new byte[] {1, 2, 3});
            var nsaProofKey = new Key();
            Console.WriteLine("Key with entropy " + nsaProofKey.GetBitcoinSecret(Network.TestNet));

            var derived = SCrypt.BitcoinComputeDerivedKey("hello", new byte[] {1, 2, 3});
            RandomUtils.AddEntropy(derived);

            var privateKey = new Key();
            var privateKeyBitcoinSecret = privateKey.GetWif(Network.TestNet);
            Console.WriteLine(privateKeyBitcoinSecret); // L1tZPQt7HHj5V49YtYAMSbAmwN9zRjajgXQt9gGtXhNZbcwbZk2r
            BitcoinEncryptedSecret bitcoinEncryptedSecret = privateKeyBitcoinSecret.Encrypt("password");
            Console.WriteLine(bitcoinEncryptedSecret); // 6PYKYQQgx947Be41aHGypBhK6TA5Xhi9TdPBkatV3fHbbKrdDoBoXFCyLK
            var decryptedBitcoinPrivateKey = bitcoinEncryptedSecret.GetSecret("password");
            Console.WriteLine(decryptedBitcoinPrivateKey); // L1tZPQt7HHj5V49YtYAMSbAmwN9zRjajgXQt9gGtXhNZbcwbZk2r

            //02.02 BIP38 and HD (Hierarchical Deterministic) Wallet
            var passphraseCode = new BitcoinPassphraseCode("my secret", Network.TestNet, null);
                // we give this passphraseCode to 3rd party
            EncryptedKeyResult encryptedKeyResult = passphraseCode.GenerateEncryptedSecret();
            var generatedAddress = encryptedKeyResult.GeneratedAddress;
            var encryptedKeySecretEc = encryptedKeyResult.EncryptedKey;
            var confirmationCode = encryptedKeyResult.ConfirmationCode;

            Console.WriteLine(confirmationCode.Check("my secret", generatedAddress)); // True
            var privateKeyBitcoinSecret1 = encryptedKeySecretEc.GetSecret("my secret");
            Console.WriteLine(privateKeyBitcoinSecret1.GetAddress() == generatedAddress); // True
            Console.WriteLine(privateKeyBitcoinSecret1); // KzzHhrkr39a7upeqHzYNNeJuaf1SVDBpxdFDuMvFKbFhcBytDF1R

            ExtKey masterKey = new ExtKey();
            Console.WriteLine("Master key : " + masterKey.ToString(Network.TestNet));
            for (int i = 0; i < 5; i++)
            {
                ExtKey extKey1 = masterKey.Derive((uint) i);
                Console.WriteLine("Key " + i + " : " + extKey1.ToString(Network.TestNet));
            }

            ExtKey extKey = new ExtKey();
            byte[] chainCode = extKey.ChainCode;
            Key key = extKey.PrivateKey;

            ExtKey newExtKey = new ExtKey(key, chainCode);

            ExtPubKey masterPubKey = masterKey.Neuter();
            for (int i = 0; i < 5; i++)
            {
                ExtPubKey pubkey = masterPubKey.Derive((uint) i);
                Console.WriteLine("PubKey " + i + " : " + pubkey.ToString(Network.TestNet));
            }

            ExtKey key1 = masterKey.Derive(4);

            //Check it is legit
            var generatedAddr = masterPubKey.Derive(4).PubKey.GetAddress(Network.TestNet);
            var expectedAddr = key1.PrivateKey.PubKey.GetAddress(Network.TestNet);
            Console.WriteLine("Generated address : " + generatedAddr);
            Console.WriteLine("Expected address : " + expectedAddr);

            ExtKey parent = new ExtKey();
            ExtKey child11 = parent.Derive(1).Derive(1);
            ExtKey sameChild11 = parent.Derive(new KeyPath("1/1"));
            Console.WriteLine("child11 : " + child11.PrivateKey.PubKey.GetAddress(Network.TestNet));
            Console.WriteLine("child11.PrivateKey == sameChild11.PrivateKey : " +
                              (child11.PrivateKey == sameChild11.PrivateKey));

            ExtKey ceoKey = new ExtKey();
            Console.WriteLine("CEO: " + ceoKey.ToString(Network.TestNet));
            ExtKey notHardenedAccountingKey = ceoKey.Derive(0, hardened: false);

            ExtPubKey ceoPubkey = ceoKey.Neuter();

            //Recover ceo key with accounting private key and ceo public key
            ExtKey ceoKeyRecovered = notHardenedAccountingKey.GetParentExtKey(ceoPubkey);
            Console.WriteLine("CEO recovered: " + ceoKeyRecovered.ToString(Network.TestNet));

            ExtKey hardenedAccountingKey = ceoKey.Derive(0, hardened: true);
            // ExtKey ceoKeyRecovered2 = hardenedAccountingKey.GetParentExtKey(ceoPubkey); => //throws exception

            Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve); // generate random 12 words list
            ExtKey hdRoot = mnemo.DeriveExtKey("my password");

            mnemo = new Mnemonic("minute put grant neglect anxiety case globe win famous correct turn link",
                Wordlist.English);
            hdRoot = mnemo.DeriveExtKey("my password");
            Console.WriteLine(mnemo);
            Console.WriteLine(hdRoot.ToString(Network.TestNet));

            var scanKey = new Key();
            var spendKey = new Key();
            BitcoinStealthAddress stealthAddress
                = new BitcoinStealthAddress
                (
                    scanKey: scanKey.PubKey,
                    pubKeys: new[] {spendKey.PubKey},
                    signatureCount: 1,
                    bitfield: null,
                    network: Network.TestNet);

            Transaction transaction = new Transaction();
            stealthAddress.SendTo(transaction, Money.Coins(1.0m));
            Console.WriteLine(transaction);
        }

        private static void Transaction()
        {
            //https://www.blocktrail.com/tBTC/tx/93be2085aa3055c404b48988efb60f224654bc27d74423b57db0838e74bf631e
            //https://blockchain.info/tx/f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94 
            //http://api.qbit.ninja/transactions/f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94 => find raw bytes and pass them below
            Transaction tx =
                new Transaction(
                    "0100000009dd9e20cecf519ce7cdec2c4890e60bbff540b2fafdca2b426304fd8fefc58847000000006b483045022100d08870b424f19d8b3921861bedec81599a9cd5f179e35e80d16709a296d41484022023f1c2a9eab7d5dd8a1043d1d423e185641e79d33f32491638c7caf6029105410121035904165d4ed4aae69b1adef8dd99a21dac2c1ad479188d9d7de3b341aae229deffffffffc5a6457d1532d7cfe5dc6802323806bfd81c365bc4bb9fdadd8cfb2fd39280c3000000006b483045022100f337bc11e419e2317a59a0acd33c2937823aa2b015a1579bd6caab6f55dc828602201cc6985189b2b654ee9b71850697460086429c91f5e90598ca927dfbe315a3940121034e1304481c7403a35e1348289468df9982bbd516a3aedb7f1bc81667f7a09c5dffffffffa94e871f35a616b5a22139cc7dc5a4da35061d6317accb935a4af037573c1dc3000000008a473044022045476041f0d2910269ee4e707dda5678af483339962c0a2d7897c3aa78cb29ea0220476032a6bbe59e67ad5f95cc2f3e5264bb2bca8ea88eb30c96123b9ff7a33a5001410482d593f88a39160eaed14470ee4dad283c29e88d9abb904f953115b1a93d6f3881d6f8c29c53ddb30b2d1c6b657068d60a93ed240d5efca247836f6395807bcdffffffff8d199257330921571d8984bf38c47304b26e3c497a09acc298941e60b998ccfb010000008b483045022100fc8e579f4cabda1e26a294b3f7f227087b64ca2451155b8747bd1f6c96780d6d022041912d38512030e1ec1d3df6b8d91d8b9aa4c564642fd7cafc48f97fd550100101410482d593f88a39160eaed14470ee4dad283c29e88d9abb904f953115b1a93d6f3881d6f8c29c53ddb30b2d1c6b657068d60a93ed240d5efca247836f6395807bcdffffffff0246c072469f817ec87273c0a0d6b30fc840a6aa31f56427cca2ce31163c49fd000000006a473044022034cd8a6ecc391539af0e0af1075cf48599a40fb011936f2397a6b457e5fb60bd02201d059c7cf571d80bb6ead165639334fc6e45985c34a0cf7bc10d9a1817d22d0501210249af09f7a52e81c6de1df85a00792522df76b6a529178673d27754772f5d2758ffffffff2515c8cbc51039bc50bc8b4504617410824d1858dcff721ab3716dae44f350a9000000008b48304502210095a36910e3a466697f0a3a42cd0e487c280b1d48f8a8ea3d2867c1bb6fa6a4cf0220486eb68f95ae081e42dd48cb96d01b9536761e17b4f2ae10935aa406ceed268d01410482d593f88a39160eaed14470ee4dad283c29e88d9abb904f953115b1a93d6f3881d6f8c29c53ddb30b2d1c6b657068d60a93ed240d5efca247836f6395807bcdffffffff36c456759b51e87a75673d8bd8a1d91177164767ea937a4365578930cd8bf855000000006b483045022100eae7bf8ead57bedc858700ff7e8f0f917650a97d905ecc5264c29c6a4e87f7ac022055483fac8618831d163370bb8a083aff5111c795bafdbd48693cf98b5f2e420b012103b1534714e589d87484e2305e32261fc157a7ddd3ca060f5293a3dfbc76b7576bffffffff50a85ca81a0c667b6484ac371493be2a5298fe9e04b095f545cc795ba7dfda19000000006b483045022100dab39ceb5f48718fd3f7f549f5cb28fdd9bca755d031a15f608ebc7902ede62502200fcd17229262dd183fbc134279a9139d9e2a1e1e5723adfec8f3599e3f62b6ed0121025fbd9ac3c3277a06e623dfed29f9d490c643c023987a0412308c4a8e78b12b55ffffffff66fb69807af8e4d8f0cbfece1f02fed8c130c168f3b06d10640d02ffdebf2d90000000008b483045022100df2e15424b9664be46e5eef90030b557655ffd4b9f1dfc4dfd5a0422e8e8d13202204c94a8c9975f914f7926cda55b04193328f612d154fa6c6c908c88b4a4f9729201410482d593f88a39160eaed14470ee4dad283c29e88d9abb904f953115b1a93d6f3881d6f8c29c53ddb30b2d1c6b657068d60a93ed240d5efca247836f6395807bcdffffffff01a4c5a84e000000001976a914b6cefbb855cabf6ee45598f518a98011c22961aa88ac00000000");

            // Create a client
            QBitNinjaClient client = new QBitNinjaClient(Network.Main);
            // Parse transaction id to NBitcoin.uint256 so the client can eat it
            var transactionId = uint256.Parse("f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94");
            // Query the transaction
            GetTransactionResponse transactionResponse = client.GetTransaction(transactionId).Result;
            Transaction transaction = transactionResponse.Transaction;
            Console.WriteLine(transactionResponse.TransactionId);
            // f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94
            Console.WriteLine(transaction.GetHash());
                // f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94

            List<ICoin> receivedCoins = transactionResponse.ReceivedCoins;
            //foreach (var coin in receivedCoins)
            //{
            //    Money amount = (Money)coin.Amount;

            //    Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
            //    var paymentScript1 = coin.TxOut.ScriptPubKey;
            //    Console.WriteLine(paymentScript1);  // It's the ScriptPubKey
            //    var address = paymentScript1.GetDestinationAddress(Network.Main);
            //    Console.WriteLine(address); // 1HfbwN6Lvma9eDsv7mdwp529tgiyfNr7jc
            //    Console.WriteLine();
            //}

            List<ICoin> spentCoins = transactionResponse.SpentCoins;
            //foreach (var coin in spentCoins)
            //{
            //    Money amount = (Money)coin.Amount;

            //    Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
            //    var paymentScript1 = coin.TxOut.ScriptPubKey;
            //    Console.WriteLine(paymentScript1);  // It's the ScriptPubKey
            //    var address = paymentScript1.GetDestinationAddress(Network.Main);
            //    Console.WriteLine(address); // 1HfbwN6Lvma9eDsv7mdwp529tgiyfNr7jc
            //    Console.WriteLine();
            //}

            var outputs = transaction.Outputs;
            //foreach (TxOut output in outputs)
            //{
            //    Money amount = output.Value;

            //    Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
            //    var paymentScript2 = output.ScriptPubKey;
            //    Console.WriteLine(paymentScript2);  // It's the ScriptPubKey
            //    var address = paymentScript2.GetDestinationAddress(Network.Main);
            //    Console.WriteLine(address);
            //    Console.WriteLine();
            //}

            var inputs = transaction.Inputs;
            //foreach (TxIn input in inputs)
            //{
            //    OutPoint previousOutpoint = input.PrevOut;
            //    Console.WriteLine(previousOutpoint.Hash); // hash of prev tx
            //    Console.WriteLine(previousOutpoint.N); // idx of out from prev tx, that has been spent in the current tx
            //    Console.WriteLine();
            //}

            OutPoint firstOutPoint = receivedCoins.First().Outpoint;
            Console.WriteLine(firstOutPoint.Hash); // f13dc48fb035bbf0a6e989a26b3ecb57b84f85e0836e777d6edf60d87a4a2d94
            Console.WriteLine(firstOutPoint.N); // 0

            Console.WriteLine(transaction.Inputs.Count); // 9

            OutPoint firstPreviousOutPoint = transaction.Inputs.First().PrevOut;
            var firstPreviousTransaction = client.GetTransaction(firstPreviousOutPoint.Hash).Result.Transaction;
            Console.WriteLine(firstPreviousTransaction.IsCoinBase); // False

            Money spentAmount = Money.Zero;
            foreach (var spentCoin in spentCoins)
            {
                spentAmount = (Money) spentCoin.Amount.Add(spentAmount);
                Money amount = (Money) spentCoin.Amount;
                Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
            }
            Console.WriteLine(spentAmount.ToDecimal(MoneyUnit.BTC)); // 13.19703492

            Money receivedAmount = Money.Zero;
            foreach (var receivedCoin in receivedCoins)
            {
                receivedAmount = (Money) receivedCoin.Amount.Add(receivedAmount);
                Money amount = (Money) receivedCoin.Amount;
                Console.WriteLine(amount.ToDecimal(MoneyUnit.BTC));
            }
            Console.WriteLine(receivedAmount.ToDecimal(MoneyUnit.BTC));
            // 13.19703492 - the same received as spent, just spending was one shot, receiving were 9 operations

            var fee = transaction.GetFee(spentCoins.ToArray());
            Console.WriteLine(fee);
        }

        private static void Blockchain()
        {
            var bitcoinPrivateKeySecret = new BitcoinSecret("cVX7SpYc8yjNW8WzPpiGTqyWD4eM4BBnfqEm9nwGqJb2QiX9hhdf");
            var network = bitcoinPrivateKeySecret.Network;
            var address = bitcoinPrivateKeySecret.GetAddress();

            Console.WriteLine(bitcoinPrivateKeySecret); // cVX7SpYc8yjNW8WzPpiGTqyWD4eM4BBnfqEm9nwGqJb2QiX9hhdf
            Console.WriteLine(address);
            // mtjeFt6dMKqvQmYKcBAkSX9AmX8qdynVKN - THIS is my public bitcoin testnet address!!!!!!!
            Console.WriteLine(network);

            //tx IDS: 7a73b2fa89315be9a1292bf817c5e97296cdd14019c112ebbdf2cbfeecc60e7d
            // 9ed5bc5b2b8c81bcf6585377fb20b4b5b82eb413981d3e01145c7b1c55c3179c
            // a034c8d913960e1135175f915ad1bb32f2a13270fd73ece1c6d717f2f3d1c2b4
            // e6beb1f58a14d2fc2dc041329cd09cadd86e364dbc46ebd950228a0740c00b9c

            // transaction inspection:
            // for test-net http://tapi.qbit.ninja/transactions/9ed5bc5b2b8c81bcf6585377fb20b4b5b82eb413981d3e01145c7b1c55c3179c
            // or for main-net http://api.qbit.ninja/transactions/....

            var client = new QBitNinjaClient(network);
            var transactionId = uint256.Parse("e6beb1f58a14d2fc2dc041329cd09cadd86e364dbc46ebd950228a0740c00b9c");
            // from this fking string we get all transaction info!
            var transactionResponse = client.GetTransaction(transactionId).Result;

            var receivedCoins = transactionResponse.ReceivedCoins;
            OutPoint outPointToSpend = null;
            foreach (var coin in receivedCoins)
            {
                if (coin.TxOut.ScriptPubKey == bitcoinPrivateKeySecret.ScriptPubKey)
                {
                    outPointToSpend = coin.Outpoint;
                }
            }
            if (outPointToSpend == null)
                throw new Exception("TxOut doesn't contain our ScriptPubKey");
            Console.WriteLine("We want to spend {0}. outpoint:", outPointToSpend.N + 1);

            Console.WriteLine(transactionResponse.TransactionId);
            // 9ed5bc5b2b8c81bcf6585377fb20b4b5b82eb413981d3e01145c7b1c55c3179c
            Console.WriteLine(transactionResponse.Block != null
                ? transactionResponse.Block.Confirmations.ToString()
                : "null block");

            var transaction = new Transaction();

            TxIn txIn = new TxIn
            {
                PrevOut = outPointToSpend
            };
            transaction.Inputs.Add(txIn);

            //blockexplorer of addresses or transactions/blocks
            // https://testnet.blockexplorer.com/address/mzp4No5cmCXjZUpf112B1XWsvWBfws5bbB
            // https://live.blockcypher.com/btc-testnet/address/mtjeFt6dMKqvQmYKcBAkSX9AmX8qdynVKN/
            var hallOfTheMakersAddress = BitcoinAddress.Create("mzp4No5cmCXjZUpf112B1XWsvWBfws5bbB");

            TxOut hallOfTheMakersTxOut = new TxOut
            {
                Value = new Money(7, MoneyUnit.Satoshi),
                ScriptPubKey = hallOfTheMakersAddress.ScriptPubKey
            };
            var minerFee = new Money(500, MoneyUnit.Satoshi); // accepts 500!
            TxOut changeBackTxOut = new TxOut
            {
                Value = (Money) receivedCoins[(int) outPointToSpend.N].Amount - hallOfTheMakersTxOut.Value - minerFee,
                ScriptPubKey = bitcoinPrivateKeySecret.ScriptPubKey
            };
            transaction.Outputs.Add(hallOfTheMakersTxOut);
            transaction.Outputs.Add(changeBackTxOut);

            transaction.Outputs.Add(new TxOut
            {
                Value = Money.Zero,
                ScriptPubKey =
                    TxNullDataTemplate.Instance.GenerateScriptPubKey(Encoding.UTF8.GetBytes("danson loves NBitcoin!"))
            });

            transaction.Inputs[0].ScriptSig = bitcoinPrivateKeySecret.ScriptPubKey;
            //transaction.Sign(bitcoinPrivateKey, false);
            transaction.Sign(bitcoinPrivateKeySecret, false);

            //return;

            // qbit ninja client works
            BroadcastResponse broadcastResponse = client.Broadcast(transaction).Result;

            if (!broadcastResponse.Success)
            {
                Console.Error.WriteLine("ErrorCode: " + broadcastResponse.Error.ErrorCode);
                Console.Error.WriteLine("Error message: " + broadcastResponse.Error.Reason);
            }
            else
            {
                Console.WriteLine("Success! You can check out the hash of the transaciton in any block explorer:");
                Console.WriteLine(transaction.GetHash());
                // my first successful transactions on testnet!
                // d9d7e05bf7a1d66bc0324824bf898d2fdd6771b2fc676eaa98efa04bd94312aa
                // 71d6f6193fc0d0959f47e7d05cc2772ad0493298120d0d7d68000a0b231665eb
                // 773d6ca5b18b875602f0058435422ed16a150c05fba8d086a599a361227ad047
                // https://live.blockcypher.com/btc-testnet/tx/d9d7e05bf7a1d66bc0324824bf898d2fdd6771b2fc676eaa98efa04bd94312aa/
                // https://live.blockcypher.com/btc-testnet/address/mtjeFt6dMKqvQmYKcBAkSX9AmX8qdynVKN/
            }

            // much slower version - direct library
            //using (var node = Node.Connect(network)) //Connect to the node
            //{
            //    node.VersionHandshake(); //Say hello
            //                             //Advertize your transaction (send just the hash)
            //    node.SendMessage(new InvPayload(InventoryType.MSG_TX, transaction.GetHash()));
            //    //Send it
            //    node.SendMessage(new TxPayload(transaction));
            //    Thread.Sleep(500); //Wait a bit
            //}
        }
    }
}
