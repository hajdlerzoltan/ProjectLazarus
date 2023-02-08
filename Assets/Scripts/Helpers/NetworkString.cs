using System;
using Unity.Collections;
using Unity.Netcode;

public struct NetworkString : INetworkSerializable, IEquatable<NetworkString>
{
	public string stringvar;
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		if (serializer.IsReader)
		{
			var reader = serializer.GetFastBufferReader();
			reader.ReadValueSafe(out stringvar);
		}
		else 
		{
			var writer = serializer.GetFastBufferWriter();
			writer.WriteValueSafe(stringvar);
		}
	}

	//public override string ToString()
	//{
	//	return stringvar.ToString();
	//}

	public bool Equals(NetworkString other) 
	{
		if (string.Equals(other.stringvar,stringvar,StringComparison.CurrentCultureIgnoreCase))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	//public static implicit operator string(NetworkString s) => s.ToString();
	//public static implicit operator NetworkString(string s) => new NetworkString() { stringvar = s };
}
