using Unity.Collections;
using Unity.Netcode;

//public struct NetworkString : INetworkSerializable
//{
//    private FixedString32Bytes stringvar;
//	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//	{
//		serializer.SerializeValue(ref stringvar);
//	}

//	public override string ToString()
//	{
//		return stringvar.ToString();
//	}

//	public static implicit operator string(NetworkString s) => s.ToString();
//	public static implicit operator NetworkString(string s) => new NetworkString() { stringvar = new FixedString32Bytes(s) };
//}
