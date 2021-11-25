Enum's pitfalls in TS API.

Be careful when using enums for database Items and Contracts.
If enum contains an underlying integer value, that is not defined in enum, then there will be a serialization exception thrown by JilSerializer.
So, if you're not sure that database Item or Contract with enum property will always contain defined value - use int instead.
