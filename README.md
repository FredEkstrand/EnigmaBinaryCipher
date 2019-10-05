# Enigma Binary Cipher

![Version 1.0.0](https://img.shields.io/badge/Version-1.0.0-brightgreen.svg) ![License MIT](https://img.shields.io/badge/Licence-MIT-blue.svg) [![HitCount](http://hits.dwyl.io/fredekstrand/EnigmaBinaryCipher.svg)](http://hits.dwyl.io/fredekstrand/EnigmaBinaryCipher) [![Build status](https://ci.appveyor.com/api/projects/status/kqnjdqb2cc7i7xo1?svg=true)](https://ci.appveyor.com/project/FredEkstrand/enigmabinarycipher)

 ![image](https://github.com/FredEkstrand/ImageFiles/raw/master/BinaryEnigma.png)

# Overview
The Enigma binary cipher presented here is a symmetric key stream cipher based on the German WWII Enigma cipher.

# Features
The Enigma binary cipher provides a number of configuration abilities to provide a unique and strong encryption.
* Provides an API for developing custom: rotor, plug-board, reflector, and entry rotor.
* Provides methods to generate entry rotor, rotors, plug-board, and reflector end points.
* Provides auto generation of entry rotor, rotors, plug-board, and reflector end points for quick use.
* Provides auto generated initial rotors starting point, the symmetric key. This is because human beings are poor random number generators.
* Settings can be exported or imported.

# Getting started
The souce code is written in C# and targeted for the .Net Framework 4.0 and later. Download the entire project and compile.

# Usage
Once you have compiled the project reference the dll in your Visual Studio project.
Then in your code file add the following to the collection of using statement.

```csharp
using Ekstrand;
```

Encryption example:
```csharp
Byte[] m_ToEncryptMsg;	  // Byte array to be encrypted.
Byte[] m_EncryptedMsg; // Byte array for encrypted.
EnigmaBinary ebc = new EnigmaBinary();	// Create a new instance.
ebc.Init();		// Initialize the cipher which includes generating the entry rotor, rotors, plug-board, and reflector end points.
EnigmaBinaryParameters ebp = ebc.ReturnConfiguration();	// Create a enigma binary parameters object which would have all the cipher settings.

//Method parameters: byte[] toEncrypet, starting index, end index, byte[] encrypted, starting index
ebc.ProcessBytes(m_ToEncryptMsg, 0, m_ToEncryptMsg.Length, m_EncryptedMsg, 0);
```
Decryption example:
```csharp
Byte[] m_EncryptMsg;	// Byte array to be decrypted.
Byte[] m_DecryptedMsg;	// Byte array for decrypted.
EnigmaBinary ebc = new EnigmaBinary();	// Create a new instance.
ebc.Init(false,ebp);	// Initialize the cipher for decryption using cipher configuration parameters.

//Method parameters: byte[] To_decrypt, starting index, end index, byte[] decrypted, starting index
ebc.ProcessBytes(m_EncryptMsg, 0, m_EncryptMsg.Length, m_DecryptedMsg, 0);
```

# Tests
The enigma binary cipher code is tested using NUnit.
The NUnit testing consists of Unit-testing to integration testing of all logical components and is included as part of the project code.

# Code Documentation
MSDN-style code documentation can be found [here](http://fredekstrand.github.io/ClassDocEnigmaBinaryCipher).

# History
 1.0.0 Initial release into the wild.

# Contributing

If you'd like to contribute, please fork the repository and use a feature
branch. Pull requests are always welcome.

# Contact
Fred Ekstrand
email: fredekstrandgithub@gmail.com

# Licensing

This project is licensed under the MIT License - see the LICENSE.md file for details.
