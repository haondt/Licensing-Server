# Licensing-Server

# Dependencies
## Python
* PyCrypto

## C#
* NewtonSoft.Json
* BouncyCastle.Crypto
* System.Management

# Run instructions
## Server
```
Start server:
screen
sh run.sh
Ctrl-a d

Kill server:
screen -r
Ctrl-c
ps -a
kill [pid]
exit

Generate license:
python3 generator.py gen-license
```

## Client
* Click buttons to load license key and find HWID
* Click button to verify license
