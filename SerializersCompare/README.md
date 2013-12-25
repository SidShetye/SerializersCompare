SerializersCompare
==================
Showcasing a few C# text and binary serializers for performance and size. Feel free to modify and improve.

Results
-------

					 ProtoBuf                Avro         MessagePack       Json.NET BSON    Binary Formatter            Json.NET    ServiceStackJson     ServiceStackJSV            Xml .NET
	 Loop      Size:163 bytes      Size:149 bytes      Size:238 bytes      Size:294 bytes     Size:1024 bytes      Size:298 bytes      Size:298 bytes      Size:266 bytes      Size:579 bytes
		1         194.2969 ms          85.9490 ms         362.9503 ms          78.7538 ms          12.2704 ms          21.0989 ms         168.7085 ms          28.0219 ms         188.9359 ms
		2           0.0319 ms           0.0292 ms           0.0542 ms           0.0811 ms           0.2912 ms           0.0023 ms           0.0406 ms           0.0385 ms           0.1359 ms
		4           0.0101 ms           0.0146 ms           0.0334 ms           0.0405 ms           0.0384 ms           0.0346 ms           0.0189 ms           0.0181 ms           0.1335 ms
		8           0.0085 ms           0.0131 ms           0.0031 ms           0.0031 ms           0.0340 ms           0.1435 ms           0.0172 ms           0.0170 ms           0.0977 ms
	   16           0.0078 ms           0.0122 ms           0.0496 ms           0.0406 ms           0.0336 ms           0.0330 ms           0.0165 ms           0.0163 ms           0.1699 ms
	   32           0.0082 ms           0.0124 ms           0.0311 ms           0.0443 ms           0.0327 ms           0.0331 ms           0.0163 ms           0.0153 ms           0.0946 ms
	   64           0.0074 ms           0.0129 ms           0.0317 ms           0.0586 ms           0.0330 ms           0.0321 ms           0.0161 ms           0.0153 ms           0.1016 ms
	  128           0.0074 ms           0.0135 ms           0.0311 ms           0.0381 ms           0.0356 ms           0.0320 ms           0.0237 ms           0.0150 ms           0.1066 ms
	  256           0.0073 ms           0.0210 ms           0.0303 ms           0.0381 ms           0.0335 ms           0.0329 ms           0.0127 ms           0.0154 ms           0.1190 ms
	  512           0.0074 ms           0.0109 ms           0.0303 ms           0.0434 ms           0.0346 ms           0.0346 ms           0.0173 ms           0.0163 ms           0.1018 ms
	If the above looks messy, please set your console width to over 200 and rerun this program
	Options: (T)est, (R)esults, (D)eserializer output, (E)xit

Notes
-----

Avro as used here is skipping code-gen phase, so schema mapping is done at runtim. Can be improved to do code-gen first
and then run tests (more representative of production usage)