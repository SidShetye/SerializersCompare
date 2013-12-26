SerializersCompare
==================
Showcasing a few C# text and binary serializers for performance and size. Feel free to modify and improve.

Results
-------

					 ProtoBuf                Avro              Thrift         MessagePack       Json.NET BSON    Binary Formatter            Json.NET    ServiceStackJson     ServiceStackJSV            Xml .NET
	 Loop      Size:163 bytes      Size:149 bytes      Size:149 bytes      Size:238 bytes      Size:294 bytes     Size:1024 bytes      Size:298 bytes      Size:298 bytes      Size:266 bytes      Size:579 bytes
		1         181.1013 ms          85.2919 ms          46.1071 ms         370.6799 ms          78.7192 ms          11.4846 ms          21.6267 ms         164.8685 ms          27.4015 ms         189.3590 ms
		2           0.0320 ms           0.0350 ms           0.0403 ms           0.0585 ms           0.0858 ms           0.0566 ms           0.0066 ms           0.0476 ms           0.0335 ms           0.1265 ms
		4           0.0098 ms           0.0123 ms           0.0608 ms           0.0342 ms           0.0389 ms           0.0378 ms           0.0784 ms           0.0195 ms           0.0176 ms           0.0991 ms
		8           0.0083 ms           0.0108 ms           0.0664 ms           0.0319 ms           0.0386 ms           0.0343 ms           0.0326 ms           0.0170 ms           0.0169 ms           0.0964 ms
	   16           0.0077 ms           0.0116 ms           0.0516 ms           0.0348 ms           0.0367 ms           0.0372 ms           0.0326 ms           0.0144 ms           0.0162 ms           0.0942 ms
	   32           0.0075 ms           0.0099 ms           0.0545 ms           0.0318 ms           0.0370 ms           0.0329 ms           0.0322 ms           0.0160 ms           0.0154 ms           0.1345 ms
	   64           0.0075 ms           0.0098 ms           0.0505 ms           0.0306 ms           0.0370 ms           0.0470 ms           0.0326 ms           0.0158 ms           0.0150 ms           0.0927 ms
	  128           0.0073 ms           0.0158 ms           0.0424 ms           0.0304 ms           0.0356 ms           0.0266 ms           0.0325 ms           0.0157 ms           0.0203 ms           0.1056 ms
	  256           0.0074 ms           0.0099 ms           0.0455 ms           0.0326 ms           0.0370 ms           0.0414 ms           0.0353 ms           0.0192 ms           0.0151 ms           0.1004 ms
	  512           0.0074 ms           0.0107 ms           0.0434 ms           0.0351 ms           0.0396 ms           0.0364 ms           0.0333 ms           0.0155 ms           0.0211 ms           0.1087 ms
	 1024           0.0079 ms           0.0101 ms           0.0491 ms           0.0320 ms           0.0402 ms           0.0344 ms           0.0325 ms           0.0163 ms           0.0175 ms           0.0990 ms
	 2048           0.0082 ms           0.0104 ms           0.0409 ms           0.0316 ms           0.0379 ms           0.0343 ms           0.0341 ms           0.0177 ms           0.0184 ms           0.0973 ms
	 4096           0.0078 ms           0.0104 ms           0.0404 ms           0.0315 ms           0.0387 ms           0.0341 ms           0.0343 ms           0.0164 ms           0.0167 ms           0.0995 ms
	 8192           0.0074 ms           0.0105 ms           0.0401 ms           0.0318 ms           0.0382 ms           0.0343 ms           0.0338 ms           0.0164 ms           0.0155 ms           0.0990 ms
	If the above looks messy, please set your console width to over 200 and rerun this program
	Options: (T)est, (R)esults, (D)eserializer output, (E)xit

Notes
-----
1. Avro as used here is skipping code-gen phase, so schema mapping is done at runtim. Can be improved to do code-gen first and then run tests (more representative of production usage)

2. Thrift complains about losing data in the serialization->deserialization process. IGNORE this, it's an artifact of injecting a float to a double - the mapping library sees them as separate fields so doesn't.