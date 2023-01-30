using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using Unity.VisualScripting;

public class RelayTest : MonoBehaviour
{
	public static RelayTest Instance { get; private set; }
	private int maxConnections = 4;
	static string joincode;
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance= this;
		}
	}
	private async void Start()
	{
		await UnityServices.InitializeAsync();

		await AuthenticationService.Instance.SignInAnonymouslyAsync();
	}

	public async void CreateRelay()
	{
		try
		{
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
			joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

			NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
				allocation.RelayServer.IpV4,
				(ushort)allocation.RelayServer.Port,
				allocation.AllocationIdBytes,
				allocation.Key,
				allocation.ConnectionData
				);
			//NetworkManager.Singleton.StartHost();
		}
		catch (RelayServiceException e)
		{
			Debug.LogError(e);
			throw;
		}
	}

	public async void joinRelay(string joinCode)
	{
		try
		{
			JoinAllocation joinallocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
				joinallocation.RelayServer.IpV4,
				(ushort)joinallocation.RelayServer.Port,
				joinallocation.AllocationIdBytes,
				joinallocation.Key,
				joinallocation.ConnectionData,
				joinallocation.HostConnectionData
			);
			NetworkManager.Singleton.StartClient();
		}
		catch (RelayServiceException e)
		{
			Debug.LogError(e);
			throw;
		}
	}
}
