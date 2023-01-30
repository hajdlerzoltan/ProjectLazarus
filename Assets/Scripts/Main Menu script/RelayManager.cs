using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;


public class RelayManager : MonoBehaviour
{
	[SerializeField]
	private string environment = "production";

	[SerializeField]
	private int maxConnections = 4;
	string joincode;

	private async void Start()
	{
		await UnityServices.InitializeAsync();

		await AuthenticationService.Instance.SignInAnonymouslyAsync();
	}

	//public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
	//public bool IsRelayEnabled => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;
	//public async Task<RelayHostData> StartRelay()
	//{
	//	Debug.Log($"relay server created with {maxConnections} max connections");
	//	InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);
	//	await UnityServices.InitializeAsync(options);
	//	if (!AuthenticationService.Instance.IsSignedIn)
	//	{
	//		await AuthenticationService.Instance.SignInAnonymouslyAsync();
	//	}

	//	Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnections);

	//	RelayHostData relayHostData = new RelayHostData 
	//	{
	//		Key = allocation.Key,
	//		Port = (ushort)allocation.RelayServer.Port,
	//		AllocationID = allocation.AllocationId,
	//		AllocationIDBytes= allocation.AllocationIdBytes,
	//		IPv4Address = allocation.RelayServer.IpV4,
	//		ConnectionData= allocation.ConnectionData,
	//	};

	//	relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);

	//	Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port, relayHostData.AllocationIDBytes, relayHostData.Key, relayHostData.ConnectionData);
	//	Debug.Log($"relay server join code {relayHostData.JoinCode}");
	//	return relayHostData;
	//}

	//public async Task<RelayJoinData> JoinRelay(string joinCode) 
	//{
	//	InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);
	//	await UnityServices.InitializeAsync(options);
	//	if (!AuthenticationService.Instance.IsSignedIn)
	//	{
	//		await AuthenticationService.Instance.SignInAnonymouslyAsync();
	//	}

	//	JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
	//	RelayJoinData relayJoinData = new RelayJoinData 
	//	{
	//		Key = allocation.Key,
	//		Port = (ushort)allocation.RelayServer.Port,
	//		AllocationID = allocation.AllocationId,
	//		AllocationIDBytes = allocation.AllocationIdBytes,
	//		IPv4Address = allocation.RelayServer.IpV4,
	//		ConnectionData = allocation.ConnectionData,
	//		HostConnectionData= allocation.HostConnectionData,
	//		JoinCode= joinCode
	//	};

	//	Transport.SetRelayServerData(relayJoinData.IPv4Address,relayJoinData.Port,relayJoinData.AllocationIDBytes,relayJoinData.Key,
	//		relayJoinData.ConnectionData,relayJoinData.HostConnectionData);
	//	Debug.Log($"player joind with {joinCode} code");
	//	return relayJoinData;
	//}
}
