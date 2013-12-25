SerializersCompare
==================
Showcasing a few C# text and binary serializers for performance and size. Feel free to modify and improve.

Results
-------

				  MessagePack            Json.NET       Json.NET BSON    ServiceStackJson     ServiceStackJSV            Xml .NET    Binary Formatter            ProtoBuf
	 Loop      Size:238 bytes      Size:298 bytes      Size:294 bytes      Size:298 bytes      Size:266 bytes      Size:579 bytes     Size:1024 bytes      Size:163 bytes
		1         413.5734 ms          68.8257 ms          32.5049 ms         192.9757 ms          30.0568 ms         198.8418 ms          13.1886 ms         145.8051 ms
		2          30.2716 ms           0.0792 ms           0.0033 ms           0.0444 ms           0.0349 ms           0.1469 ms           0.0599 ms           0.0264 ms
		4          29.2147 ms           0.0357 ms           0.0417 ms           0.0298 ms           0.0193 ms           0.1106 ms           0.0712 ms           0.0108 ms
		8          30.1866 ms           0.0415 ms           0.0400 ms           0.0252 ms           0.0316 ms           0.1031 ms           0.0357 ms           0.0091 ms
	   16          28.9911 ms           0.0332 ms           0.0401 ms           0.0166 ms           0.0159 ms           0.1030 ms           0.0353 ms           0.0085 ms
	   32          29.4548 ms           0.0351 ms           0.0392 ms           0.0180 ms           0.0177 ms           0.1988 ms           0.0352 ms           0.0081 ms
	   64          30.3773 ms           0.0340 ms           0.0437 ms           0.0175 ms           0.0160 ms           0.1506 ms           0.0341 ms           0.0081 ms
	If the above looks messy, please set your console width to over 200 and rerun this program
	Options: (T)est, (R)esults, (D)eserializer output, (E)xit

Note
---
The MessagePack library is use here is about 1000x slow, so its disabled in SerializersCompare.Test.RunTests(). This is likely because they aren't caching results resulting in a cold-hit on each run.