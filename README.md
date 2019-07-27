# Enigma Binary Cipher

![Version 1.0.0](https://img.shields.io/badge/Version-1.0.0-brightgreen.svg) ![License MIT](https://img.shields.io/badge/Licence-MIT-blue.svg) [![Build Status](https://travis-ci.org/FredEkstrand/EnigmaBinaryCipher.svg?branch=master)](https://travis-ci.org/FredEkstrand/EnigmaBinaryCipher) [![HitCount](http://hits.dwyl.io/fredekstrand/EnigmaBinaryCipher.svg)](http://hits.dwyl.io/fredekstrand/EnigmaBinaryCipher)

<img src="https://github.com/FredEkstrand/ImageFiles/raw/master/BinaryEnigma.png" width=194 height=278 />

# Overview
The Enigma binary cipher presented here is a symmetric key stream cipher based on the German WWII Enigma cipher.

# Features
The Enigma binary cipher provides a number of configuration abilities to provide a unique and strong encryption.
* API provides an interface for a custom rotor.
* API provides an interface for a custom plug-board.
* API provides an interface for a custom reflector.
* API provides an interface for a custom entry rotor.
* API provides an interface for a custom rotor controller.
* API for generating rotors, entry rotor, reflector, and plug-board end points.
* API for generating initial rotor starting point, the symmetric key.
* API for saving and loading configuration of all components.

# Getting started

The source code is written in C# and targeted for the .Net Framework 4.0 and later. There are two options to obtain the dll.

### Option 1:
You can download the DLL [here](https://github.com/FredEkstrand/EnigmaBinaryCipher/releases).
### Option 2:
Download the entire project and compile. Then add a reference to the dll for use in your project.

# Usage
Once downloaded add a reference to the dll in your Visual Studio project.
Then in your code file add the following to the collection of using statement.

```csharp
using Ekstrand;
```
{ additional code examples please }

# Tests

# Code Documentation
MSDN-style code documentation can be found [here](http://fredekstrand.github.io/EnigmaBinaryCipher).

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
